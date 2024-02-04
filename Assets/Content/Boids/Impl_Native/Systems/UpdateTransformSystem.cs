using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Components;
using Content.Boids.Impl_Native.Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateVelocitySystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateTransformSystem : ISystem
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
            UpdateTransformJob updateTransformJob = new UpdateTransformJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            updateTransformJob.ScheduleParallel(_boidQuery);
        }

        [RequireMatchingQueriesForUpdate]
        [StructLayout(LayoutKind.Auto)]
        private partial struct UpdateTransformJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(ref LocalTransform transform, ref VelocityComponent velocity)
            {
                transform.Position += velocity.Value * DeltaTime;
                float3 dir = velocity.Value / math.length(velocity.Value);
                transform.Rotation = quaternion.LookRotationSafe(dir, math.up());
            }
        }
    }
}