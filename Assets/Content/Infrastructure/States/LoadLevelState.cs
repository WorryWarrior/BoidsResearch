using System.Threading.Tasks;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Interfaces;
using Content.StaticData;

namespace Content.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<StageStaticData>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IStageFactory _stageFactory;
        
        public LoadLevelState(
            GameStateMachine gameStateMachine,
            ISceneLoader sceneLoader,
            IUIFactory uiFactory,
            IStageFactory stageFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _stageFactory = stageFactory;
        }
        
        public async void Enter(StageStaticData stageStaticData)
        {
            await _stageFactory.WarmUp();
            
            await _sceneLoader.LoadScene(SceneName.Core, OnSceneLoaded);
        }
        
        public void Exit()
        {
            _stageFactory.CleanUp();
        }

        private async void OnSceneLoaded(SceneName sceneName)
        {
            await InitGameWorld();
        }

        private async Task InitGameWorld()
        {
            await InitBoidController();
        }

        private async Task InitBoidController()
        {
            IBoidsController boidsController = await _stageFactory.CreateBoidsController(BoidsControllerType.Entitas);
            boidsController.InitializeBoids();
        }
    }
}