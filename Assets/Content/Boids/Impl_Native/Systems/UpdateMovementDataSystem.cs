using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Aspects;
using Content.Boids.Impl_Native.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateFlockDataSystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateMovementDataSystem : ISystem
    {
        private EntityQuery _boidQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _boidQuery = SystemAPI.QueryBuilder().WithAspect<BoidAspect>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new UpdateMovementDataJob().ScheduleParallel(_boidQuery);
        }

        [RequireMatchingQueriesForUpdate]
        private partial struct UpdateMovementDataJob : IJobEntity
        {
            private void Execute(ref BoidMovementDataComponent movementData,
                ref LocalTransform transform,
                ref VelocityComponent velocity,
                ref FlockDataComponent flockData,
                ref BoidParameterComponent parameters)
            {
                movementData.CohesionContribution = flockData.FlockmateCount == 0 ? float3.zero : GetClampedDirection(
                    (flockData.FlockCenter / flockData.FlockmateCount)- transform.Position, velocity.Value,
                    parameters.MaxSpeedValue, parameters.MaxSteerForce) * parameters.CohesionWeight;

                movementData.SeparationContribution = flockData.FlockmateCount == 0 ? float3.zero : GetClampedDirection(
                    flockData.AverageFlockAvoidance, velocity.Value,
                    parameters.MaxSpeedValue, parameters.MaxSteerForce) * parameters.SeparationWeight;

                movementData.AlignmentContribution = flockData.FlockmateCount == 0 ? float3.zero : GetClampedDirection(
                    flockData.AverageFlockHeading, velocity.Value,
                    parameters.MaxSpeedValue, parameters.MaxSteerForce) * parameters.AlignmentWeight;

                movementData.FollowTargetContribution = GetClampedDirection(
                    movementData.FollowTargetPosition - transform.Position, velocity.Value,
                    parameters.MaxSpeedValue, parameters.MaxSteerForce) * parameters.TargetWeight;
            }

            private float3 GetClampedDirection(float3 vector, float3 velocity, float maxSpeed, float maxSteerForce)
            {
                float3 v = math.normalizesafe(vector) * maxSpeed - velocity;
                return ClampMagnitude(v, maxSteerForce);
            }

            private float3 ClampMagnitude(float3 vector, float maxLength)
            {
                float sqrMagnitude = math.lengthsq(vector);
                if (sqrMagnitude <= maxLength * maxLength)
                    return vector;

                float magnitude = math.sqrt(sqrMagnitude);
                float3 normalizedVector = vector / magnitude;
                return normalizedVector * maxLength;
            }
        }
    }
}