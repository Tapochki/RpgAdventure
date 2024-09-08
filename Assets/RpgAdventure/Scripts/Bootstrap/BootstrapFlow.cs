using TandC.RpgAdventure.Bootstrap.Units;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;

namespace TandC.RpgAdventure.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly SceneManager _sceneManager;

        public BootstrapFlow(LoadingService loadingService, SceneManager sceneManager, DataService dataService)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _dataService = dataService;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();

            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_dataService);


            _sceneManager.LoadScene(AppConstants.Scenes.Loading).Forget();
        }
    }
}
