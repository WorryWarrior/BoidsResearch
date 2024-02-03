using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Components;
using Content.Boids.Impl_Native.Aspects;
using Content.Infrastructure.Services.PersistentData;
using Unity.Burst;
using Unity.Entities;
using Zenject;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UpdateBoidsParametersSystem : SystemBase
    {
        private IPersistentDataService _persistentDataService;
        private EntityQuery _boidQuery;

        [Inject]
        private void Construct(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
            _boidQuery = SystemAPI.QueryBuilder().WithAspect<BoidAspect>().Build();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            if (_persistentDataService == null)
                return;

            UpdateBoidParametersJob updateBoidParametersJob = new UpdateBoidParametersJob
            {
                MinSpeed = _persistentDataService.BoidSettings.MinSpeed,
                MaxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                PerceptionRadius = _persistentDataService.BoidSettings.PerceptionRadius,
                AvoidanceRadius = _persistentDataService.BoidSettings.AvoidanceRadius,
                MaxSteerForce = _persistentDataService.BoidSettings.MaxSteerForce,
                AlignmentWeight = _persistentDataService.BoidSettings.AlignmentWeight,
                CohesionWeight = _persistentDataService.BoidSettings.CohesionWeight,
                SeparationWeight = _persistentDataService.BoidSettings.SeparationWeight,
                TargetWeight = _persistentDataService.BoidSettings.TargetWeight,
                BoundsRadius = _persistentDataService.BoidSettings.BoundsRadius,
                CollisionAvoidanceWeight = _persistentDataService.BoidSettings.CollisionAvoidanceWeight,
                CollisionAvoidanceDistance = _persistentDataService.BoidSettings.CollisionAvoidanceDistance,
            };

            updateBoidParametersJob.ScheduleParallel(_boidQuery);
        }

        [StructLayout(LayoutKind.Auto)]
        [RequireMatchingQueriesForUpdate]
        private partial struct UpdateBoidParametersJob : IJobEntity
        {
            public float MinSpeed;
            public float MaxSpeed;
            public float PerceptionRadius;
            public float AvoidanceRadius;
            public float MaxSteerForce;
            public float AlignmentWeight;
            public float CohesionWeight;
            public float SeparationWeight;
            public float TargetWeight;
            public float BoundsRadius;
            public float CollisionAvoidanceWeight;
            public float CollisionAvoidanceDistance;

            private void Execute(ref BoidParameterComponent parameters)
            {
                parameters.MinSpeedValue = MinSpeed;
                parameters.MaxSpeedValue = MaxSpeed;
                parameters.PerceptionRadius = PerceptionRadius;
                parameters.AvoidanceRadius = AvoidanceRadius;
                parameters.MaxSteerForce = MaxSteerForce;
                parameters.AlignmentWeight = AlignmentWeight;
                parameters.CohesionWeight = CohesionWeight;
                parameters.SeparationWeight = SeparationWeight;
                parameters.TargetWeight = TargetWeight;
                parameters.BoundsRadius = BoundsRadius;
                parameters.CollisionAvoidanceWeight = CollisionAvoidanceWeight;
                parameters.CollisionAvoidanceDistance = CollisionAvoidanceDistance;
            }
        }
    }
}