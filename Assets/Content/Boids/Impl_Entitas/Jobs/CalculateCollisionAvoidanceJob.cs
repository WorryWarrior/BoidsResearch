using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Jobs
{
    [BurstCompile]
    public struct CalculateCollisionAvoidanceJob : IJobParallelFor
    {
        [ReadOnly] public float CollisionAvoidanceWeight;
        [ReadOnly] public float MaxSpeed;
        [ReadOnly] public float MaxSteerForce;
        [ReadOnly] public float BoundsRadius;
        [ReadOnly] public float CollisionAvoidanceDistance;
        [ReadOnly] public NativeArray<float3> BoidPositions;
        [ReadOnly] public NativeArray<float3> BoidRotations;
        [ReadOnly] public NativeArray<float3> BoidVelocities;
        [ReadOnly] public NativeArray<bool> BoidIsOnCollisionTrajectoryStatuses;
        [ReadOnly] public NativeArray<float3> AvoidanceRaycastDirections;

        [WriteOnly] public NativeArray<float3> CollisionAvoidances;

        public void Execute(int index)
        {
            if (!BoidIsOnCollisionTrajectoryStatuses[index])
            {
                CollisionAvoidances[index] = float3.zero;
                return;
            }

            float3 avoidanceTrajectory = BoidRotations[index];

            for (int i = 0; i < BoidPositions.Length; i++)
            {
                if (i != index)
                {
                    for (int j = 0; j < AvoidanceRaycastDirections.Length; j++)
                    {
                        float3 rayDirection = math.mul(quaternion.LookRotationSafe(BoidRotations[index], math.up()),
                            AvoidanceRaycastDirections[j]);
                        float3 rayProjection = BoidsMathUtility.GetClosestPointOnRay(BoidPositions[index],
                            rayDirection, BoidPositions[i]);

                        if (math.distance(BoidPositions[index], rayProjection) >= CollisionAvoidanceDistance &&
                            math.distance(BoidPositions[i], rayProjection) < BoundsRadius)
                        {
                            CollisionAvoidances[index] = BoidsMathUtility.GetClampedDirection(
                                avoidanceTrajectory, BoidVelocities[index],
                                MaxSpeed, MaxSteerForce) * CollisionAvoidanceWeight;
                            return;
                        }
                    }
                }
            }

            CollisionAvoidances[index] = BoidsMathUtility.GetClampedDirection(
                avoidanceTrajectory, BoidVelocities[index], MaxSpeed, MaxSteerForce) * CollisionAvoidanceWeight;
        }
    }
}