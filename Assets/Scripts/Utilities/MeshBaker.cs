using System.Collections.Generic;
using Planets;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public static class MeshBaker
{
    private static readonly Dictionary<Mesh, JobHandle> jobsByMesh;

    static MeshBaker()
    {
        jobsByMesh = new Dictionary<Mesh, JobHandle>();
    }

    public static void BakeMeshImmediate(Mesh mesh)
    {
        var job = new BakeJob(mesh.GetInstanceID());
        JobHandle currentJob = job.Schedule();
        currentJob.Complete();
    }

    public static void StartBakingMesh(Mesh mesh)
    {
        var job = new BakeJob(mesh.GetInstanceID());
        JobHandle currentJob = job.Schedule();
        // jobsByMesh.Add(mesh, currentJob);
    }

    public static void EnsureBakingComplete(Mesh mesh)
    {
        if (jobsByMesh.ContainsKey(mesh))
        {
            jobsByMesh[mesh].Complete();
            jobsByMesh.Remove(mesh);
        }
        else
            BakeMeshImmediate(mesh);
    }

    public static void BakeAndSetColliders(PlanetGenerator[] pgs)
    {
        Dictionary<int, Mesh> meshes = PlanetGenerator.meshesToBake;

        // You cannot access GameObjects and Components from other threads directly.
        // As such, you need to create a native array of instance IDs that BakeMesh will accept.
        var meshIds = new NativeArray<int>(meshes.Count, Allocator.TempJob);

        foreach (KeyValuePair<int, Mesh> pair in meshes)
        {
            meshIds[pair.Key] = pair.Value.GetInstanceID();
        }

        // This spreads the expensive operation over all cores.
        var job = new BakeAllMeshes(meshIds);
        job.Schedule(meshIds.Length, 1).Complete();

        meshIds.Dispose();

        // Now instantiate colliders on the main thread.
        foreach (KeyValuePair<int, Mesh> pair in meshes)
        {
            pgs[pair.Key].terrainMesh.GetComponent<MeshCollider>().sharedMesh = pair.Value;
        }
    }
}

public struct BakeJob : IJob
{
    private readonly int meshID;

    public BakeJob(int meshID)
    {
        this.meshID = meshID;
    }

    public void Execute()
    {
        Physics.BakeMesh(meshID, false);
    }
}

public struct BakeAllMeshes : IJobParallelFor
{
    private NativeArray<int> meshIds;

    public BakeAllMeshes(NativeArray<int> meshIds)
    {
        this.meshIds = meshIds;
    }

    public void Execute(int index)
    {
        Physics.BakeMesh(meshIds[index], false);
    }
}