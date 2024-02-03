using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Jobs
{
    [BurstCompile]
    public struct CalculateAccelerationJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> BoidTargetOffsets;
        [ReadOnly] public NativeArray<float3> BoidCohesions;
        [ReadOnly] public NativeArray<float3> BoidAlignments;
        [ReadOnly] public NativeArray<float3> BoidSeparations;
        [ReadOnly] public NativeArray<float3> BoidCollisionAvoidances;

        [WriteOnly] public NativeArray<float3> Accelerations;

        public void Execute(int index)
        {
            float3 acceleration = BoidTargetOffsets[index];

            acceleration += BoidCohesions[index];
            acceleration += BoidSeparations[index];
            acceleration += BoidAlignments[index];
            acceleration += BoidCollisionAvoidances[index];

            Accelerations[index] = acceleration;
        }
    }
}