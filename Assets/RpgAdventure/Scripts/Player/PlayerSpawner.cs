using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.HexGrid 
{
    public class PlayerSpawner
    {
        private readonly TilemapViewModel _viewModel;
        private readonly Tilemap _tilemap;
        private readonly GameObject _playerPrefab;
        private readonly Vector3 _step;

        public GameObject Player { get; private set; }

        public PlayerSpawner(TilemapViewModel viewModel, Tilemap tilemap, GameObject playerPrefab, Vector3 step)
        {
            _viewModel = viewModel;
            _tilemap = tilemap;
            _playerPrefab = playerPrefab;
            _step = step;
        }

        public void RespawnPlayer() 
        {
            Object.Destroy(Player);
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            var spawnTile = _viewModel.GetSpawnPlayerTile();
            if (spawnTile == null) return;

            var worldPosition = _tilemap.CellToWorld(spawnTile.Position) + _step;
            Player = Object.Instantiate(_playerPrefab, worldPosition, Quaternion.identity);
        }

        public void MovePlayerToTile(TileModel tile)
        {
            var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
            Player.transform.position = worldPosition;
            _viewModel.SetCurrentCentralTile(tile.Position);
        }
    }
}

