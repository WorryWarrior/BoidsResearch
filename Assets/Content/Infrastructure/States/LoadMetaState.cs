using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly GameStateMachine _stateMachine;
        //private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        
        public LoadMetaState(
            GameStateMachine stateMachine,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneName.Meta, OnMetaSceneLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnMetaSceneLoaded(SceneName sceneName)
        {
            
        }
    }
}