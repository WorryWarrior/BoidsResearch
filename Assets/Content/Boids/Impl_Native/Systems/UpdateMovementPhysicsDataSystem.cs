using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateMovementDataSystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateMovementPhysicsDataSystem : ISystem
    {
        private NativeArray<float3> Directions;

        public void OnCreate(ref SystemState state)
        {
            const int directionCount = 100;
            Directions = new NativeArray<float3>(BoidsMathUtility.GetAvoidanceRayDirections(directionCount), Allocator.Persistent);

            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            CalculateMovementPhysicsDataJob calculateMovementPhysicsDataJob = new CalculateMovementPhysicsDataJob
            {
                World = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld,
                Directions = Directions,
            };
            calculateMovementPhysicsDataJob.ScheduleParallel();
        }

        public void OnDestroy(ref SystemState state)
        {
            Directions.Dispose();
        }

        [BurstCompile]
        [StructLayout(LayoutKind.Auto)]
        [RequireMatchingQueriesForUpdate]
        private partial struct CalculateMovementPhysicsDataJob : IJobEntity
        {
            [ReadOnly] public PhysicsWorld World;
            [ReadOnly] public NativeArray<float3> Directions;

            private void Execute(ref LocalToWorld transform,
                ref VelocityComponent velocity,
                ref BoidParameterComponent parameters,
                ref BoidMovementDataComponent movementData
                //,ref DebugPhysicsComponent physData
            )
            {
                bool isColliding = World.SphereCast(transform.Position, parameters.BoundsRadius,
                    math.forward(transform.Rotation), parameters.CollisionAvoidanceDistance, CollisionFilter.Default);
                //physData.IsColliding = isColliding;

                if (!isColliding)
                {
                    movementData.AvoidanceContribution = float3.zero;
                    return;
                }

                for (int i = 0; i < Directions.Length; i++)
                {
                    float3 worldDirection = transform.Value.TransformDirection(Directions[i]);

                    if (World.SphereCast(transform.Position, parameters.BoundsRadius, worldDirection,
                            parameters.CollisionAvoidanceDistance, CollisionFilter.Default))
                    {
                        movementData.AvoidanceContribution = GetClampedDirection(
                            worldDirection, velocity.Value,
                            parameters.MaxSpeedValue, parameters.MaxSteerForce) * parameters.CollisionAvoidanceWeight;

                        /*physData.DirIndex = i;
                        physData.DirValue = worldDirection;*/

                        return;
                    }
                }
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

    public struct DebugPhysicsComponent : IComponentData
    {
        public bool IsColliding;
        public int DirIndex;
        public float3 DirValue;
    }
}