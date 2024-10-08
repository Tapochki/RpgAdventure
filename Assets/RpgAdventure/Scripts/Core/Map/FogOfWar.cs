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

        public void UpdateFog(Vector3Int playerPosition)
        {
            OpenTile(playerPosition);
            if (_playerVision == 0)
            {
                return;
            }
            if (_playerVision == 1)
            {
                var firstCircle = _tilemapViewModel.GetFirstCircle(playerPosition);
                foreach (var position in firstCircle)
                {
                    OpenTile(position);
                }
            }
            if (_playerVision == 2)
            {
                var secondCircle = _tilemapViewModel.GetSecondCircle(playerPosition);
                foreach (var position in secondCircle)
                {
                    OpenTile(position);
                }

            }
        }

        private void OpenTile(Vector3Int position)
        {
            _fogTilemap.SetTile(position, null);
            _tilemapViewModel.SetTileOpen(position);
        }

        public void OpenFogPosition(Vector3Int tilePosition)
        {
            _fogTilemap.SetTile(tilePosition, null);
        }
    }
}

