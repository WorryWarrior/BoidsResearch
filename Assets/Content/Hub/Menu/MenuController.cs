using Content.Infrastructure.States;
using Content.StaticData;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.Hub.Menu
{
    public class MenuController : MonoBehaviour
    {
        public ToggleGroup levelCardContainer;
        [SerializeField] private Button selectStageButton;

        public readonly ReactiveProperty<StageStaticData> SelectedStage = new();

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
            SelectedStage
                .Subscribe(it => selectStageButton.interactable = it != null);

            selectStageButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, StageStaticData>(SelectedStage.Value)
            );
        }
    }
}