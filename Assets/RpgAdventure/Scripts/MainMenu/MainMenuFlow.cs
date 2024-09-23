using TandC.RpgAdventure.Bootstrap.Units;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.UI.MainMenu;
using VContainer.Unity;

namespace TandC.RpgAdventure.MainMenu
{
    public class MainMenuFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly IUIService _uiService;

        public MainMenuFlow(LoadingService loadingService, SceneManager sceneManager, IUIService uiService)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _uiService = uiService;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(1));

            _uiService.Init();
            _uiService.RegisterPage(new MainMenuPage());
            _uiService.RegisterPage(new SettingsPage());
            _uiService.RegisterPage(new SelectCharacterPage());
            //_sceneManager.LoadScene(AppConstants.Scenes.MainMenu).Forget();
        }
    }
}