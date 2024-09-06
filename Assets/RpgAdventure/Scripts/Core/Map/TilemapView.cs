using System.Collections.Generic;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class TilemapView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private GameObject _placeholderPrefab;
        [SerializeField] private GameObject _playerPrefab;

        [SerializeField] private Vector3 _step;

        [SerializeField] private FogOfWar _fogOfWar;
        [SerializeField] private Tilemap _structureTilemap;
        [SerializeField] private StructureConfig _structureConfig;
        [SerializeField] private DataService _dataService;

        [Inject] private TilemapViewModel _viewModel;
        [Inject] private ClickDetector2D _clickDetector;

        private List<GameObject> _placeholders = new List<GameObject>();
        private PlayerSpawner _playerSpawner;

        public void Start()
        {
            if (AppConstants.DEBUG_ENABLE) 
            {
                _clickDetector = GameObject.Find("ClickDetector").GetComponent<ClickDetector2D>();
                _viewModel = new TilemapViewModel(_tilemap, _dataService, _structureConfig);
                _fogOfWar.SetTileViewModel(_viewModel);
                _playerSpawner = new PlayerSpawner(_viewModel, _tilemap, _playerPrefab, _step, _fogOfWar);

                InitializeTiles();
                _playerSpawner.SpawnPlayer(_viewModel.CurrentCentralTile);
                UpdateTileVisibility();
                HandleClicks();
            }
        }

        //public void Initialize()
        //{
        //    //  _viewModel = new TilemapViewModel(_tilemap);
        //    _playerSpawner = new PlayerSpawner(_viewModel, _tilemap, _playerPrefab, _step, _fogOfWar);
        //    InitializeTiles();
        //    _playerSpawner.SpawnPlayer(_viewModel.CurrentCentralTile);
        //    UpdateTileVisibility();
        //    HandleClicks();
        //}

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S)) 
            {
                _viewModel.SaveWorldState();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                _viewModel.LoadTileMapViewModel();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _dataService.ResetData(CacheType.MapData);
                _structureTilemap.ClearAllTiles();
                //Initialize();
            }
        }

        private void InitializeTiles()
        {
            foreach (var tile in _viewModel.Tiles)
            {
                CreatePlaceHolders(tile);
                //CreateStructure(tile);
                OpenFogOfWar(tile);
            }
        }

        private void CreatePlaceHolders(TileModel tileModel) 
        {
            var worldPosition = _tilemap.CellToWorld(tileModel.Position) + _step;
            var placeholder = Instantiate(_placeholderPrefab, worldPosition, Quaternion.identity, transform);
            placeholder.name = $"Placeholder_{tileModel.Type}_{tileModel.Position}";
            placeholder.SetActive(false);
            _placeholders.Add(placeholder);
        }

        private void CreateStructure(TileModel tileModel) 
        {
            Tile structureTile = _structureConfig.GetStructureTile(tileModel.StructureTileType);
            if (structureTile != null)
            {
                _structureTilemap.SetTile(tileModel.Position, structureTile);
            }
        }

        private void OpenFogOfWar(TileModel tileModel) 
        {
            if(tileModel.IsOpen) 
            {
                Debug.LogError($"{tileModel.Position} must be open");
                _fogOfWar.OpenFogPosition(tileModel.Position);
            }
        }

        private void HandleClicks()
        {
            _clickDetector.OnObjectClicked
                .Select(clickedObject => clickedObject.transform.position)
                .Subscribe(clickPosition => HandleTileClick(clickPosition))
                .AddTo(this);
        }

        public void HandleTileClick(Vector3 clickPosition)
        {
            Debug.LogError(clickPosition);
            var cellPosition = _tilemap.WorldToCell(clickPosition);
            var clickedTile = _viewModel.GetTileAtPosition(cellPosition);

            if (clickedTile != null &&
                (clickedTile.Type == TileType.Land || clickedTile.Type == TileType.Sand))
            {
                _playerSpawner.MovePlayerToTile(clickedTile);
                UpdateTileVisibility();
            }
        }

        public void UpdateTileVisibility()
        {
            List<TileModel> surroundingTiles = _viewModel.GetSurroundingTiles();

            foreach (var placeholder in _placeholders)
            {
                Vector3Int placeholderPosition = _tilemap.WorldToCell(placeholder.transform.position - _step);
                bool isVisible = surroundingTiles.Exists(t => t.Position == placeholderPosition);

                SetPlaceholderVisible(placeholder, isVisible);
            }
        }

        private void SetPlaceholderVisible(GameObject placeholder, bool visible)
        {
            placeholder?.SetActive(visible);
        }
    }
}

