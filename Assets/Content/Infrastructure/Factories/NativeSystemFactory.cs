using Content.Boids.Impl_Native;
using Unity.Entities;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class NativeSystemFactory
    {
        private readonly DiContainer _container;

        public NativeSystemFactory(DiContainer container) =>
            _container = container;

        public NativeBoidInitializer CreateBoidInitializer() =>
            _container.Resolve<NativeBoidInitializer>();

        public T PrepareExistingSystem<T>() where T : SystemBase
        {
            T systemInstance = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<T>();
            _container.Inject(systemInstance);
            return systemInstance;
        }
    }
}