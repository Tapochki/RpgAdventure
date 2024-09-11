using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Config;
using UnityEngine;
using UnityEngine.Tilemaps;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;

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
        private readonly PlayerSpawner _playerSpawner;
        private readonly ClickDetector2D _clickDetector2D;
        private readonly ICameraService _cameraService;
        private readonly IUIService _uiService;

        public CoreFlow(LoadingService loadingService, SceneManager sceneManager, TilemapFactory tilemapFactory, PlayerFactory playerFactory,
            TilemapViewModel tilemapViewModel, TilemapView tilemapView, LevelConfig levelConfig, PlayerSpawner playerSpawner, ClickDetector2D clickDetector, 
            ICameraService cameraService, IUIService uiService)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _tilemapFactory = tilemapFactory;
            _playerFactory = playerFactory;
            _tilemapViewModel = tilemapViewModel;
            _tilemapView = tilemapView;
            _levelConfig = levelConfig;
            _playerSpawner = playerSpawner;
            _clickDetector2D = clickDetector;
            _cameraService = cameraService;
            _uiService = uiService;
        }

        public async void Start()
        {
            RegisterUi();

            int levelId = 0;

            if (!_levelConfig.LevelExists(levelId))
            {
                Debug.LogError($"Level {levelId} does not exist.");
                return;
            }

            var tilemap = _levelConfig.GetRandomTileMapForLevel(levelId);

            Tilemap tilemapInstance = _tilemapFactory.CreateTilemap(tilemap);
            _tilemapViewModel.SetTilemap(tilemapInstance);
            _tilemapView.SetTileViewModel(_tilemapViewModel, tilemapInstance);

            var playerRace = RaceType.Human;
            var playerPrefab = _playerFactory.CreatePlayer(playerRace);
            _playerSpawner.SetPlayerPrefab(playerPrefab);

            await _loadingService.BeginLoading(_tilemapViewModel);
            await _loadingService.BeginLoading(_clickDetector2D);

            _playerSpawner.Initialize(tilemapInstance); //TODO make player object factory
            _tilemapView.Initialize();
            _cameraService.Init(_playerSpawner.Player.transform);
        }

        public void RegisterUi() 
        {
            
        }
    }
}