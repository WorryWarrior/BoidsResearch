using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        public BoidsSettingsData BoidsSettings { get; set; }
        public BoidsStateData BoidsState { get; set; }
        public PlayerStateData PlayerState { get; set; }
    }
}