using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.Services.StaticData;
using Zenject;

namespace Content.Infrastructure.Installers
{
    public class InfrastructureInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterProviders();
            RegisterServices();
            RegisterFactories();
        }

        private void RegisterProviders()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
        
        private void RegisterServices()
        {
            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle().NonLazy();
        }

        private void RegisterFactories()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}