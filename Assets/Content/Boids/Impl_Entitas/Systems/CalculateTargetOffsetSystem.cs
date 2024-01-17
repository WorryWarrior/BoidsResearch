using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateTargetOffsetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Velocity,
            GameMatcher.FollowingTarget,
            GameMatcher.TargetOffset));
        private readonly IPersistentDataService _persistentDataService;

        public CalculateTargetOffsetSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            NativeArray<float3> _targetOffsets = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidTargetPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidTargetPositions[entityIndex] = e.followingTarget.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateTargetOffsetJob targetOffsetJob = new()
            {
                targetWeight = _persistentDataService.BoidSettings.TargetWeight,
                maxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                maxSteerForce = _persistentDataService.BoidSettings.MaxSteerForce,
                boidPositions = _boidPositions,
                boidTargetPositions = _boidTargetPositions,
                boidVelocities = _boidVelocities,
                targetOffsetValues = _targetOffsets
            };

            JobHandle jobHandle = targetOffsetJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceTargetOffset(_targetOffsets[entityIndex]);

                entityIndex++;
            }

            _targetOffsets.Dispose();
            _boidPositions.Dispose();
            _boidTargetPositions.Dispose();
            _boidVelocities.Dispose();
        }
    }
}