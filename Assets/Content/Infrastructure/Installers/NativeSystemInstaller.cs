using Content.Boids.Impl_Native;
using Zenject;

namespace Content.Infrastructure.Installers
{
    public class NativeSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NativeBoidInitializer>().AsSingle();
        }
    }
}