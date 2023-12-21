using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public interface IPersistentDataService
    {
        BoidsSettingsData BoidsSettings { get; set; }
        BoidsStateData BoidsState { get; set; }
        PlayerStateData PlayerState { get; set; }
    }
}