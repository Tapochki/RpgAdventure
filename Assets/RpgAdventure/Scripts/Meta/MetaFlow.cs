using TandC.RpgAdventure.Bootstrap.Units;
using TandC.RpgAdventure.Utilities;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using TandC.RpgAdventure.Settings;
using TandC.RpgAdventure.Services;

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
            await _loadingService.BeginLoading(new FooLoadingUnit(3));
            _sceneManager.LoadScene(AppConstants.Scenes.Core).Forget();
        }
    }
}