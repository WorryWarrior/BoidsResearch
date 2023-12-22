using System.Threading.Tasks;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        
        public LoadMetaState(
            GameStateMachine stateMachine,
            IUIFactory uiFactory,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter()
        {
            await _uiFactory.WarmUp();
            await _sceneLoader.LoadScene(SceneName.Meta, OnMetaSceneLoaded);
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }

        private async void OnMetaSceneLoaded(SceneName sceneName)
        {
            await ConstructUIRoot();
            await ConstructSelectionMenu();
        }

        private async Task ConstructUIRoot() => await _uiFactory.CreateUIRoot();

        private async Task ConstructSelectionMenu()
        {
            await _uiFactory
                .CreateSelectionMenu()
                .ContinueWith(it => it.Result.Initialize(), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}