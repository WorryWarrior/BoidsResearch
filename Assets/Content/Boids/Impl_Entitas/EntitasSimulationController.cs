using System;
using Content.Boids.Impl_Entitas.Systems;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Content.Boids.Impl_Entitas
{
    public class EntitasSimulationController : MonoBehaviour, IBoidsSimulationController
    {
        public Action Initialized { get; set; }
        
        private EntitasSystemFactory _systemFactory; 
        private Feature _systems;
        
        [Inject]
        private void Construct(
            EntitasSystemFactory systemFactory)
        {
            _systemFactory = systemFactory;
        }
        
        private void Update()
        {
            _systems.Execute();
            _systems.Cleanup();
        }
        
        public void InitializeBoids()
        {
            _systems = ConstructEntitasSystems();
            _systems.Initialize();
            
            Initialized?.Invoke();
        }

        private Feature ConstructEntitasSystems()
        {
            Feature simulation = new Feature("Entitas Boid Simulation");
            simulation.Add(_systemFactory.CreateSystem<InitializeBoidsSystem>());
            simulation.Add(_systemFactory.CreateSystem<InitializeBoidGameObjectsSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateFlockParametersSystem>());
            simulation.Add(_systemFactory.CreateSystem<CheckCollisionTrajectorySystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateTargetOffsetSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateCohesionSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateSeparationSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateAlignmentSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateCollisionAvoidanceSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateAccelerationSystem>());
            simulation.Add(_systemFactory.CreateSystem<CalculateVelocitySystem>());
            simulation.Add(_systemFactory.CreateSystem<UpdatePositionSystem>());
            simulation.Add(_systemFactory.CreateSystem<UpdateRotationSystem>());
            simulation.Add(_systemFactory.CreateSystem<UpdateLinkedGameObjectPositionSystem>());
            
            return simulation;
        }
    }
}