using Content.Infrastructure.States;
using Content.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.Hub.Menu
{
    public class MenuController : MonoBehaviour
    {
        public ToggleGroup levelCardContainer;
        [SerializeField] private Button startStageButton;
        
        private StageStaticData _selectedStage;
        public StageStaticData SelectedStage
        {
            get => _selectedStage;
            set
            {
                _selectedStage = value;
                OnSelectedStageChanged(value);
            }
        }
        
        private GameStateMachine _stateMachine;
        
        [Inject]
        private void Construct(
            GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Initialize()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            startStageButton.onClick.AddListener(() =>
            {
                _stateMachine.Enter<LoadLevelState, StageStaticData>(SelectedStage);
            });
        }
        
        private void OnSelectedStageChanged(StageStaticData value)
        {
            startStageButton.interactable = value != null;
        }
    }
}