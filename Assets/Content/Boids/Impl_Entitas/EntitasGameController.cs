using Content.Boids.Impl_Entitas.Features;
using Content.Boids.Impl_Entitas.Systems;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Content.Boids.Impl_Entitas
{
    public class EntitasGameController : MonoBehaviour, IBoidsController
    {
        [SerializeField] private BoidSettings boidSettings = null;
        
        private Feature _systems;
        private EntitasSystemFactory _systemFactory;

        private bool isInitialized = false;
    
        [Inject]
        private void Construct(
            EntitasSystemFactory systemFactory)
        {
            _systemFactory = systemFactory;
        }
        
        private void Update()
        {
            /*if (!isInitialized)
                return;*/
        
            _systems.Execute();
            _systems.Cleanup();
        }

        public void InitializeBoids()
        {
            Contexts contexts = Contexts.sharedInstance;

            //_systems = new BoidSimulationSystems(contexts.game, boidSettings);

            _systems = ConstructEntitasSystems();
            
            _systems.Initialize();
            
            isInitialized = true;
        
            //Debug.Log(_boidsSimulation == null);
        }

        private Feature ConstructEntitasSystems()
        {
            Feature res = new Feature("Entitas Boid Simulation");
            res.Add(_systemFactory.CreateSystem<InitializeBoidsSystem>());
            res.Add(_systemFactory.CreateSystem<InitializeBoidGameObjectsSystem>());
            
            return res;
        }
    }
}