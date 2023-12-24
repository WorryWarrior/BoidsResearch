using System.Threading.Tasks;
using Content.Boids.Interfaces;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IStageFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<IBoidsSimulationController> CreateBoidsController(BoidsSimulationType boidsSimulationType);
    }
}