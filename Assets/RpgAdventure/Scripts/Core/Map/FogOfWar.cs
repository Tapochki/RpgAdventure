using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private Tilemap _fogTilemap;
        [SerializeField] private Tile _fogTile;

        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private int _fogStep;

        [SerializeField] private Vector3Int _tilePosition;

        private TilemapViewModel _tilemapViewModel;

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

        public void UpdateFog(Vector3Int playerPosition)
        {
            for (int i = -_fogStep; i <= _fogStep; i++)
            {
                for(int j = -_fogStep; j <= _fogStep; j++) 
                {
                    Vector3Int openedPosition = playerPosition + new Vector3Int(i, j);
                    _fogTilemap.SetTile(openedPosition, null);
                    _tilemapViewModel.SetTileOpen(openedPosition);
                }
            }
        }

        public void OpenFogPosition(Vector3Int tilePosition) 
        {
            Debug.LogError($"{tilePosition} {_fogTilemap.GetTile(tilePosition)}");
            _fogTilemap.SetTile(tilePosition, null);
        }
    }
}

