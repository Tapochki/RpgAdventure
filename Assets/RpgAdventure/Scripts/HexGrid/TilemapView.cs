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

        private TilemapViewModel viewModel;
        private List<GameObject> placeholders = new List<GameObject>();
        private PlayerSpawner playerSpawner;

        private void Start()
        {
            viewModel = new TilemapViewModel(_tilemap);
            playerSpawner = new PlayerSpawner(viewModel, _tilemap, _playerPrefab, _step);

            CreatePlaceholders();
            playerSpawner.SpawnPlayer();
            UpdateTileVisibility();
            HandleClicks();
        }

        private void CreatePlaceholders()
        {
            foreach (var tile in viewModel.Tiles)
            {
                var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
                var placeholder = Instantiate(_placeholderPrefab, worldPosition, Quaternion.identity, transform);
                placeholder.name = $"Placeholder_{tile.Type}_{tile.Position}";
                placeholder.SetActive(false);
                placeholders.Add(placeholder);
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
            var clickedTile = viewModel.Tiles.Find(t => t.Position == cellPosition);

            if (clickedTile != null &&
                (clickedTile.Type == TileType.Land || clickedTile.Type == TileType.Sand))
            {
                playerSpawner.MovePlayerToTile(clickedTile);
                UpdateTileVisibility();
            }
        }

        public void UpdateTileVisibility()
        {
            List<TileModel> surroundingTiles = viewModel.GetSurroundingTiles();

            foreach (var placeholder in placeholders)
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

