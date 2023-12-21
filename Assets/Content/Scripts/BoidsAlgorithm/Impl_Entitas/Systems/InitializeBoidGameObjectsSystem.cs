using Entitas;
using UnityEngine;

namespace Boids.Entitas
{
    public class InitializeBoidGameObjectsSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;
        private readonly GameObject parentGO;

        public InitializeBoidGameObjectsSystem(GameContext context)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.LinkedGO));

            parentGO = new GameObject("Boid_LinkedGO_Parent");
        }

        public void Initialize()
        {
            int k = 0;
            GameObject refObject = GameObject.Find("Ref");
            
            foreach (GameEntity e in _boidsGroup)
            {
                GameObject linkedGO = Object.Instantiate(refObject, parentGO.transform, true);//GameObject.CreatePrimitive(PrimitiveType.Cube);
                linkedGO.name = $"Linked_GO_{k}";

                e.ReplaceLinkedGO(linkedGO);

                k++;
            }

            Object.Destroy(refObject);
        }
    }
}