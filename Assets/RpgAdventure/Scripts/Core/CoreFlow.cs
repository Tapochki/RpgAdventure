using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Config;
using UnityEngine;
using UnityEngine.Tilemaps;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Core.Map.MapObject;
using System.Threading.Tasks;

namespace TandC.RpgAdventure.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;
        private readonly TilemapFactory _tilemapFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly TilemapViewModel _tilemapViewModel;
        private readonly TilemapView _tilemapView;
        private readonly LevelConfig _levelConfig;
        private readonly PlayerViewModel _playerViewModel;
        private readonly ClickDetector2D _clickDetector2D;
        private readonly ICameraService _cameraService;
        private readonly IUIService _uiService;
        private readonly MapObjectView _mapObjectView;
        private readonly MapObjectViewModel _mapObjectViewModel;

        public CoreFlow(LoadingService loadingService, SceneManager sceneManager, IUIService uiService,
            TilemapFactory tilemapFactory, TilemapViewModel tilemapViewModel, TilemapView tilemapView,
             PlayerFactory playerFactory, LevelConfig levelConfig,
             PlayerViewModel playerViewModel,
             MapObjectView mapObjectView, MapObjectViewModel mapObjectViewModel,
            ICameraService cameraService, ClickDetector2D clickDetector)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _tilemapFactory = tilemapFactory;
            _playerFactory = playerFactory;
            _tilemapViewModel = tilemapViewModel;
            _tilemapView = tilemapView;
            _levelConfig = levelConfig;
            _playerViewModel = playerViewModel;
            _clickDetector2D = clickDetector;
            _cameraService = cameraService;
            _uiService = uiService;
            _mapObjectView = mapObjectView;
            _mapObjectViewModel = mapObjectViewModel;
        }

        public async void Start()
        {
            RegisterUi();

            if (!TryInitializeLevel(0, out var tilemapInstance))
                return;

            InitializePlayer(tilemapInstance);
            _mapObjectView.Initialize(tilemapInstance);

            await LoadAssetsAsync();

            FinalizeInitialization();
        }

        private bool TryInitializeLevel(int levelId, out Tilemap tilemapInstance)
        {
            if (!_levelConfig.LevelExists(levelId))
            {
                Debug.LogError($"Level {levelId} does not exist.");
                tilemapInstance = null;
                return false;
            }

            tilemapInstance = CreateTileMap(levelId);
            return true;
        }

        private Tilemap CreateTileMap(int levelId)
        {
            var tilemap = _levelConfig.GetRandomTileMapForLevel(levelId);
            var tilemapInstance = _tilemapFactory.CreateTilemap(tilemap);
            _tilemapViewModel.SetTilemap(tilemapInstance);
            _tilemapView.SetTileViewModel(_tilemapViewModel, tilemapInstance);
            return tilemapInstance;
        }

        private void InitializePlayer(Tilemap tilemap)
        {
            var playerModel = _playerFactory.CreatePlayer(Settings.RaceType.Human, Settings.ClassType.Warrior, tilemap);
            _playerViewModel.SetPlayerModel(playerModel);
        }

        private async Task LoadAssetsAsync()
        {
            await _loadingService.BeginLoading(_tilemapViewModel);
            await _loadingService.BeginLoading(_clickDetector2D);
            await _loadingService.BeginLoading(_mapObjectViewModel);
        }

        private void FinalizeInitialization()
        {
            _tilemapView.Initialize();
            _cameraService.Init(_playerViewModel.Player.transform);
            RegisterSubscriptions();
        }

        private void RegisterUi()
        {
            // UI Registration Logic Here
        }

        private void RegisterSubscriptions()
        {
            _playerViewModel.OnPlayerMoveEnd += _tilemapView.HandleMoveEnd;
            _playerViewModel.OnPlayerMoveStart += _tilemapView.HandleMoveStart;
        }
    }
}