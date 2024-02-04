using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Components;
using Content.Boids.Impl_Native.Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateAccelerationSystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateVelocitySystem : ISystem
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
            UpdateVelocityJob updateVelocityJob = new UpdateVelocityJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            updateVelocityJob.ScheduleParallel(_boidQuery);
        }

        [RequireMatchingQueriesForUpdate]
        [StructLayout(LayoutKind.Auto)]
        private partial struct UpdateVelocityJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(ref VelocityComponent velocity,
                ref AccelerationComponent acceleration,
                ref BoidParameterComponent parameters)
            {
                velocity.Value += acceleration.Value * DeltaTime;
                float3 velocityValueCopy = velocity.Value;
                float speed = math.length(velocityValueCopy);
                float3 dir = velocityValueCopy / speed;
                speed = math.clamp(speed, parameters.MinSpeedValue, parameters.MaxSpeedValue);

                velocity.Value = dir * speed;
            }
        }
    }
}