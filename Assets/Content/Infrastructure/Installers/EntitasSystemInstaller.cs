using Content.Boids.Impl_Entitas.Systems;
using Zenject;

namespace Content.Infrastructure.Installers
{
    public class EntitasSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InitializeBoidsSystem>().AsSingle().NonLazy();
            Container.Bind<InitializeBoidGameObjectsSystem>().AsSingle().NonLazy();
        }
    }
}