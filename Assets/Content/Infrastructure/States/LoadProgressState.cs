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
        private readonly IPersistentDataService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(
            GameStateMachine stateMachine,
            IPersistentDataService progressService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
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
            _progressService.PlayerState = await _saveLoadService.LoadPlayerState() ?? CreateNewPlayerState();
        }

        private PlayerStateData CreateNewPlayerState() => 
            new()
            {
                PlayerPosition = Vector3.zero
            };
    }
}