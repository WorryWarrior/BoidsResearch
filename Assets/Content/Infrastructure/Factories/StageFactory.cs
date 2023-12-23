using System.Threading.Tasks;
using Content.Boids.Interfaces;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

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
            //await _assetProvider.Load<GameObject>(EntitasBoidsControllerId);
        }

        public void CleanUp()
        {
            //_assetProvider.Release(EntitasBoidsControllerId);
        }

        public async Task<IBoidsController> CreateBoidsController(BoidsControllerType boidsControllerType)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>("PFB_EntitasSystemController");
            IBoidsController controller = Object.Instantiate(prefab).GetComponent<IBoidsController>();
            
            _container.Inject(controller);
            
            return controller;
        }
    }
}