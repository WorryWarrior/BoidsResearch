using System.Threading.Tasks;
using Content.Data;

namespace Content.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SavePlayerState();
        Task<PlayerStateData> LoadPlayerState();
        void SaveBoidsSettings();
        Task<BoidsSettingsData> LoadBoidsSettings();
        void SaveBoidsState();
        Task<BoidsStateData> LoadBoidsState();
    }
}