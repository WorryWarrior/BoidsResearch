using System;
using System.Threading.Tasks;
using Content.Boids.Impl_Native.Systems;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Content.Boids.Impl_Native
{
    public class NativeSimulationController : MonoBehaviour, IBoidsSimulationController
    {
        public event Action Initialized;

        private NativeSystemFactory _systemFactory;
        private NativeBoidInitializer _nativeBoidInitializer;

        [Inject]
        private void Construct(
            NativeSystemFactory systemFactory)
        {
            _systemFactory = systemFactory;
        }

        public async void InitializeBoids()
        {
            await SetupBoidsSimulation();
        }

        public void DestroyBoids()
        {
            TearDownBoidsSimulation();
        }

        private async Task SetupBoidsSimulation()
        {
            _nativeBoidInitializer = _systemFactory.CreateBoidInitializer();
            await _nativeBoidInitializer.InitializeBoids();
            _systemFactory.PrepareExistingSystem<UpdateBoidsParametersSystem>();
        }

        private void TearDownBoidsSimulation()
        {
            _nativeBoidInitializer.DestroyBoidEntities();
        }
    }
}