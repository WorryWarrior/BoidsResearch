using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Jobs
{
    [BurstCompile]
    public struct CheckCollisionTrajectoryJob : IJobParallelFor
    {
        [ReadOnly] public float BoundsRadius;
        [ReadOnly] public float CollisionAvoidanceDistance;
        [ReadOnly] public NativeArray<float3> BoidPositions;
        [ReadOnly] public NativeArray<float3> BoidRotations;

        [WriteOnly] public NativeArray<bool> CollisionTrajectoryStatuses;

        public void Execute(int index)
        {
            bool foundCollisionTrajectory = false;

            for (int i = 0; i < BoidPositions.Length; i++)
            {
                if (i != index && !foundCollisionTrajectory)
                {
                    float3 rayProjection = BoidsMathUtility.GetClosestPointOnRay(BoidPositions[index],
                        BoidRotations[index], BoidPositions[i]);

                    if (math.distance(BoidPositions[index], rayProjection) < CollisionAvoidanceDistance &&
                        math.distance(BoidPositions[i], rayProjection) < BoundsRadius)
                    {
                        foundCollisionTrajectory = true;
                    }
                }
            }

            CollisionTrajectoryStatuses[index] = foundCollisionTrajectory;
        }
    }
}