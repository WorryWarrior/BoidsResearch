using Content.Boids.Jobs;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CheckCollisionTrajectorySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Rotation));
        private readonly IPersistentDataService _persistentDataService;

        public CheckCollisionTrajectorySystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            if (_persistentDataService.BoidSettings.CollisionAvoidanceWeight == 0f)
            {
                foreach (GameEntity e in _boidsGroup)
                {
                    e.ReplaceIsOnCollisionTrajectory(false);
                }

                return;
            }

            NativeArray<bool> _boidCollisionTrajectoryStatuses =
                new NativeArray<bool>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidRotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidRotations[entityIndex] = e.rotation.value;

                entityIndex++;
            }

            CheckCollisionTrajectoryJob checkCollisionTrajectoryJob = new()
            {
                BoundsRadius = _persistentDataService.BoidSettings.BoundsRadius,
                CollisionAvoidanceDistance = _persistentDataService.BoidSettings.CollisionAvoidanceDistance,
                BoidPositions = _boidPositions,
                BoidRotations = _boidRotations,
                CollisionTrajectoryStatuses = _boidCollisionTrajectoryStatuses
            };

            JobHandle jobHandle = checkCollisionTrajectoryJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceIsOnCollisionTrajectory(_boidCollisionTrajectoryStatuses[entityIndex]);

                entityIndex++;
            }

            _boidCollisionTrajectoryStatuses.Dispose();
            _boidPositions.Dispose();
            _boidRotations.Dispose();
        }
    }
}