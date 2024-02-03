using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Mathematics;
using UnityEngine;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateVelocitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Velocity,
            GameMatcher.Acceleration));
        private readonly IPersistentDataService _persistentDataService;

        public CalculateVelocitySystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            foreach (GameEntity e in _boidsGroup)
            {
                e.velocity.value += e.acceleration.value * Time.deltaTime;

                float3 velocity = e.velocity.value;
                float speed = math.length(velocity);
                float3 dir = velocity / speed;
                speed = math.clamp(speed, _persistentDataService.BoidSettings.MinSpeed,
                    _persistentDataService.BoidSettings.MaxSpeed);

                e.velocity.value = dir * speed;
            }
        }
    }
}