using System;
using Content.Infrastructure.Factories.Interfaces;
using Entitas;
using UnityEngine;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class InitializeBoidGameObjectsSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.LinkedGO));
        private readonly IBoidFactory _boidFactory;
        
        public InitializeBoidGameObjectsSystem(
            IBoidFactory boidFactory)
        {
            _boidFactory = boidFactory;
        }
        
        public async void Initialize()
        {
            GameObject parentGO = new GameObject("Boid_LinkedGO_Parent");
            int k = 0;
            
            foreach (GameEntity e in _boidsGroup)
            {
                GameObject linkedGO = await _boidFactory.Create(Vector3.zero);
                linkedGO.transform.parent = parentGO.transform;
                linkedGO.name = $"Linked_GO_{k}";

                e.ReplaceLinkedGO(linkedGO);

                k++;
            }
        }
    }
}