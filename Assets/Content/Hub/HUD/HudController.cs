using Content.Hub.Settings;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.StaticData;
using Content.Infrastructure.States;
using RSG;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.Hub.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Button returnButton = null;
        [SerializeField] private Button settingsButton = null;

        [SerializeField] private SettingsWindow settingsWindow = null;
        
        private GameStateMachine _stateMachine;
        private IPersistentDataService _persistentDataService;
        private IStaticDataService _staticDataService;
        
        [Inject]
        private void Construct(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            returnButton.onClick.AddListener(() => 
                _stateMachine.Enter<LoadMetaState>());
            
            settingsButton.onClick.AddListener(() =>
                settingsWindow.Show(Tuple.Create(_persistentDataService.BoidSettings, 
                    _staticDataService.GetSettingsStaticData()))
                    //.Then(it => Debug.Log(it))
                );
        }
    }
}