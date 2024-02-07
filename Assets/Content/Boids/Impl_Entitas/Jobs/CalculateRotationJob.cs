using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Jobs
{
    [BurstCompile]
    public struct CalculateRotationJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> BoidVelocities;
        [WriteOnly] public NativeArray<float3> BoidRotations;

        public void Execute(int index)
        {
            float3 dir = BoidVelocities[index] / math.length(BoidVelocities[index]);
            BoidRotations[index] = math.forward(quaternion.LookRotationSafe(dir, math.up()));
        }
    }
}