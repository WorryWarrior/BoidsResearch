using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateSeparationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Velocity,
            GameMatcher.AverageAvoidance,
            GameMatcher.Separation));
        private readonly IPersistentDataService _persistentDataService;

        public CalculateSeparationSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            NativeArray<float3> _separationValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidAverageAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidAverageAvoidances[entityIndex] = e.averageAvoidance.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateSeparationJob calculateSeparationJob = new()
            {
                separationWeight = _persistentDataService.BoidsSettings.SeparationWeight,
                maxSpeed = _persistentDataService.BoidsSettings.MaxSpeed,
                maxSteerForce = _persistentDataService.BoidsSettings.MaxSteerForce,
                boidVelocities = _boidVelocities,
                boidAverageAvoidances = _boidAverageAvoidances,
                separationValues = _separationValues
            };

            JobHandle jobHandle = calculateSeparationJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _separationValues[entityIndex] = float3.zero;
                }

                e.ReplaceSeparation(_separationValues[entityIndex]);

                entityIndex++;
            }

            _separationValues.Dispose();
            _boidAverageAvoidances.Dispose();
            _boidVelocities.Dispose();
        }
    }
}