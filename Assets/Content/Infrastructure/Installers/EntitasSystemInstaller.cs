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
            Container.Bind<CalculateFlockParametersSystem>().AsSingle().NonLazy();
            Container.Bind<CheckCollisionTrajectorySystem>().AsSingle().NonLazy();
            Container.Bind<CalculateTargetOffsetSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateCohesionSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateSeparationSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateAlignmentSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateCollisionAvoidanceSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateAccelerationSystem>().AsSingle().NonLazy();
            Container.Bind<CalculateVelocitySystem>().AsSingle().NonLazy();
            Container.Bind<UpdatePositionSystem>().AsSingle().NonLazy();
            Container.Bind<UpdateRotationSystem>().AsSingle().NonLazy();
            Container.Bind<UpdateLinkedGameObjectPositionSystem>().AsSingle().NonLazy();
            
        }
    }
}