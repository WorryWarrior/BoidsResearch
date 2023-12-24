using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class UpdateRotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Rotation,
            GameMatcher.Velocity));

        public void Execute()
        {
            NativeArray<float3> _rotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _velocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _velocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateRotationJob calculateRotationJob = new()
            {
                boidRotations = _rotations,
                boidVelocities = _velocities,
            };

            JobHandle jobHandle = calculateRotationJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceRotation(_rotations[entityIndex]);

                entityIndex++;
            }

            _rotations.Dispose();
            _velocities.Dispose();
        }
    }
}