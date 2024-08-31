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
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameObject placeholderPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private ClickDetector2D clickDetector;
        [SerializeField] private Vector3 _step;

        private TilemapViewModel viewModel;
        private List<GameObject> placeholders = new List<GameObject>();
        private GameObject player;

        private void Start()
        {
            viewModel = new TilemapViewModel(tilemap);
            CreatePlaceholders();
            SpawnPlayer();
            UpdateTileVisibility();
            HandleClicks();
        }

        private void CreatePlaceholders()
        {
            foreach (var tile in viewModel.Tiles.Values)
            {
                var worldPosition = tilemap.CellToWorld(tile.Position) + _step;
                var placeholder = Instantiate(placeholderPrefab, worldPosition, Quaternion.identity, transform);
                placeholder.name = $"Placeholder_{tile.Type}_{tile.Position}";
                placeholder.SetActive(false);
                placeholders.Add(placeholder);
            }
        }

        private void SpawnPlayer()
        {
            var spawnTile = viewModel.GetSpawnPlayerTile();
            if (spawnTile == null) return;

            var worldPosition = tilemap.CellToWorld(spawnTile.Position) + _step;
            player = Instantiate(playerPrefab, worldPosition, Quaternion.identity);
        }

        private void HandleClicks()
        {
            clickDetector.OnObjectClicked
                .Select(clickedObject => tilemap.WorldToCell(clickedObject.transform.position - _step))
                .Subscribe(OnTileClicked)
                .AddTo(this);
        }

        private void OnTileClicked(Vector3Int cellPosition)
        {
            if (viewModel.Tiles.TryGetValue(cellPosition, out var clickedTile) &&
                (clickedTile.Type == TileType.Land || clickedTile.Type == TileType.Sand))
            {
                MovePlayerToTile(clickedTile);
                UpdateTileVisibility();
            }
        }

        private void MovePlayerToTile(TileModel tile)
        {
            var worldPosition = tilemap.CellToWorld(tile.Position) + _step;
            player.transform.position = worldPosition;
            viewModel.SetCurrentCentralTile(tile.Position);
        }

        private void UpdateTileVisibility()
        {
            List<TileModel> surroundingTiles = viewModel.GetSurroundingTiles();

            foreach (var placeholder in placeholders)
            {
                Vector3Int placeholderPosition = tilemap.WorldToCell(placeholder.transform.position - _step);
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

