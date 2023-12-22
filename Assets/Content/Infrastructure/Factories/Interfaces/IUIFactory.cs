using System.Threading.Tasks;
using Content.Hub.HUD;
using Content.Hub.Menu;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<HudController> CreateHud();
        Task<MenuController> CreateSelectionMenu();
    }
}