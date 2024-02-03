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
            _persistentDataService.BoidSettings = await _saveLoadService.LoadBoidsSettings()
                                                   ?? CreateNewBoidSettings();
            _persistentDataService.PlayerState = await _saveLoadService.LoadPlayerState()
                                                 ?? CreateNewPlayerState();
        }

        private PlayerStateData CreateNewPlayerState() =>
            new()
            {
                PlayerPosition = Vector3.zero
            };

        private BoidSettingsData CreateNewBoidSettings() =>
            new()
            {
                BoidCount = 2250,
                SpawnRadius = 10f,
                MinSpeed = 10f,
                MaxSpeed = 12f,
                PerceptionRadius = 4.5f,
                AvoidanceRadius = 4.5f,
                MaxSteerForce = 12.5f,
                AlignmentWeight = 5f,
                CohesionWeight = 2.25f,
                SeparationWeight = 3f,
                TargetWeight = 1f,
                BoundsRadius = 1f,
                CollisionAvoidanceWeight = 20f,
                CollisionAvoidanceDistance = 5
            };
    }
}