using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        public BoidSettingsData BoidSettings { get; set; }
        public PlayerStateData PlayerState { get; set; }
    }
}