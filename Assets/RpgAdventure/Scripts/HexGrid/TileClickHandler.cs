using System.Collections.Generic;
using TandC.RpgAdventure.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.HexGrid 
{
    public class TileClickHandler
    {
        private readonly TilemapViewModel _viewModel;
        private readonly Tilemap _tilemap;
        private readonly PlayerSpawner _playerSpawner;

        public TileClickHandler(TilemapViewModel viewModel, Tilemap tilemap, PlayerSpawner playerSpawner)
        {
            _viewModel = viewModel;
            _tilemap = tilemap;
            _playerSpawner = playerSpawner;
        }

        public void HandleTileClick(Vector3 clickPosition)
        {
            var cellPosition = _tilemap.WorldToCell(clickPosition);
            if (_viewModel.Tiles.TryGetValue(cellPosition, out var clickedTile) &&
                (clickedTile.Type == TileType.Land || clickedTile.Type == TileType.Sand))
            {
                _playerSpawner.MovePlayerToTile(clickedTile);
                UpdateTileVisibility();
            }
        }

        private void UpdateTileVisibility()
        {
            List<TileModel> surroundingTiles = _viewModel.GetSurroundingTiles();
        }
    }
}

