using Content.Infrastructure.States;
using Zenject;

namespace Content.Infrastructure.Installers
{
    public class StateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapState>().AsSingle().NonLazy();
            Container.Bind<LoadProgressState>().AsSingle().NonLazy();
            Container.Bind<LoadMetaState>().AsSingle().NonLazy();
            Container.Bind<LoadLevelState>().AsSingle().NonLazy();
            Container.Bind<GameLoopState>().AsSingle().NonLazy();
            
            Container.Bind<TestState>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
    }
}