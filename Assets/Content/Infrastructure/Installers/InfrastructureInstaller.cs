using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States;
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
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
        }
        
        private void RegisterServices()
        {
            Container.BindInterfacesTo<LoggingService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle().NonLazy();
        }

        private void RegisterFactories()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
        }
    }
}