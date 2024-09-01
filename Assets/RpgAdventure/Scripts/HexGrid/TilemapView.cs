using System.Collections.Generic;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.HexGrid 
{
    public class TilemapView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private GameObject _placeholderPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private ClickDetector2D _clickDetector;
        [SerializeField] private Vector3 _step;

        [SerializeField] private FogOfWar _fogOfWar;

        private TilemapViewModel _viewModel;
        private List<GameObject> _placeholders = new List<GameObject>();
        private PlayerSpawner _playerSpawner;

        private void Start()
        {
            _viewModel = new TilemapViewModel(_tilemap);
            _playerSpawner = new PlayerSpawner(_viewModel, _tilemap, _playerPrefab, _step, _fogOfWar);

            CreatePlaceholders();
            _playerSpawner.SpawnPlayer();
            UpdateTileVisibility();
            HandleClicks();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.V)) 
            {
                _playerSpawner.RespawnPlayer();
            }
        }

        private void CreatePlaceholders()
        {
            foreach (var tile in _viewModel.Tiles)
            {
                var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
                var placeholder = Instantiate(_placeholderPrefab, worldPosition, Quaternion.identity, transform);
                placeholder.name = $"Placeholder_{tile.Type}_{tile.Position}";
                placeholder.SetActive(false);
                _placeholders.Add(placeholder);
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
            if (placeholder != null)
            {
                placeholder.SetActive(visible);
            }
        }
    }
}

