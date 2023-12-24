using System;
using System.Threading.Tasks;
using Content.Boids.Interfaces;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Content.Infrastructure.Factories
{
    public class StageFactory : IStageFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        
        public StageFactory(
            DiContainer container, 
            IAssetProvider assetProvider, 
            IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }
        
        public async Task WarmUp()
        {
            foreach (BoidsSimulationType boidsSimulationType in
                     (BoidsSimulationType[])Enum.GetValues(typeof(BoidsSimulationType)))
                await _assetProvider.Load<GameObject>(boidsSimulationType.ToSimulationControllerId());

        }

        public void CleanUp()
        {
            foreach (BoidsSimulationType boidsSimulationType in 
                     (BoidsSimulationType[]) Enum.GetValues(typeof(BoidsSimulationType)))
                 _assetProvider.Release(boidsSimulationType.ToSimulationControllerId());
        }

        public async Task<IBoidsSimulationController> CreateBoidsController(BoidsSimulationType boidsSimulationType)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(boidsSimulationType.ToSimulationControllerId());
            IBoidsSimulationController simulationController = Object.Instantiate(prefab).GetComponent<IBoidsSimulationController>();
            
            _container.Inject(simulationController);
            
            return simulationController;
        }
    }
}