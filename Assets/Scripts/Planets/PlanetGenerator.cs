using System;
using System.Collections.Generic;
using System.Diagnostics;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;

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

        private const bool debugDoubleUpdate = true;


        private static Dictionary<int, SphereMesh> sphereGenerators;

        public static Dictionary<int, Mesh> meshesToBake = new();

        internal static readonly Dictionary<int, (Vector3[] vertices, int[] triangles)> spheres = new();


        public float scale;

        public ResolutionSettings resolutionSettings;

        public PreviewMode previewMode;
        public ShapeSettings shape;
        public ShaderSettings shader;

        public GameObject terrainMesh;

        public Vector2 heightMinMax;

        public int key;

        // Game mode data 
        private int activeLODIndex;
        private int debugNumUpdates;
        internal GameObject lod0;
        internal GameObject lod1;
        internal GameObject lod2;
        private Mesh meshCollision;
        private Mesh[] meshLoDs;

        private GameObject ocean;
        private Mesh previewMesh;
        private bool shaderSettingsUpdated;
        private bool shapeSettingsUpdated;
        private Material terrainMatInstance;

        private ComputeBuffer vertexBuffer;

        private LODGroup lodGroup { get; set; }
        public Vector3[] spawnObjectVertices { get; private set; }


        private bool InEditMode => !Application.isPlaying;

        public float bodyScale =>
            // Body radius is determined by the celestial body class,
            // which sets the local scale of the generator object (this object)
            transform.localScale.x;

        // Start is called before the first frame update
        private void Start()
        {
            if (!ocean) ocean = transform.Find("Ocean").gameObject;
            if (!terrainMesh) terrainMesh = transform.Find("Terrain Mesh").gameObject;
            if (!lod0) lod0 = terrainMesh.transform.Find("Terrain Mesh_LOD0").gameObject;
            if (!lod1) lod1 = terrainMesh.transform.Find("Terrain Mesh_LOD1").gameObject;
            if (!lod2) lod2 = terrainMesh.transform.Find("Terrain Mesh_LOD2").gameObject;
            if (!lodGroup) lodGroup = terrainMesh.GetComponent<LODGroup>();

            // HandleGameModeGeneration();
        }

        // Update is called once per frame
        private void Update()
        {
            bool requestPlanets = DevTools.drawPlanets;
            if (InEditMode)
            {
                if (requestPlanets)
                    HandleEditModeGeneration();
                else
                    DeletePlanets();


                if (!ocean) ocean = transform.Find("Ocean").gameObject;
                if (!terrainMesh) terrainMesh = transform.Find("Terrain Mesh").gameObject;
                if (!lod0) lod0 = terrainMesh.transform.Find("Terrain Mesh_LOD0").gameObject;
                if (!lod1) lod1 = terrainMesh.transform.Find("Terrain Mesh_LOD1").gameObject;
                if (!lod2) lod2 = terrainMesh.transform.Find("Terrain Mesh_LOD2").gameObject;
                if (!lodGroup) lodGroup = terrainMesh.GetComponent<LODGroup>();
            }

            float oceanScale = scale * 2;
            ocean.transform.localScale = new Vector3(oceanScale, oceanScale, oceanScale);
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

            resolutionSettings?.ClampResolutions();

            OnShapeSettingChanged();
        }


        private void GetObjects()
        {
            if (!ocean) ocean = transform.Find("Ocean").gameObject;
            if (!terrainMesh) terrainMesh = transform.Find("Terrain Mesh").gameObject;
            if (!lod0) lod0 = terrainMesh.transform.Find("Terrain Mesh_LOD0").gameObject;
            if (!lod1) lod1 = terrainMesh.transform.Find("Terrain Mesh_LOD1").gameObject;
            if (!lod2) lod2 = terrainMesh.transform.Find("Terrain Mesh_LOD2").gameObject;
            if (!lodGroup) lodGroup = terrainMesh.GetComponent<LODGroup>();
        }

        private void DeletePlanets()
        {
            if (terrainMesh)
            {
                DestroyImmediate(terrainMesh);
                shaderSettingsUpdated = true;
                shapeSettingsUpdated = true;
            }
        }


        public void SetLOD(int lodIndex)
        {
            if (lodIndex != activeLODIndex) activeLODIndex = lodIndex;
        }


        public void HandleEditModeGeneration()
        {
            GetObjects();
            if (InEditMode)
            {
                ComputeHelper.shouldReleaseEditModeBuffers -= ReleaseAllBuffers;
                ComputeHelper.shouldReleaseEditModeBuffers += ReleaseAllBuffers;
                lodGroup.enabled = false;
                if (activeLODIndex != 0) lod0.GetComponent<MeshRenderer>().enabled = false;
                if (activeLODIndex != 1) lod1.GetComponent<MeshRenderer>().enabled = false;
                if (activeLODIndex != 2) lod2.GetComponent<MeshRenderer>().enabled = false;
            }

            if (CanGenerateMesh())
            {
                // Update shape settings and shading noise
                if (shapeSettingsUpdated)
                {
                    /*shapeSettingsUpdated = false;
                    shaderSettingsUpdated = false;
                    Dummy();

                    var terrainMeshTimer = Stopwatch.StartNew();
                    heightMinMax = GenerateTerrainMesh(ref previewMesh, PickTerrainRes());


                    LogTimer(terrainMeshTimer, "Generate terrain mesh");

                    terrainMesh.transform.localScale = new Vector3(scale, scale, scale);


                    lod0.GetComponent<MeshRenderer>().sharedMaterial = shader.terrainMaterial;
                    lod1.GetComponent<MeshRenderer>().sharedMaterial = shader.terrainMaterial;
                    lod2.GetComponent<MeshRenderer>().sharedMaterial = shader.terrainMaterial;

                    if (activeLODIndex == 0)
                    {
                        lod0.GetComponent<MeshFilter>().sharedMesh = previewMesh;
                        lod0.GetComponent<MeshRenderer>().enabled = true;
                    }
                    else if (activeLODIndex == 1)
                    {
                        lod1.GetComponent<MeshFilter>().sharedMesh = previewMesh;
                        lod1.GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        lod2.GetComponent<MeshFilter>().sharedMesh = previewMesh;
                        lod0.GetComponent<MeshRenderer>().enabled = true;
                    }*/
                }
                // If only shading noise has changed, update it separately from shape to save time
                else if (shaderSettingsUpdated)
                {
                    shaderSettingsUpdated = false;
                    shader.Initialize(shape);

                    // Sometimes when changing a colour property, invalid data is returned from compute shader
                    // Running the shading a second time fixes it.
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

        private void DrawMesh()
        {
            GetObjects();
            // var _ = GetOrCreateMeshObject("Terrain Mesh", previewMesh, shader.terrainMaterial);

            lodGroup.fadeMode = LODFadeMode.CrossFade;
            lodGroup.animateCrossFading = true;

            var lods = new LOD[3];
            var renderers = new Renderer[1];
            renderers[0] = lod0.gameObject.GetComponent<Renderer>();
            lods[0] = new LOD(.6f, renderers);

            renderers = new Renderer[1];
            renderers[0] = lod1.gameObject.GetComponent<Renderer>();
            lods[1] = new LOD(.3f, renderers);

            renderers = new Renderer[1];
            renderers[0] = lod2.gameObject.GetComponent<Renderer>();
            lods[2] = new LOD(.1f, renderers);

            lodGroup.SetLODs(lods);

            terrainMesh.transform.localScale = new Vector3(scale, scale, scale);
        }


        private void LogTimer(Stopwatch sw, string text)
        {
            if (DevTools.logPlanetInfo) print(text + " " + sw.ElapsedMilliseconds + " ms.");
        }


        private (Vector3[] vertices, int[] triangles) GetOrCreateSphere(int resolution)
        {
            // if (spheres.ContainsKey(resolution))
            // {
            //     (Vector3[] vertices, int[] triangles) b = spheres[resolution];
            //     Vector3[] c = Array.ConvertAll(b.vertices, a => new Vector3(a.x, a.y, a.z));
            //     (Vector3[] c, int[] triangles) a = (c, b.triangles);
            //     return a;
            // }

            spheres[resolution] = CreateSphereVertsAndTris(resolution);
            return spheres[resolution];
        }

        // Generates terrain mesh based on heights generated by the Shape object
        // Shading data from the Shading object is stored in the mesh uvs
        // Returns the min/max height of the terrain
        private Vector2 GenerateTerrainMesh(ref Mesh mesh, int resolution)
        {
            (Vector3[] vertices, int[] triangles) = GetOrCreateSphere(resolution);
            ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, vertices);

            float edgeLength = (vertices[triangles[0]] - vertices[triangles[1]]).magnitude;

            // Set heights
            float[] heights = shape.CalculateHeights(vertexBuffer);

            // Perturb vertices to give terrain a less perfectly smooth appearance
            if (shape.perturbVertices && shape.perturbCompute)
            {
                ComputeShader perturbShader = shape.perturbCompute;
                float maxperturbStrength = shape.perturbStrength * edgeLength / 2;

                perturbShader.SetBuffer(0, "points", vertexBuffer);
                perturbShader.SetInt("numPoints", vertices.Length);
                perturbShader.SetFloat("maxStrength", maxperturbStrength);

                ComputeHelper.Run(perturbShader, vertices.Length);
                var pertData = new Vector3[vertices.Length];
                vertexBuffer.GetData(vertices);
            }


            // Calculate terrain min/max height and set heights of vertices
            float minHeight = float.PositiveInfinity;
            float maxHeight = float.NegativeInfinity;
            for (var i = 0; i < heights.Length; i++)
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


            // Create crude tangents (vectors perpendicular to surface normal)
            // This is needed (even though normal mapping is being done with triplanar)
            // because surface shader wants normals in tangent space
            Vector3[] normals = mesh.normals;
            var crudeTangents = new Vector4[mesh.vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                Vector3 normal = normals[i];
                crudeTangents[i] = new Vector4(-normal.z, 0, normal.x, 1);
            }

            mesh.SetTangents(crudeTangents);


            return new Vector2(minHeight, maxHeight);
        }

        // Generate sphere (or reuse if already generated) and return a copy of the vertices and triangles
        private (Vector3[] vertices, int[] triangles) CreateSphereVertsAndTris(int resolution)
        {
            sphereGenerators ??= new Dictionary<int, SphereMesh>();

            if (!sphereGenerators.ContainsKey(resolution)) sphereGenerators.Add(resolution, new SphereMesh(resolution));

            SphereMesh generator = sphereGenerators[resolution];

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


        // Handles creation of celestial body when entering game mode
        // This differs from the edit-mode version in the following ways:
        // • creates all LOD meshes and stores them in mesh array (to be picked based on player position)
        // • creates its own instances of materials so multiple bodies can exist with their own shading
        // • doesn't support updating of shape/shading values once generated
        public void HandleGameModeGeneration(int index = 0)
        {
            key = index;
            GetObjects();
            lodGroup.enabled = true;

            var lodTimer = Stopwatch.StartNew();

            Dummy();

            // Generate LOD meshes
            meshLoDs = new Mesh[ResolutionSettings.NumLODLevels];
            for (var i = 0; i < meshLoDs.Length; i++)
            {
                meshLoDs[i] = new Mesh();
                Vector2 lodTerrainHeightMinMax = GenerateTerrainMesh(
                    ref meshLoDs[i],
                    resolutionSettings.GetLODResolution(i)
                );
                // Use min/max height of first (most detailed) LOD
                if (i == 0) heightMinMax = lodTerrainHeightMinMax;
            }


            // Generate collision mesh
            GenerateCollisionMesh(resolutionSettings.collider);

            // Create terrain renderer and set shading properties on the instanced material
            // terrainMatInstance = new Material(shader.terrainMaterial);
            shader.Initialize(shape);
            shader.SetTerrainProperties(shader.terrainMaterial, heightMinMax, bodyScale);

            // LOD stuff
            lodGroup.fadeMode = LODFadeMode.CrossFade;
            lodGroup.animateCrossFading = true;

            var lods = new LOD[3];
            var renderers = new Renderer[1];
            renderers[0] = lod0.gameObject.GetComponent<Renderer>();
            lods[0] = new LOD(.6f, renderers);

            renderers = new Renderer[1];
            renderers[0] = lod1.gameObject.GetComponent<Renderer>();
            lods[1] = new LOD(.3f, renderers);

            renderers = new Renderer[1];
            renderers[0] = lod2.gameObject.GetComponent<Renderer>();
            lods[2] = new LOD(.1f, renderers);

            lodGroup.SetLODs(lods);

            terrainMesh.transform.localScale = new Vector3(scale, scale, scale);


            lod0.GetComponent<MeshRenderer>().enabled = true;
            lod1.GetComponent<MeshRenderer>().enabled = true;
            lod2.GetComponent<MeshRenderer>().enabled = true;

            lod0.GetComponent<MeshFilter>().sharedMesh = meshLoDs[0];
            lod1.GetComponent<MeshFilter>().sharedMesh = meshLoDs[1];
            lod2.GetComponent<MeshFilter>().sharedMesh = meshLoDs[2];


            // Medium LOD used for spawning objects
            spawnObjectVertices = meshLoDs[1].vertices;


            LogTimer(lodTimer, "Generate all LODs");


            var collisionBakeTimer = Stopwatch.StartNew();
            meshesToBake[index] = meshCollision;
            // MeshBaker.BakeMeshImmediate(meshCollision);
            // MeshBaker.StartBakingMesh(meshCollision);
            // terrainMesh.GetComponent<MeshCollider>().sharedMesh = meshCollision;
            // LogTimer(collisionBakeTimer, "Bake Mesh collider");


            ReleaseAllBuffers();
        }

        private void GenerateCollisionMesh(int resolution)
        {
            (Vector3[] vertices, int[] triangles) = CreateSphereVertsAndTris(resolution);
            ComputeHelper.CreateStructuredBuffer(ref vertexBuffer, vertices);

            // Set heights
            float[] heights = shape.CalculateHeights(vertexBuffer);
            for (var i = 0; i < vertices.Length; i++)
            {
                float height = heights[i];
                vertices[i] *= height;
            }

            // Create mesh
            CreateMesh(ref meshCollision, vertices.Length);
            meshCollision.vertices = vertices;
            meshCollision.triangles = triangles;
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
            shape?.ReleaseBuffers();
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