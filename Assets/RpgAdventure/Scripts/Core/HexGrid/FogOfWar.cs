using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private Tilemap _fogTilemap;
        [SerializeField] private AnimatedTile _fogTile;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilemapViewModel _viewModel;

        [SerializeField] private int _fogStep;

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
            Debug.LogError(playerPosition);
            for (int i = -_fogStep; i <= _fogStep; i++)
            {
                for (int j = -_fogStep; j <= _fogStep; j++)
                {
                    _fogTilemap.SetTile(playerPosition + new Vector3Int(i, j), null);
                }
            }
        }
    }
}

