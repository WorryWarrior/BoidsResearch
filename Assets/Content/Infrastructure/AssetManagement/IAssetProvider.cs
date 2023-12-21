using System.Threading.Tasks;
using Zenject;

namespace Content.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IInitializable
    {
        public Task<T> Load<T>(string key) where T : class;
        public void Release(string key);
        public void Cleanup();
    }
}