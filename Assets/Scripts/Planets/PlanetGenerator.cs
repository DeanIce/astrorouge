using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;
using Debug = UnityEngine.Debug;

namespace Planets
{
    [ExecuteInEditMode]
    public class PlanetGenerator : MonoBehaviour
    {
        public enum PreviewMode
        {
            LOD0,
            LOD1,
            LOD2,
            CollisionRes
        }

        private static Dictionary<int, SphereMesh> sphereGenerators;

        public ResolutionSettings resolutionSettings;

        public PreviewMode previewMode;
        public ShapeSettings shape;
        public ShaderSettings shader;
        public bool logTimers = true;
        private readonly bool debugDoubleUpdate = true;
        private int debug_numUpdates;

        private Vector2 heightMinMax;
        private Mesh previewMesh;
        private bool shadingNoiseSettingsUpdated;
        private bool shapeSettingsUpdated;

        private ComputeBuffer vertexBuffer;

        private bool InGameMode => Application.isPlaying;

        private bool InEditMode => !Application.isPlaying;

        public float BodyScale =>
            // Body radius is determined by the celestial body class,
            // which sets the local scale of the generator object (this object)
            transform.localScale.x;

        // Start is called before the first frame update
        private void Start()
        {
            if (InGameMode) HandleGameModeGeneration();
        }

        // Update is called once per frame
        private void Update()
        {
            if (InEditMode) HandleEditModeGeneration();
        }

        private void OnValidate()
        {
            if (shape != null)
            {
                shape.OnSettingChanged -= OnShapeSettingChanged;
                shape.OnSettingChanged += OnShapeSettingChanged;
            }
            // if (body.shading) {
            //     body.shading.OnSettingChanged -= OnShadingNoiseSettingChanged;
            //     body.shading.OnSettingChanged += OnShadingNoiseSettingChanged;
            // }

            if (resolutionSettings != null) resolutionSettings.ClampResolutions();

            OnShapeSettingChanged();
        }


        private void HandleEditModeGeneration()
        {
            if (InEditMode)
            {
                ComputeHelper.shouldReleaseEditModeBuffers -= ReleaseAllBuffers;
                ComputeHelper.shouldReleaseEditModeBuffers += ReleaseAllBuffers;
            }

            if (CanGenerateMesh())
            {
                // Update shape settings and shading noise
                if (shapeSettingsUpdated)
                {
                    shapeSettingsUpdated = false;
                    shadingNoiseSettingsUpdated = false;
                    Dummy();

                    var terrainMeshTimer = Stopwatch.StartNew();
                    heightMinMax = GenerateTerrainMesh(ref previewMesh, PickTerrainRes());


                    LogTimer(terrainMeshTimer, "Generate terrain mesh");
                    DrawEditModeMesh();
                }
                // If only shading noise has changed, update it separately from shape to save time
                else if (shadingNoiseSettingsUpdated)
                {
                    shadingNoiseSettingsUpdated = false;
                    ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, previewMesh.vertices);
                    shader.Initialize(shape);
                    var shadingData = shader.GenerateShadingData(vertexBuffer);
                    previewMesh.SetUVs(0, shadingData);

                    // Sometimes when changing a colour property, invalid data is returned from compute shader
                    // Running the shading a second time fixes it.
                    // Not sure if this is my bug, or Unity's (TODO: investigate)
                    debug_numUpdates++;
                    if (debugDoubleUpdate && debug_numUpdates < 2)
                    {
                        shadingNoiseSettingsUpdated = true;
                        HandleEditModeGeneration();
                    }

                    if (debug_numUpdates == 2) debug_numUpdates = 0;
                }
            }

            // Update shading
            if (shader != null)
            {
                // Set material properties
                shader.Initialize(shape);
                shader.SetTerrainProperties(shader.terrainMaterial, heightMinMax, 1f);
            }

            ReleaseAllBuffers(); //
        }

        private void DrawEditModeMesh()
        {
            var terrainHolder = GetOrCreateMeshObject("Terrain Mesh", previewMesh, shader.terrainMaterial);
            //var terrainHolder =
            //GetOrCreateMeshObject("Terrain Mesh", previewMesh, new Material(Shader.Find("Standard")));
        }

        // Gets child object with specified name.
        // If it doesn't exist, then creates object with that name, adds mesh renderer/filter and attaches mesh and material
        private GameObject GetOrCreateMeshObject(string name, Mesh mesh, Material material)
        {
            // Find/create object
            var child = transform.Find(name);
            if (!child)
            {
                child = new GameObject(name).transform;
                child.parent = transform;
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
                child.localScale = Vector3.one;
                child.gameObject.layer = gameObject.layer;
            }

            // Add mesh components
            MeshFilter filter;
            if (!child.TryGetComponent(out filter)) filter = child.gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;

            MeshRenderer renderer;
            if (!child.TryGetComponent(out renderer)) renderer = child.gameObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;

            return child.gameObject;
        }


        private void LogTimer(Stopwatch sw, string text)
        {
            if (logTimers) Debug.Log(text + " " + sw.ElapsedMilliseconds + " ms.");
        }

