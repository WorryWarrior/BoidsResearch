using System.Threading.Tasks;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class CameraFactory : ICameraFactory
    {
        private const string CameraPrefabId = "PFB_CameraActor";
        
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        
        public CameraFactory(
            DiContainer container,
            IAssetProvider assetProvider)
        {
            _container = container;
            _assetProvider = assetProvider;
        }
        
        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(CameraPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(CameraPrefabId);
        }

        public async Task<GameObject> CreateCameraActor(Vector3 at)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(CameraPrefabId);
            GameObject cameraActor = Object.Instantiate(prefab, at, Quaternion.identity);
            
            _container.InjectGameObject(cameraActor);
            
            return cameraActor;
        }
    }
}