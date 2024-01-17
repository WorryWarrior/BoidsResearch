using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public interface IPersistentDataService
    {
        BoidSettingsData BoidSettings { get; set; }
        PlayerStateData PlayerState { get; set; }
    }
}