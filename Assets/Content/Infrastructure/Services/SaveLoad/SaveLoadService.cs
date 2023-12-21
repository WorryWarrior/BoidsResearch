using System.Threading.Tasks;
using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PlayerStateKey = "PlayerState";
        private const string BoidsSettingsKey = "BoidsSettings";
        private const string BoidsStateKey = "BoidsState";
        
        private readonly IPersistentDataService _persistentDataService;

        public SaveLoadService(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }
        
        public void SavePlayerState()
        {
            string playerStateJson = SerializeObject(_persistentDataService.PlayerState);
            PlayerPrefs.SetString(PlayerStateKey, playerStateJson);
        }

        public Task<PlayerStateData> LoadPlayerState()
        {
            PlayerStateData playerState = DeserializeObject<PlayerStateData>(PlayerPrefs.GetString(PlayerStateKey));
            return Task.FromResult(playerState);
        }

        public void SaveBoidsSettings()
        {
            throw new System.NotImplementedException();
        }

        public Task<BoidsSettingsData> LoadBoidsSettings()
        {
            throw new System.NotImplementedException();
        }

        public void SaveBoidsState()
        {
            throw new System.NotImplementedException();
        }

        public Task<BoidsStateData> LoadBoidsState()
        {
            throw new System.NotImplementedException();
        }
    }
}