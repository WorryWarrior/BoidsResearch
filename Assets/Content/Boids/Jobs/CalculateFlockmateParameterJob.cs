using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Jobs
{
    [BurstCompile]
    public struct CalculateFlockmateParameterJob : IJobParallelFor
    {
        [ReadOnly] public float PerceptionRadius;
        [ReadOnly] public float AvoidanceRadius;
        [ReadOnly] public NativeArray<float3> BoidPositions;
        [ReadOnly] public NativeArray<float3> BoidRotations;

        [WriteOnly] public NativeArray<float3> FlockHeadings;
        [WriteOnly] public NativeArray<int> FlockmateNumbers;
        [WriteOnly] public NativeArray<float3> FlockCenters;
        [WriteOnly] public NativeArray<float3> AverageAvoidances;

        public void Execute(int index)
        {
            float3 currentPosition = BoidPositions[index];

            int flockmates = 0;
            float3 flockHeading = float3.zero;
            float3 flockmatesCenter = float3.zero;
            float3 avoidanceHeading = float3.zero;

            for (int i = 0; i < BoidPositions.Length; i++)
            {
                if (i != index)
                {
                    float3 otherPosition = BoidPositions[i];
                    float sqrDst = math.distancesq(currentPosition, otherPosition);

                    if (sqrDst < PerceptionRadius * PerceptionRadius)
                    {
                        flockmates++;
                        flockHeading += BoidRotations[i];
                        flockmatesCenter += otherPosition;

                        if (sqrDst < AvoidanceRadius * AvoidanceRadius)
                        {
                            avoidanceHeading -= (otherPosition - currentPosition) / sqrDst;
                        }
                    }
                }
            }

            FlockmateNumbers[index] = flockmates;
            FlockHeadings[index] = flockHeading;
            FlockCenters[index] = flockmatesCenter;
            AverageAvoidances[index] = avoidanceHeading;
        }
    }
}