using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

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

        public ResolutionSettings resolutionSettings;

        public PreviewMode previewMode;
        bool shapeSettingsUpdated;
        public ShapeSettings shapeSettings;

        static Dictionary<int, SphereMesh> sphereGenerators;
        public bool logTimers;

        ComputeBuffer vertexBuffer;
        bool shadingNoiseSettingsUpdated;
        Mesh previewMesh;

        Vector2 heightMinMax;

        // Start is called before the first frame update
        void Start()
        {
            if (InGameMode)
            {
                HandleGameModeGeneration();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (InEditMode)
            {
                HandleEditModeGeneration();
            }
        }


        void HandleEditModeGeneration()
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

                    var terrainMeshTimer = System.Diagnostics.Stopwatch.StartNew();
                    heightMinMax = GenerateTerrainMesh(ref previewMesh, PickTerrainRes());

                    LogTimer(terrainMeshTimer, "Generate terrain mesh");
                    DrawEditModeMesh();
                }
                // If only shading noise has changed, update it separately from shape to save time
                else if (shadingNoiseSettingsUpdated)
                {
                    // shadingNoiseSettingsUpdated = false;
                    // ComputeHelper.CreateStructuredBuffer<Vector3>(ref vertexBuffer, previewMesh.vertices);
                    // body.shading.Initialize(body.shape);
                    // Vector4[] shadingData = body.shading.GenerateShadingData(vertexBuffer);
                    // previewMesh.SetUVs(0, shadingData);
                    //
                    // // Sometimes when changing a colour property, invalid data is returned from compute shader
                    // // Running the shading a second time fixes it.
                    // // Not sure if this is my bug, or Unity's (TODO: investigate)
                    // debug_numUpdates++;
                    // if (debugDoubleUpdate && debug_numUpdates < 2)
                    // {
                    //     shadingNoiseSettingsUpdated = true;
                    //     HandleEditModeGeneration();
                    // }
                    //
                    // if (debug_numUpdates == 2)
                    // {
                    //     debug_numUpdates = 0;
                    // }
                }
            }

            // Update shading
            // if (body.shading)
            // {
            //     // Set material properties
            //     body.shading.Initialize(body.shape);
            //     body.shading.SetTerrainProperties(body.shading.terrainMaterial, heightMinMax, BodyScale);
            // }

            ReleaseAllBuffers(); //
        }
        void DrawEditModeMesh () {
            // GameObject terrainHolder = GetOrCreateMeshObject ("Terrain Mesh", previewMesh, body.shading.terrainMaterial);
            GameObject terrainHolder = GetOrCreateMeshObject ("Terrain Mesh", previewMesh, new Material(Shader.Find("Standard")));
        }

        // Gets child object with specified name.
        // If it doesn't exist, then creates object with that name, adds mesh renderer/filter and attaches mesh and material
        GameObject GetOrCreateMeshObject (string name, Mesh mesh, Material material) {
            // Find/create object
            var child = transform.Find (name);
            if (!child) {
                child = new GameObject (name).transform;
                child.parent = transform;
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
                child.localScale = Vector3.one;
                child.gameObject.layer = gameObject.layer;
            }

            // Add mesh components
            MeshFilter filter;
            if (!child.TryGetComponent<MeshFilter> (out filter)) {
                filter = child.gameObject.AddComponent<MeshFilter> ();
            }
            filter.sharedMesh = mesh;

            MeshRenderer renderer;
            if (!child.TryGetComponent<MeshRenderer> (out renderer)) {
                renderer = child.gameObject.AddComponent<MeshRenderer> ();
            }
            renderer.sharedMaterial = material;

            return child.gameObject;
        }

        
        void LogTimer (System.Diagnostics.Stopwatch sw, string text) {
            if (logTimers) {
                Debug.Log (text + " " + sw.ElapsedMilliseconds + " ms.");
            }
        }

        // Generates terrain mesh based on heights generated by the Shape object
        // Shading data from the Shading object is stored in the mesh uvs
        // Returns the min/max height of the terrain
        Vector2 GenerateTerrainMesh(ref Mesh mesh, int resolution)
        {
            var (vertices, triangles) = CreateSphereVertsAndTris(resolution);
            ComputeHelper.CreateStructuredBuffer<Vector3>(ref vertexBuffer, vertices);

            float edgeLength = (vertices[triangles[0]] - vertices[triangles[1]]).magnitude;

            // Set heights
            float[] heights = shapeSettings.CalculateHeights(vertexBuffer);

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
            float minHeight = float.PositiveInfinity;
            float maxHeight = float.NegativeInfinity;
            for (int i = 0; i < heights.Length; i++)
            {
                float height = heights[i];
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
            // Vector4[] shadingData = body.shading.GenerateShadingData (vertexBuffer);
            // mesh.SetUVs (0, shadingData);

            // Create crude tangents (vectors perpendicular to surface normal)
            // This is needed (even though normal mapping is being done with triplanar)
            // because surfaceshader wants normals in tangent space
            var normals = mesh.normals;
            var crudeTangents = new Vector4[mesh.vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 normal = normals[i];
                crudeTangents[i] = new Vector4(-normal.z, 0, normal.x, 1);
            }

            mesh.SetTangents(crudeTangents);

            return new Vector2(minHeight, maxHeight);
        }
        
        // Generate sphere (or reuse if already generated) and return a copy of the vertices and triangles
        (Vector3[] vertices, int[] triangles) CreateSphereVertsAndTris (int resolution) {
            if (sphereGenerators == null) {
                sphereGenerators = new Dictionary<int, SphereMesh> ();
            }

            if (!sphereGenerators.ContainsKey (resolution)) {
                sphereGenerators.Add (resolution, new SphereMesh (resolution));
            }

            var generator = sphereGenerators[resolution];

            var vertices = new Vector3[generator.Vertices.Length];
            var triangles = new int[generator.Triangles.Length];
            System.Array.Copy (generator.Vertices, vertices, vertices.Length);
            System.Array.Copy (generator.Triangles, triangles, triangles.Length);
            return (vertices, triangles);
        }

        void CreateMesh(ref Mesh mesh, int numVertices)
        {
            const int vertexLimit16Bit = 1 << 16 - 1; // 65535
            if (mesh == null)
            {
                mesh = new Mesh();
            }
            else
            {
                mesh.Clear();
            }

            mesh.indexFormat = (numVertices < vertexLimit16Bit)
                ? UnityEngine.Rendering.IndexFormat.UInt16
                : UnityEngine.Rendering.IndexFormat.UInt32;
        }

        public int PickTerrainRes()
        {
            if (!Application.isPlaying)
            {
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
            }

            return 0;
        }

        void HandleGameModeGeneration()
        {
        }

        void Dummy()
        {
            // Crude fix for a problem I was having where the values in the vertex buffer were *occasionally* all zero at start of game
            // This function runs the compute shader once with single dummy input, after which it seems the problem doesn't occur
            // (Waiting until Time.frameCount > 3 before generating is another gross hack that seems to fix the problem)
            // I don't know why...
            Vector3[] vertices = new Vector3[] {Vector3.zero};
            ComputeHelper.CreateStructuredBuffer<Vector3>(ref vertexBuffer, vertices);
            shapeSettings.CalculateHeights(vertexBuffer);
        }

        bool InGameMode => Application.isPlaying;

        bool InEditMode => !Application.isPlaying;

        void ReleaseAllBuffers()
        {
            ComputeHelper.Release(vertexBuffer);
            if (shapeSettings != null)
            {
                shapeSettings.ReleaseBuffers();
            }
            // if (body.shading) {
            //     body.shading.ReleaseBuffers ();
            // }
        }

        void OnValidate()
        {
            if (shapeSettings != null)
            {
                shapeSettings.OnSettingChanged -= OnShapeSettingChanged;
                shapeSettings.OnSettingChanged += OnShapeSettingChanged;
            }
            // if (body.shading) {
            //     body.shading.OnSettingChanged -= OnShadingNoiseSettingChanged;
            //     body.shading.OnSettingChanged += OnShadingNoiseSettingChanged;
            // }

            if (resolutionSettings != null)
            {
                resolutionSettings.ClampResolutions();
            }

            OnShapeSettingChanged();
        }

        public void OnShapeSettingChanged()
        {
            shapeSettingsUpdated = true;
        }

        bool CanGenerateMesh()
        {
            return ComputeHelper.CanRunEditModeCompute && shapeSettings != null && shapeSettings.heightMapCompute;
        }
    }
}