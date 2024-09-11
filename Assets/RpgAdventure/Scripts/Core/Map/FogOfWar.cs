using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Map
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private Tilemap _fogTilemap;
        [SerializeField] private AnimatedTile _fogTile;

        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private int _playerVision;

        [Inject] private TilemapViewModel _tilemapViewModel;

        public void SetTileViewModel(TilemapViewModel tilemapViewModel)
        {
            _tilemapViewModel = tilemapViewModel;
        }

        public void InitializeFog()
        {
            _fogTilemap.ClearAllTiles();
            foreach (var position in _tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int fogTilePosition = new Vector3Int(position.x, position.y);
                if (_tilemap.HasTile(position))
                {
                    _fogTilemap.SetTile(fogTilePosition, _fogTile);
                }
            }
        }
        //public void UpdateFog(Vector3Int playerPosition) Old
        //{
        //    for (int i = -_playerVision; i <= _playerVision; i++)
        //    {
        //        for (int j = -_playerVision; j <= _playerVision; j++)
        //        {
        //            Vector3Int openedPosition = playerPosition + new Vector3Int(i, j);
        //            _fogTilemap.SetTile(openedPosition, null);
        //            _tilemapViewModel.SetTileOpen(openedPosition);
        //        }
        //    }
        //}

        public void UpdateFog(Vector3Int playerPosition)
        {
            if(_playerVision == 0) 
            {
                _fogTilemap.SetTile(playerPosition, null);
                _tilemapViewModel.SetTileOpen(playerPosition);
                return;
            }
            List<Vector3Int> directions = new List<Vector3Int>() { new Vector3Int(0, 0) };

            directions.AddRange(_tilemapViewModel.GetDirections());

            for (int i = 0; i < directions.Count; i++)
            {
                Vector3Int openedPosition = playerPosition + directions[i];
                _fogTilemap.SetTile(openedPosition, null);
                _tilemapViewModel.SetTileOpen(openedPosition);
            }
        }

        public void OpenFogPosition(Vector3Int tilePosition)
        {
            _fogTilemap.SetTile(tilePosition, null);
        }
    }
}

