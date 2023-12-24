using System;

namespace Content.Boids.Interfaces
{
    public interface IBoidsSimulationController
    {
        Action Initialized { get; set; }
        void InitializeBoids();
    }
}