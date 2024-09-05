using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Core.HexGrid;

namespace TandC.RpgAdventure.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly TilemapViewModel _tilemapViewModel;
        private readonly TilemapView _tilemapView;

        public CoreFlow(LoadingService loadingService, SceneManager sceneManager, TilemapViewModel tilemapViewModel, TilemapView tilemapView)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _tilemapViewModel = tilemapViewModel;
            _tilemapView = tilemapView;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(_tilemapViewModel);

           // _tilemapView.Initialize();
        }
    }
}