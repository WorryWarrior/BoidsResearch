using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Components;
using Content.Boids.Impl_Native.Aspects;
using Unity.Burst;
using Unity.Entities;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateBoidsParametersSystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateAccelerationSystem : ISystem
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
            UpdateAccelerationJob updateAccelerationJob = new UpdateAccelerationJob();
            updateAccelerationJob.ScheduleParallel(_boidQuery);
        }

        [RequireMatchingQueriesForUpdate]
        private partial struct UpdateAccelerationJob : IJobEntity
        {
            private void Execute(ref AccelerationComponent acceleration,
                ref BoidMovementDataComponent movementData)
            {
                acceleration.Value = movementData.FollowTargetContribution + movementData.AlignmentContribution +
                                     movementData.CohesionContribution + movementData.SeparationContribution +
                                     movementData.AvoidanceContribution;
            }
        }
    }
}