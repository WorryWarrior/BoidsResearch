using System.Threading.Tasks;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.States.Interfaces;
using Content.StaticData;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<StageStaticData>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IStageFactory _stageFactory;
        private readonly IBoidFactory _boidFactory;
        private readonly ICameraFactory _cameraFactory;
        private readonly ILoggingService _loggingService;
        
        private StageStaticData _currentStageStaticData;
        
        public LoadLevelState(
            GameStateMachine gameStateMachine,
            ISceneLoader sceneLoader,
            IUIFactory uiFactory,
            IStageFactory stageFactory,
            IBoidFactory boidFactory,
            ICameraFactory cameraFactory,
            ILoggingService loggingService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _stageFactory = stageFactory;
            _boidFactory = boidFactory;
            _cameraFactory = cameraFactory;
            _loggingService = loggingService;
        }
        
        public async void Enter(StageStaticData stageStaticData)
        {
            _currentStageStaticData = stageStaticData;
            
            await _stageFactory.WarmUp();
            await _boidFactory.WarmUp();
            await _cameraFactory.WarmUp();
            
            await _sceneLoader.LoadScene(SceneName.Core, OnSceneLoaded);
        }
        
        public void Exit()
        {
            _stageFactory.CleanUp();
            _boidFactory.CleanUp();
            _cameraFactory.CleanUp();
        }

        private async void OnSceneLoaded(SceneName sceneName)
        {
            await InitGameWorld();
            await InitUI();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUI()
        {
            await _uiFactory.CreateUIRoot();
            await _uiFactory
                .CreateHud()
                .ContinueWith(it => it.Result.Initialize(), 
                    TaskScheduler.FromCurrentSynchronizationContext());

        }
        
        private async Task InitGameWorld()
        {
            await InitBoidController();
            await InitCameraActor();
        }

        private async Task InitBoidController()
        {
            IBoidsSimulationController boidsSimulationController = 
                await _stageFactory.CreateBoidsController(_currentStageStaticData.BoidsSimulationType);
            
            boidsSimulationController.Initialized += () => 
                _loggingService.LogMessage("Boid Simulation Ready", this);
            boidsSimulationController.InitializeBoids();
        }

        private async Task InitCameraActor()
        {
            await _cameraFactory.CreateCameraActor(_currentStageStaticData.CameraSpawnPoint);
        }
    }
}