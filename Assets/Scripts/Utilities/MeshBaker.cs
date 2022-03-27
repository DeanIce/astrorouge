using System.Collections.Generic;
using Planets;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Utilities
{
    public static class MeshBaker
    {
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

            // Now instantiate colliders on the main thread.
            foreach (KeyValuePair<int, Mesh> pair in meshes)
            {
                pgs[pair.Key].terrainMesh.GetComponent<MeshCollider>().sharedMesh = pair.Value;
            }

            meshIds.Dispose();
            PlanetGenerator.meshesToBake.Clear();
        }
    }


    [BurstCompile]
    public struct BakeAllMeshes : IJobParallelFor, JobHelper.IJobDisposable
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

        public void OnDispose()
        {
            // meshIds.Dispose();
        }
    }
}