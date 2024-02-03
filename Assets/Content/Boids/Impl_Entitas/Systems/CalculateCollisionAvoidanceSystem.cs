using Content.Boids.Jobs;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateCollisionAvoidanceSystem : IExecuteSystem
    {
        private const int AVOIDANCE_RAYCAST_DIRECTION_COUNT = 300;

        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Rotation,
            GameMatcher.Velocity,
            GameMatcher.IsOnCollisionTrajectory,
            GameMatcher.Avoidance));
        private readonly IPersistentDataService _persistentDataService;

        private float3[] _avoidanceRaycastDirectionsArray;

        private float3[] _AvoidanceRaycastDirectionsArray =>
            _avoidanceRaycastDirectionsArray ??=
                BoidsMathUtility.GetAvoidanceRayDirections(AVOIDANCE_RAYCAST_DIRECTION_COUNT);

        public CalculateCollisionAvoidanceSystem(
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
                    e.ReplaceAvoidance(float3.zero);
                }

                return;
            }

            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidRotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<bool> _boidIsOnCollisionTrajectoryStatuses =
                new NativeArray<bool>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _collisionAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _avoidanceRaycastDirections = new NativeArray<float3>(_AvoidanceRaycastDirectionsArray,
                Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidRotations[entityIndex] = e.rotation.value;
                _boidVelocities[entityIndex] = e.velocity.value;
                _boidIsOnCollisionTrajectoryStatuses[entityIndex] = e.isOnCollisionTrajectory.value;

                entityIndex++;
            }

            CalculateCollisionAvoidanceJob calculateCollisionAvoidanceJob = new()
            {
                CollisionAvoidanceWeight = _persistentDataService.BoidSettings.CollisionAvoidanceWeight,
                MaxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                MaxSteerForce = _persistentDataService.BoidSettings.MaxSteerForce,
                BoundsRadius = _persistentDataService.BoidSettings.BoundsRadius,
                CollisionAvoidanceDistance = _persistentDataService.BoidSettings.CollisionAvoidanceDistance,
                BoidPositions = _boidPositions,
                BoidRotations = _boidRotations,
                BoidVelocities = _boidVelocities,
                BoidIsOnCollisionTrajectoryStatuses = _boidIsOnCollisionTrajectoryStatuses,
                AvoidanceRaycastDirections = _avoidanceRaycastDirections,
                CollisionAvoidances = _collisionAvoidances
            };

            JobHandle jobHandle = calculateCollisionAvoidanceJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (!e.isOnCollisionTrajectory.value)
                {
                    _collisionAvoidances[entityIndex] = float3.zero;
                }

                e.ReplaceAvoidance(_collisionAvoidances[entityIndex]);

                entityIndex++;
            }

            _boidPositions.Dispose();
            _boidRotations.Dispose();
            _boidVelocities.Dispose();
            _boidIsOnCollisionTrajectoryStatuses.Dispose();
            _collisionAvoidances.Dispose();
            _avoidanceRaycastDirections.Dispose();
        }
    }
}