using Entitas;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class EntitasSystemFactory
    {
        private readonly DiContainer _container;

        public EntitasSystemFactory(DiContainer container) =>
            _container = container;

        public T CreateSystem<T>() where T : ISystem =>
            _container.Resolve<T>();
    }
}