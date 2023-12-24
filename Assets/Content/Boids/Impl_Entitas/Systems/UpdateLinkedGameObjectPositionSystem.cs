using Entitas;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class UpdateLinkedGameObjectPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.LinkedGO,
            GameMatcher.Position,
            GameMatcher.Rotation
        ));

        public void Execute()
        {
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.linkedGO.linkedGO == null)
                    return;
                
                e.linkedGO.linkedGO.transform.position = e.position.value;
                e.linkedGO.linkedGO.transform.forward = e.rotation.value;
            }
        }
    }
}