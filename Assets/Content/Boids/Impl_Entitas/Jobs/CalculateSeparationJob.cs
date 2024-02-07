using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Jobs
{
    [BurstCompile]
    public struct CalculateSeparationJob : IJobParallelFor
    {
        [ReadOnly] public float SeparationWeight;
        [ReadOnly] public float MaxSpeed;
        [ReadOnly] public float MaxSteerForce;
        [ReadOnly] public NativeArray<float3> BoidVelocities;
        [ReadOnly] public NativeArray<float3> BoidAverageAvoidances;
        [WriteOnly] public NativeArray<float3> SeparationValues;

        public void Execute(int index)
        {
            float3 separationValue = BoidsMathUtility.GetClampedDirection(BoidAverageAvoidances[index],
                BoidVelocities[index], MaxSpeed, MaxSteerForce) * SeparationWeight;

            SeparationValues[index] = separationValue;
        }
    }
}