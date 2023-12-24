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
                boundsRadius = _persistentDataService.BoidsSettings.BoundsRadius,
                collisionAvoidanceDistance = _persistentDataService.BoidsSettings.CollisionAvoidanceDistance,
                boidPositions = _boidPositions,
                boidRotations = _boidRotations,
                collisionTrajectoryStatuses = _boidCollisionTrajectoryStatuses
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