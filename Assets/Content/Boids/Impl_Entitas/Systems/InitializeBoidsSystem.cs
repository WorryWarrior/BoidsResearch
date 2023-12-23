using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class InitializeBoidsSystem : IInitializeSystem
    {
        private readonly GameContext _context;
        private readonly IPersistentDataService _persistentDataService;
        
        /*private readonly int _boidCount;
        private readonly float _spawnRadius;
        private readonly float _minSpeed;
        private readonly float _maxSpeed;*/

        public InitializeBoidsSystem(
            /*GameContext context, 
            BoidSettings settings */
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
            //_context = context;

            /*_boidCount = settings.boidCount;
            _spawnRadius = settings.spawnRadius;
            _minSpeed = settings.minSpeed;
            _maxSpeed = settings.maxSpeed;*/

            /*_boidCount = persistentDataService.BoidsSettings.BoidCount;
            _spawnRadius = persistentDataService.BoidsSettings.SpawnRadius;
            _minSpeed = persistentDataService.BoidsSettings.MinSpeed;
            _maxSpeed = persistentDataService.BoidsSettings.MaxSpeed;*/
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