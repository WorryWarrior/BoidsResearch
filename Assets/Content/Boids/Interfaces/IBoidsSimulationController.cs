using System;

namespace Content.Boids.Interfaces
{
    public interface IBoidsSimulationController
    {
        public event Action Initialized;
        void InitializeBoids();
    }
}