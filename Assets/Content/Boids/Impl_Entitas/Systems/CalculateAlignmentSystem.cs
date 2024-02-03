using Content.Boids.Jobs;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateAlignmentSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Velocity,
            GameMatcher.AverageFlockDirection,
            GameMatcher.Alignment));
        private readonly IPersistentDataService _persistentDataService;

        public CalculateAlignmentSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            NativeArray<float3> _alignmentValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidAverageFlockDirections =
                new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidAverageFlockDirections[entityIndex] = e.averageFlockDirection.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateAlignmentJob calculateAlignmentJob = new()
            {
                AlignmentWeight = _persistentDataService.BoidSettings.AlignmentWeight,
                MaxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                MaxSteerForce = _persistentDataService.BoidSettings.MaxSteerForce,
                BoidVelocities = _boidVelocities,
                BoidFlockDirections = _boidAverageFlockDirections,
                AlignmentValues = _alignmentValues
            };

            JobHandle jobHandle = calculateAlignmentJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _alignmentValues[entityIndex] = float3.zero;
                }

                e.ReplaceAlignment(_alignmentValues[entityIndex]);

                entityIndex++;
            }

            _alignmentValues.Dispose();
            _boidAverageFlockDirections.Dispose();
            _boidVelocities.Dispose();
        }
    }
}