using System.Threading.Tasks;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using UnityEngine;

namespace Content.Infrastructure.Factories
{
    public class BoidFactory : IBoidFactory
    {
        private const string BoidPrefabId = "PFB_Boid";
        
        private readonly IAssetProvider _assetProvider;
        
        public BoidFactory(
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(BoidPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(BoidPrefabId);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(BoidPrefabId);
            GameObject boid = Object.Instantiate(prefab, at, Quaternion.identity);
            
            return boid;
        }
    }
}