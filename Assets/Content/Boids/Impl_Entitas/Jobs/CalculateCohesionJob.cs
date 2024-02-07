using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Jobs
{
    [BurstCompile]
    public struct CalculateCohesionJob : IJobParallelFor
    {
        [ReadOnly] public float CohesionWeight;
        [ReadOnly] public float MaxSpeed;
        [ReadOnly] public float MaxSteerForce;
        [ReadOnly] public NativeArray<float3> BoidPositions;
        [ReadOnly] public NativeArray<float3> BoidVelocities;
        [ReadOnly] public NativeArray<float3> BoidFlockCenters;
        [WriteOnly] public NativeArray<float3> CohesionValues;

        public void Execute(int index)
        {
            float3 cohesionValue = BoidsMathUtility.GetClampedDirection(
                BoidFlockCenters[index] - BoidPositions[index], BoidVelocities[index],
                MaxSpeed, MaxSteerForce) * CohesionWeight;

            CohesionValues[index] = cohesionValue;
        }
    }
}