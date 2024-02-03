using Content.Hub.Settings;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
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
        private ISaveLoadService _saveLoadService;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            IStaticDataService staticDataService,
            ISaveLoadService saveLoadService,
            ILoggingService loggingService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
            _saveLoadService = saveLoadService;
            _loggingService = loggingService;
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
                    .Then(accepted =>
                    {
                        if (!accepted)
                            _saveLoadService.RestoreSavedBoidsSettings();

                        _saveLoadService.SaveBoidsSettings();
                    })
                );
        }
    }
}