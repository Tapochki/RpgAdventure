using Cysharp.Threading.Tasks;
using TandC.RpgAdventure.Bootstrap.Units;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using VContainer.Unity;

namespace TandC.RpgAdventure.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;

        public MetaFlow(LoadingService loadingService, SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(1));
            _sceneManager.LoadScene(AppConstants.Scenes.MainMenu).Forget();
        }
    }
}