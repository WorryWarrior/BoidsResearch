using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Jobs
{
    [BurstCompile]
    public struct CalculateAlignmentJob : IJobParallelFor
    {
        [ReadOnly] public float AlignmentWeight;
        [ReadOnly] public float MaxSpeed;
        [ReadOnly] public float MaxSteerForce;
        [ReadOnly] public NativeArray<float3> BoidVelocities;
        [ReadOnly] public NativeArray<float3> BoidFlockDirections;

        [WriteOnly] public NativeArray<float3> AlignmentValues;

        public void Execute(int index)
        {
            float3 alignmentValue = BoidsMathUtility.GetClampedDirection(BoidFlockDirections[index],
                BoidVelocities[index], MaxSpeed, MaxSteerForce) * AlignmentWeight;

            AlignmentValues[index] = alignmentValue;
        }
    }
}