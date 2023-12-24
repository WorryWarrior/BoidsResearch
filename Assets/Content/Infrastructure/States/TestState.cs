using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class TestState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;
        
        public TestState(GameStateMachine gameStateMachine, 
            IPersistentDataService persistentDataService, 
            ISaveLoadService saveLoadService)
        {
            _stateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }
        
        public void Enter()
        {
            
        }
        
        public void Exit()
        {
            
        }
    }
}