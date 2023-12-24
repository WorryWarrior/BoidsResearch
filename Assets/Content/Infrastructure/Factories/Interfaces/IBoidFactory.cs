using System.Threading.Tasks;
using UnityEngine;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IBoidFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<GameObject> Create(Vector3 at);
    }
}