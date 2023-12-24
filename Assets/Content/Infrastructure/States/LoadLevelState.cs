using System.Threading.Tasks;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Logging;
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
        private readonly IBoidFactory _boidFactory;
        private readonly ILoggingService _loggingService;
        
        private StageStaticData _currentStageStaticData;
        
        public LoadLevelState(
            GameStateMachine gameStateMachine,
            ISceneLoader sceneLoader,
            IUIFactory uiFactory,
            IStageFactory stageFactory,
            IBoidFactory boidFactory,
            ILoggingService loggingService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _stageFactory = stageFactory;
            _boidFactory = boidFactory;
            _loggingService = loggingService;
        }
        
        public async void Enter(StageStaticData stageStaticData)
        {
            _currentStageStaticData = stageStaticData;
            
            await _stageFactory.WarmUp();
            await _boidFactory.WarmUp();
            
            await _sceneLoader.LoadScene(SceneName.Core, OnSceneLoaded);
        }
        
        public void Exit()
        {
            _stageFactory.CleanUp();
            _boidFactory.CleanUp();
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
            IBoidsSimulationController boidsSimulationController = 
                await _stageFactory.CreateBoidsController(_currentStageStaticData.BoidsSimulationType);
            
            // Show loading curtain until boid simulation has finished initialization
            boidsSimulationController.Initialized += () => _loggingService.LogMessage("Boid Simulation Ready", this);
            boidsSimulationController.InitializeBoids();
        }
    }
}