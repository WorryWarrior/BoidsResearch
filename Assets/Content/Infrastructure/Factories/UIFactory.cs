using System.Threading.Tasks;
using Content.Hub.HUD;
using Content.Hub.Menu;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.StaticData;
using Content.StaticData;
using UnityEngine;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId          = "PFB_UIRoot";
        private const string HudPrefabId             = "PFB_Hud";
        private const string SelectionMenuPrefabId   = "PFB_SelectionMenu";
        private const string LevelCardPrefabId       = "PFB_LevelCard";
        
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        
        private Canvas _uiRoot;

        public UIFactory(
            DiContainer container,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider)
        {
            _container = container;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }
        
        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(UIRootPrefabId);
            //await _assetProvider.Load<GameObject>(HudPrefabId);
            
            await _assetProvider.Load<GameObject>(SelectionMenuPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(key: SelectionMenuPrefabId);
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<HudController> CreateHud()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(HudPrefabId);
            HudController hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<HudController>();

            _container.Inject(hud);
            return hud;
        }

        public async Task<MenuController> CreateSelectionMenu()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(SelectionMenuPrefabId);
            MenuController menu = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<MenuController>();

            foreach (var stageData in _staticDataService.GetAllStages()) 
                await CreateLevelCard(stageData, menu);

            _container.InjectGameObject(menu.gameObject);
            return menu;
        }

        private async Task CreateLevelCard(StageStaticData stageStaticData, MenuController menu)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(LevelCardPrefabId);
            //Sprite sprite = await _assetProvider.Load<Sprite>(stageStaticData.StageKey);
            MenuLevelCard card = Object.Instantiate(prefab, menu.levelCardContainer.transform).GetComponent<MenuLevelCard>();

            card.OnSelect += st => menu.SelectedStage = st;
            card.Initialize(stageStaticData, null, menu.levelCardContainer);
        }
    }
}