        // Generates terrain mesh based on heights generated by the Shape object
        // Shading data from the Shading object is stored in the mesh uvs
        // Returns the min/max height of the terrain
        private Vector2 GenerateTerrainMesh(ref Mesh mesh, int resolution)
        {
            var (vertices, triangles) = CreateSphereVertsAndTris(resolution);
            ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, vertices);

            var edgeLength = (vertices[triangles[0]] - vertices[triangles[1]]).magnitude;

            // Set heights
            var heights = shape.CalculateHeights(vertexBuffer);

            // // Perturb vertices to give terrain a less perfectly smooth appearance
            // if (shapeSettings.perturbVertices && body.shape.perturbCompute) {
            // 	ComputeShader perturbShader = body.shape.perturbCompute;
            // 	float maxperturbStrength = body.shape.perturbStrength * edgeLength / 2;
            //
            // 	perturbShader.SetBuffer (0, "points", vertexBuffer);
            // 	perturbShader.SetInt ("numPoints", vertices.Length);
            // 	perturbShader.SetFloat ("maxStrength", maxperturbStrength);
            //
            // 	ComputeHelper.Run (perturbShader, vertices.Length);
            // 	Vector3[] pertData = new Vector3[vertices.Length];
            // 	vertexBuffer.GetData (vertices);
            // }

            // Calculate terrain min/max height and set heights of vertices
            var minHeight = float.PositiveInfinity;
            var maxHeight = float.NegativeInfinity;
            for (var i = 0; i < heights.Length; i++)
            {
                var height = heights[i];
                vertices[i] *= height;
                minHeight = Mathf.Min(minHeight, height);
                maxHeight = Mathf.Max(maxHeight, height);
            }

            // Create mesh
            CreateMesh(ref mesh, vertices.Length);
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0, true);
            mesh.RecalculateNormals(); //

            // Shading noise data
            // body.shading.Initialize (body.shape);
            var shadingData = shader.GenerateShadingData(vertexBuffer);
            mesh.SetUVs(0, shadingData);

            // Create crude tangents (vectors perpendicular to surface normal)
            // This is needed (even though normal mapping is being done with triplanar)
            // because surfaceshader wants normals in tangent space
            var normals = mesh.normals;
            var crudeTangents = new Vector4[mesh.vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                var normal = normals[i];
                crudeTangents[i] = new Vector4(-normal.z, 0, normal.x, 1);
            }

            mesh.SetTangents(crudeTangents);

            return new Vector2(minHeight, maxHeight);
        }

        // Generate sphere (or reuse if already generated) and return a copy of the vertices and triangles
        private (Vector3[] vertices, int[] triangles) CreateSphereVertsAndTris(int resolution)
        {
            if (sphereGenerators == null) sphereGenerators = new Dictionary<int, SphereMesh>();

            if (!sphereGenerators.ContainsKey(resolution)) sphereGenerators.Add(resolution, new SphereMesh(resolution));

            var generator = sphereGenerators[resolution];

            var vertices = new Vector3[generator.Vertices.Length];
            var triangles = new int[generator.Triangles.Length];
            Array.Copy(generator.Vertices, vertices, vertices.Length);
            Array.Copy(generator.Triangles, triangles, triangles.Length);
            return (vertices, triangles);
        }

        private void CreateMesh(ref Mesh mesh, int numVertices)
        {
            const int vertexLimit16Bit = 1 << (16 - 1); // 65535
            if (mesh == null)
                mesh = new Mesh();
            else
                mesh.Clear();

            mesh.indexFormat = numVertices < vertexLimit16Bit
                ? IndexFormat.UInt16
                : IndexFormat.UInt32;
        }

        public int PickTerrainRes()
        {
            if (!Application.isPlaying)
                switch (previewMode)
                {
                    case PreviewMode.LOD0:
                        return resolutionSettings.lod0;
                    case PreviewMode.LOD1:
                        return resolutionSettings.lod1;
                    case PreviewMode.LOD2:
                        return resolutionSettings.lod2;
                    case PreviewMode.CollisionRes:
                        return resolutionSettings.collider;
                }

            return 0;
        }

        private void HandleGameModeGeneration()
        {
        }

        private void Dummy()
        {
            // Crude fix for a problem I was having where the values in the vertex buffer were *occasionally* all zero at start of game
            // This function runs the compute shader once with single dummy input, after which it seems the problem doesn't occur
            // (Waiting until Time.frameCount > 3 before generating is another gross hack that seems to fix the problem)
            // I don't know why...
            Vector3[] vertices = {Vector3.zero};
            ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, vertices);
            shape.CalculateHeights(vertexBuffer);
        }

        private void ReleaseAllBuffers()
        {
            ComputeHelper.Release(vertexBuffer);
            if (shape != null) shape.ReleaseBuffers();
            // if (body.shading) {
            //     body.shading.ReleaseBuffers ();
            // }
        }

        public void OnShapeSettingChanged()
        {
            shapeSettingsUpdated = true;
        }

        private bool CanGenerateMesh()
        {
            return ComputeHelper.CanRunEditModeCompute && shape != null && shape.heightMapCompute;
        }
    }
}