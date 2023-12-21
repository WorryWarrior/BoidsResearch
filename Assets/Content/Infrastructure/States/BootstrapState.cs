using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        
        public BootstrapState(
            GameStateMachine gameStateMachine,
            ISceneLoader sceneLoader)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneName.Boot);
            
            _stateMachine.Enter<LoadProgressState>();
        }
        
        public void Exit()
        {
            
        }
    }
}