using TandC.RpgAdventure.Bootstrap.Units;
using TandC.RpgAdventure.Utilities;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;

namespace TandC.RpgAdventure.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;

        public BootstrapFlow(LoadingService loadingService, SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);

            _sceneManager.LoadScene(AppConstants.Scenes.Loading).Forget();
        }
    }
}
