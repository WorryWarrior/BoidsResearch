using Content.Data;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class GameLoopState : IPayloadedState<GameLoopData>
    {
        private GameLoopData _gameLoopData;

        public void Enter(GameLoopData gameLoopData)
        {
            _gameLoopData = gameLoopData;
        }

        public void Exit()
        {
            _gameLoopData.LevelBoidsSimulationController.DestroyBoids();
        }
    }
}