using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Jobs
{
    [BurstCompile]
    public struct CalculateTargetOffsetJob : IJobParallelFor
    {
        [ReadOnly] public float TargetWeight;
        [ReadOnly] public float MaxSpeed;
        [ReadOnly] public float MaxSteerForce;
        [ReadOnly] public NativeArray<float3> BoidPositions;
        [ReadOnly] public NativeArray<float3> BoidTargetPositions;
        [ReadOnly] public NativeArray<float3> BoidVelocities;

        [WriteOnly] public NativeArray<float3> TargetOffsetValues;

        public void Execute(int index)
        {
            float3 targetOffsetValue = BoidsMathUtility.GetClampedDirection(
                BoidTargetPositions[index] - BoidPositions[index], BoidVelocities[index],
                MaxSpeed, MaxSteerForce) * TargetWeight;

            TargetOffsetValues[index] = targetOffsetValue;
        }
    }
}