using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.StaticData;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        
        public BootstrapState(
            GameStateMachine gameStateMachine,
            IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }
        
        public void Enter()
        {
            _staticDataService.Initialized += () =>
                _stateMachine.Enter<LoadProgressState>();
        }
        
        public void Exit()
        {
            
        }
    }
}