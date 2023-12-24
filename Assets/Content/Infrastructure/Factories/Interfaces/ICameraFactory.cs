using System.Threading.Tasks;
using UnityEngine;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface ICameraFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<GameObject> CreateCameraActor(Vector3 at);
    }
}