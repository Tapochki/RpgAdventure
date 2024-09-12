using VContainer.Unity;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Config;
using UnityEngine;
using UnityEngine.Tilemaps;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;
using TandC.RpgAdventure.Core.Map.MapObject;

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
        private readonly PlayerController _playerController;
        private readonly ClickDetector2D _clickDetector2D;
        private readonly ICameraService _cameraService;
        private readonly IUIService _uiService;
        private readonly MapObjectView _mapObjectView;
        private readonly MapObjectViewModel _mapObjectViewModel;

        public CoreFlow(LoadingService loadingService, SceneManager sceneManager, IUIService uiService,
            TilemapFactory tilemapFactory, TilemapViewModel tilemapViewModel, TilemapView tilemapView,
             PlayerFactory playerFactory, LevelConfig levelConfig, PlayerController playerSpawner,
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
            _playerController = playerSpawner;
            _clickDetector2D = clickDetector;
            _cameraService = cameraService;
            _uiService = uiService;
            _mapObjectView = mapObjectView;
            _mapObjectViewModel = mapObjectViewModel;
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

            Tilemap tilemapInstance = CreateTileMap(levelId);

            CreatePlayer();
            _mapObjectView.Initialize(tilemapInstance);

            await _loadingService.BeginLoading(_tilemapViewModel);
            await _loadingService.BeginLoading(_clickDetector2D);
            await _loadingService.BeginLoading(_mapObjectViewModel);

            _playerController.Initialize(tilemapInstance); //TODO make player object factory
            _tilemapView.Initialize();
            _cameraService.Init(_playerController.Player.transform);

            RegisterSubscribe();
        }

        public void RegisterUi() 
        {
            
        }

        public Tilemap CreateTileMap(int levelId) 
        {
            var tilemap = _levelConfig.GetRandomTileMapForLevel(levelId);
            Tilemap tilemapInstance = _tilemapFactory.CreateTilemap(tilemap);
            _tilemapViewModel.SetTilemap(tilemapInstance);
            _tilemapView.SetTileViewModel(_tilemapViewModel, tilemapInstance);
            return tilemapInstance;
        }

        public void CreatePlayer() 
        {
            var playerRace = RaceType.Human;
            var playerPrefab = _playerFactory.CreatePlayer(playerRace);
            _playerController.SetPlayerPrefab(playerPrefab);
        }

        public void RegisterSubscribe() 
        {
            _playerController.OnPlayerMoveEnd += _tilemapView.HandleMoveEnd;
            _playerController.OnPlayerMoveStart += _tilemapView.HandleMoveStart;
        }
    }
}