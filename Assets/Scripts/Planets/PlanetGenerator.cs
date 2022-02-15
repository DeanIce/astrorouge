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


        public float scale;

        public ResolutionSettings resolutionSettings;

        public PreviewMode previewMode;
        public ShapeSettings shape;
        public ShaderSettings shader;
        public bool logTimers = true;

        private readonly bool debugDoubleUpdate = true;

        // Game mode data 
        private int activeLODIndex = -1;
        private Mesh collisionMesh;
        private int debugNumUpdates;

        private Vector2 heightMinMax;
        private Mesh[] lodMeshes;
        private Mesh previewMesh;
        private bool shaderSettingsUpdated;
        private bool shapeSettingsUpdated;
        private Material terrainMatInstance;

        private ComputeBuffer vertexBuffer;

        //private MeshFilter terrainMeshFilter;
        public MeshFilter terrainMeshFilter { get; private set; }

        private bool InGameMode => Application.isPlaying;

        private bool InEditMode => !Application.isPlaying;

        public float bodyScale =>
            // Body radius is determined by the celestial body class,
            // which sets the local scale of the generator object (this object)
            transform.localScale.x;

        // Start is called before the first frame update
        private void Start()
        {
            if (InGameMode)
            {
                HandleGameModeGeneration();
                SetLOD(1);
            }
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

            if (shader != null)
            {
                shader.OnSettingChanged -= OnShaderSettingChanged;
                shader.OnSettingChanged += OnShaderSettingChanged;
            }

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
                    shaderSettingsUpdated = false;
                    Dummy();

                    var terrainMeshTimer = Stopwatch.StartNew();
                    heightMinMax = GenerateTerrainMesh(ref previewMesh, PickTerrainRes());


                    LogTimer(terrainMeshTimer, "Generate terrain mesh");
                    DrawEditModeMesh();
                }
                // If only shading noise has changed, update it separately from shape to save time
                else if (shaderSettingsUpdated)
                {
                    shaderSettingsUpdated = false;
                    shader.Initialize(shape);

                    // Sometimes when changing a colour property, invalid data is returned from compute shader
                    // Running the shading a second time fixes it.
                    // Not sure if this is my bug, or Unity's (TODO: investigate)
                    debugNumUpdates++;
                    if (debugDoubleUpdate && debugNumUpdates < 2)
                    {
                        shaderSettingsUpdated = true;
                        HandleEditModeGeneration();
                    }

                    if (debugNumUpdates == 2) debugNumUpdates = 0;
                }
            }

            // Update shading
            if (shader != null && shader.terrainMaterial != null)
            {
                // Set material properties
                shader.Initialize(shape);
                shader.SetTerrainProperties(shader?.terrainMaterial, heightMinMax, 1f);
            }

            ReleaseAllBuffers(); //
        }

        private void DrawEditModeMesh()
        {
            var _ = GetOrCreateMeshObject("Terrain Mesh", previewMesh, shader.terrainMaterial);
        }

        // Gets child object with specified name.
        // If it doesn't exist, then creates object with that name, adds mesh renderer/filter and attaches mesh and material
        private GameObject GetOrCreateMeshObject(string gameObjectName, Mesh mesh, Material material)
        {
            // Find/create object
            var child = transform.Find(gameObjectName);
            if (!child)
            {
                print("Creating new Terrain Mesh.");
                child = new GameObject(gameObjectName).transform;
                child.parent = transform;
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
                child.localScale = Vector3.one;
                // child.gameObject.layer = gameObject.layer;
                child.gameObject.layer = LayerMask.GetMask("Ground");
            }

            // Add mesh components
            if (!child.TryGetComponent(out MeshFilter meshFilter))
                meshFilter = child.gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            if (!child.TryGetComponent(out MeshRenderer meshRenderer))
                meshRenderer = child.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;

            child.transform.localScale = new Vector3(scale, scale, scale);

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

            // Perturb vertices to give terrain a less perfectly smooth appearance
            if (shape.perturbVertices && shape.perturbCompute)
            {
                var perturbShader = shape.perturbCompute;
                var maxperturbStrength = shape.perturbStrength * edgeLength / 2;

                perturbShader.SetBuffer(0, "points", vertexBuffer);
                perturbShader.SetInt("numPoints", vertices.Length);
                perturbShader.SetFloat("maxStrength", maxperturbStrength);

                ComputeHelper.Run(perturbShader, vertices.Length);
                var pertData = new Vector3[vertices.Length];
                vertexBuffer.GetData(vertices);
            }


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


            // Create crude tangents (vectors perpendicular to surface normal)
            // This is needed (even though normal mapping is being done with triplanar)
            // because surface shader wants normals in tangent space
            var normals = mesh.normals;
            var crudeTangents = new Vector4[mesh.vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                var normal = normals[i];
                crudeTangents[i] = new Vector4(-normal.z, 0, normal.x, 1);
            }

            mesh.SetTangents(crudeTangents);

            // PROCEDURALLY SPAWN HERE?
            /*for (int i = 0; i < environmentAssets.Length; i++)
            {
                //SpawnObject(vertices, environmentAssets[i], numOfAsset[i]);
            }*/

            return new Vector2(minHeight, maxHeight);
        }

        // Generate sphere (or reuse if already generated) and return a copy of the vertices and triangles
        private (Vector3[] vertices, int[] triangles) CreateSphereVertsAndTris(int resolution)
        {
            sphereGenerators ??= new Dictionary<int, SphereMesh>();

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

        private int PickTerrainRes()
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

        public void SetLOD(int lodIndex)
        {
            if (lodIndex != activeLODIndex && terrainMeshFilter)
            {
                activeLODIndex = lodIndex;
                terrainMeshFilter.sharedMesh = lodMeshes[lodIndex];
            }
        }

        // Handles creation of celestial body when entering game mode
        // This differs from the edit-mode version in the following ways:
        // • creates all LOD meshes and stores them in mesh array (to be picked based on player position)
        // • creates its own instances of materials so multiple bodies can exist with their own shading
        // • doesn't support updating of shape/shading values once generated
        private void HandleGameModeGeneration()
        {
            if (true /*CanGenerateMesh()*/)
            {
                var lodTimer = Stopwatch.StartNew();
                Dummy();

                // Generate LOD meshes
                lodMeshes = new Mesh[ResolutionSettings.NumLODLevels];
                for (var i = 0; i < lodMeshes.Length; i++)
                {
                    var lodTerrainHeightMinMax =
                        GenerateTerrainMesh(ref lodMeshes[i], resolutionSettings.GetLODResolution(i));
                    // Use min/max height of first (most detailed) LOD
                    if (i == 0) heightMinMax = lodTerrainHeightMinMax;
                }

                // Generate collision mesh
                GenerateCollisionMesh(resolutionSettings.collider);

                // Create terrain renderer and set shading properties on the instanced material
                terrainMatInstance = new Material(shader.terrainMaterial);
                shader.Initialize(shape);
                shader.SetTerrainProperties(terrainMatInstance, heightMinMax, bodyScale);
                var terrainHolder = GetOrCreateMeshObject("Terrain Mesh", null, terrainMatInstance);
                terrainMeshFilter = terrainHolder.GetComponent<MeshFilter>();
                LogTimer(lodTimer, "Generate all LODs");

                // Add collider
                MeshCollider collider;
                if (!terrainHolder.TryGetComponent(out collider)) collider = terrainHolder.AddComponent<MeshCollider>();

                var collisionBakeTimer = Stopwatch.StartNew();
                MeshBaker.BakeMeshImmediate(collisionMesh);
                collider.sharedMesh = collisionMesh;
                LogTimer(collisionBakeTimer, "Bake Mesh collider");
            }
            else
            {
                Debug.Log("Could not generate mesh");
            }

            ReleaseAllBuffers();
        }

        private void GenerateCollisionMesh(int resolution)
        {
            var (vertices, triangles) = CreateSphereVertsAndTris(resolution);
            ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, vertices);

            // Set heights
            var heights = shape.CalculateHeights(vertexBuffer);
            for (var i = 0; i < vertices.Length; i++)
            {
                var height = heights[i];
                vertices[i] *= height;
            }

            // Create mesh
            CreateMesh(ref collisionMesh, vertices.Length);
            collisionMesh.vertices = vertices;
            collisionMesh.triangles = triangles;
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

        private void OnShapeSettingChanged()
        {
            shapeSettingsUpdated = true;
        }

        private void OnShaderSettingChanged()
        {
            shaderSettingsUpdated = true;
        }

        private bool CanGenerateMesh()
        {
            return ComputeHelper.CanRunEditModeCompute && shape != null && shape.heightMapCompute;
        }
    }
}