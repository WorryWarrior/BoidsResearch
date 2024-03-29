using Content.Boids.Impl_Entitas.Jobs;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateCohesionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Velocity,
            GameMatcher.FlockmateNumber,
            GameMatcher.FlockCenter,
            GameMatcher.Cohesion));
        private readonly IPersistentDataService _persistentDataService;

        public CalculateCohesionSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            NativeArray<float3> _cohesionValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidFlockCenters = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidFlockCenters[entityIndex] = e.flockCenter.value / e.flockmateNumber.value;
                _boidPositions[entityIndex] = e.position.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateCohesionJob calculateCohesionJob = new()
            {
                CohesionWeight = _persistentDataService.BoidSettings.CohesionWeight,
                MaxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                MaxSteerForce = _persistentDataService.BoidSettings.MaxSteerForce,
                BoidPositions = _boidPositions,
                BoidVelocities = _boidVelocities,
                BoidFlockCenters = _boidFlockCenters,
                CohesionValues = _cohesionValues
            };

            JobHandle jobHandle = calculateCohesionJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _cohesionValues[entityIndex] = float3.zero;
                }

                e.ReplaceCohesion(_cohesionValues[entityIndex]);

                entityIndex++;
            }

            _cohesionValues.Dispose();
            _boidFlockCenters.Dispose();
            _boidPositions.Dispose();
            _boidVelocities.Dispose();
        }
    }
}