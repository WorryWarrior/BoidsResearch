using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }
        
        public void Enter()
        {
            LoadProgressOrCreateNew();
            
            _stateMachine.Enter<LoadMetaState>();
        }
        
        public void Exit()
        {
            
        }

        private async void LoadProgressOrCreateNew()
        {
            _persistentDataService.BoidsSettings = await _saveLoadService.LoadBoidsSettings() 
                                                   ?? CreateNewBoidSettings();
            _persistentDataService.PlayerState = await _saveLoadService.LoadPlayerState() 
                                                 ?? CreateNewPlayerState();
        }

        private PlayerStateData CreateNewPlayerState() => 
            new()
            {
                PlayerPosition = Vector3.zero
            };

        private BoidsSettingsData CreateNewBoidSettings() =>
            new()
            {
                BoidCount = 1000,
                SpawnRadius = 40f,
                MinSpeed = 10f,
                MaxSpeed = 12f,
                PerceptionRadius = 2.5f,
                AvoidanceRadius = 3f,
                MaxSteerForce = 7.5f,
                AlignmentWeight = 5,
                CohesionWeight = .75f,
                SeparationWeight = 2f,
                TargetWeight = .75f,
                BoundsRadius = 1f,
                AvoidCollisionWeight = 20f,
                CollisionAvoidDistance = 5
            };
    }
}