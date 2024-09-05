using TandC.RpgAdventure.Core.HexGrid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.Core.Player
{
    public class PlayerSpawner
    {
        private readonly TilemapViewModel _viewModel;
        private readonly Tilemap _tilemap;
        private readonly GameObject _playerPrefab;
        private readonly Vector3 _step;
        private readonly FogOfWar _fogOfWar;

        public GameObject Player { get; private set; }

        public PlayerSpawner(TilemapViewModel viewModel, Tilemap tilemap, GameObject playerPrefab, Vector3 step, FogOfWar fogOfWar)
        {
            _viewModel = viewModel;
            _tilemap = tilemap;
            _playerPrefab = playerPrefab;
            _step = step;
            _fogOfWar = fogOfWar;
            _fogOfWar.InitializeFog();
        }

        public void RespawnPlayer() 
        {
            Object.Destroy(Player);
            SpawnPlayer();
        }

        public void SpawnPlayer(TileModel tileModel = null)
        {
            bool isFirstCreate = false;
            if (tileModel == null)
            {
                tileModel = _viewModel.GetSpawnPlayerTile();
                isFirstCreate = true;
            }

            if (tileModel != null)
            {
                CreatePlayer(tileModel, isFirstCreate);
            }
        }

        public void CreatePlayer(TileModel tileModel, bool isFirstCreate) 
        {
            var worldPosition = _tilemap.CellToWorld(tileModel.Position) + _step;
            Player = Object.Instantiate(_playerPrefab, worldPosition, Quaternion.identity);
            if (isFirstCreate) 
            {
                _fogOfWar.UpdateFog(tileModel.Position);
            }
        }

        public void MovePlayerToTile(TileModel tile)
        {
            var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
            Player.transform.position = worldPosition;
            _viewModel.SetCurrentCentralTile(tile.Position);
            _fogOfWar.UpdateFog(tile.Position);
        }
    }
}

