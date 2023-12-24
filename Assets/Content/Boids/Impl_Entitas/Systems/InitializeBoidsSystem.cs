using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class InitializeBoidsSystem : IInitializeSystem
    {
        private readonly GameContext _context;
        private readonly IPersistentDataService _persistentDataService;
        
        public InitializeBoidsSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Initialize()
        {
            for (int i = 0; i < _persistentDataService.BoidsSettings.BoidCount; i++)
            {
                GameEntity e = Contexts.sharedInstance.game.CreateEntity();

                float3 position = BoidsMathUtility.InsideUnitSphere() * _persistentDataService.BoidsSettings.SpawnRadius;
                e.AddPosition(position);

                float3 rotation = math.forward(quaternion.Euler(BoidsMathUtility.InsideUnitSphere()));
                e.AddRotation(rotation);

                float startSpeed = (_persistentDataService.BoidsSettings.MinSpeed + _persistentDataService.BoidsSettings.MaxSpeed) / 2;
                e.AddVelocity(rotation * startSpeed);

                e.AddAcceleration(float3.zero);
                e.AddFollowingTarget(float3.zero);

                e.AddFlockmateNumber(0);
                e.AddFlockCenter(float3.zero);
                e.AddCohesion(float3.zero);

                e.AddAverageAvoidance(float3.zero);
                e.AddSeparation(float3.zero);

                e.AddAverageFlockDirection(float3.zero);
                e.AddAlignment(float3.zero);

                e.AddTargetOffset(float3.zero);

                e.AddIsOnCollisionTrajectory(false);
                e.AddAvoidance(float3.zero);

                e.AddLinkedGO(null);
            }
        }
    }
}