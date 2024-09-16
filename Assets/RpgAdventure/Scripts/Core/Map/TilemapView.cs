using System.Collections.Generic;
using TandC.RpgAdventure.Core.Map.MapObject;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Map
{
    public class TilemapView : MonoBehaviour
    {
        [SerializeField] private PlaceholderObject _placeholderPrefab;

        [Inject] private ClickDetector2D _clickDetector;
        [Inject] private FogOfWar _fogOfWar;
        [Inject] private MapObjectViewModel _mapObjectViewModel;
        [Inject] private MapObjectView _mapObjectView;
        [Inject] private PlayerController _playerSpawner;

        private TilemapViewModel _viewModel;

        private Tilemap _tilemap;

        private Vector3 _step;

        private List<PlaceholderObject> _placeholders = new List<PlaceholderObject>();
        private List<PlaceholderObject> _currentPlaceholders = new List<PlaceholderObject>();

        public void SetTileViewModel(TilemapViewModel tilemapViewModel, Tilemap currentTileMap)
        {
            _viewModel = tilemapViewModel;
            _tilemap = currentTileMap;
        }

        public void Initialize()
        {
            _step = AppConstants.TILE_STEP;
            InitializeTiles();
            _fogOfWar.InitializeFog();
            _playerSpawner.SpawnPlayer(_viewModel.CurrentCentralTile);
            List<TileModel> surroundingTiles = _viewModel.GetSurroundingTilesFromCentral();
            UpdateTileVisibility(surroundingTiles);
            ShowHideCurrentPlaceHodlers(true);
            HandleClicks();
        }

        private void InitializeTiles()
        {
            foreach (var tile in _viewModel.Tiles)
            {
                CreatePlaceHolders(tile);
                OpenFogOfWar(tile);
            }
        }

        private void CreatePlaceHolders(TileModel tileModel) 
        {
            var worldPosition = _tilemap.CellToWorld(tileModel.Position) + _step;
            var placeholder = Instantiate(_placeholderPrefab, worldPosition, Quaternion.identity, transform);
            placeholder.name = $"Placeholder_{tileModel.Type}_{tileModel.Position}";
            placeholder.gameObject.SetActive(false);
            _placeholders.Add(placeholder);
        }

        private void OpenFogOfWar(TileModel tileModel) 
        {
            if(tileModel.IsOpen) 
            {
                _fogOfWar.OpenFogPosition(tileModel.Position);
            }
        }

        private void HandleClicks()
        {
            _clickDetector.OnObjectClicked
                .Select(clickedObject => clickedObject.GetComponent<PlaceholderObject>())
                .Where(placeholder => placeholder != null)
                .Select(placeholder => placeholder.transform.position)
                .Subscribe(clickPosition => HandleTileClick(clickPosition))
                .AddTo(this);
        }

        public void HandleTileClick(Vector3 clickPosition)
        {
            var cellPosition = _tilemap.WorldToCell(clickPosition);
            var clickedTile = _viewModel.GetTileAtPosition(cellPosition);

            if (clickedTile != null &&
                (clickedTile.Type == TileType.Land || clickedTile.Type == TileType.Sand))
            {
                _viewModel.SetCurrentCentralTile(clickedTile.Position);             
                _playerSpawner.MovePlayerToTile(clickedTile);              
            }
        }

        public void HandleMoveStart() 
        {
            ShowHideCurrentPlaceHodlers(false);
            List<TileModel> surroundingTiles = _viewModel.GetSurroundingTilesFromCentral();
            List<Vector3Int> secondCirclePositions = _viewModel.GetSecondCircle(_viewModel.CurrentCentralTile.Position);
            List<TileModel> tilesForCreate = _viewModel.GetTilesByPositions(secondCirclePositions);
            _mapObjectViewModel.TryCreateNarrativeMark(tilesForCreate);
            _mapObjectView.UpLayerToOpenedStructure(surroundingTiles);
            _mapObjectViewModel.UpdateNarratives();
            UpdateTileVisibility(surroundingTiles);
        }

        public void HandleMoveEnd() 
        {
            ShowHideCurrentPlaceHodlers(true);
        }

        public void UpdateTileVisibility(List<TileModel> surroundingTiles)
        {
           
            _currentPlaceholders.Clear();
            foreach (var placeholder in _placeholders)
            {
                Vector3Int placeholderPosition = _tilemap.WorldToCell(placeholder.transform.position - _step);
                bool isVisible = surroundingTiles.Exists(t => t.Position == placeholderPosition);
                if (isVisible)
                {
                    _currentPlaceholders.Add(placeholder);
                }
            }
        }

        private void ShowHideCurrentPlaceHodlers(bool value) 
        {
            foreach(var placeholder in _currentPlaceholders) 
            {
                placeholder?.gameObject.SetActive(value);
            }
        }
    }
}

