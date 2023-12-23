using Content.Infrastructure.AssetManagement;
using Entitas;
using UnityEngine;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class InitializeBoidGameObjectsSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private IAssetProvider _assetProvider;
        
        public InitializeBoidGameObjectsSystem(
            IAssetProvider assetProvider
            /*GameContext context*/)
        {
            _assetProvider = assetProvider;
            
            _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
                GameMatcher.LinkedGO));
        }
        // BOID FACTORY 
        public async void Initialize()
        {
            GameObject parentGO = new GameObject("Boid_LinkedGO_Parent");
            GameObject prefab = await _assetProvider.Load<GameObject>("PFB_Boid");
            int k = 0;
            
            foreach (GameEntity e in _boidsGroup)
            {
                GameObject linkedGO = Object.Instantiate(prefab, parentGO.transform, true);
                linkedGO.name = $"Linked_GO_{k}";

                e.ReplaceLinkedGO(linkedGO);

                k++;
            }
        }
    }
}