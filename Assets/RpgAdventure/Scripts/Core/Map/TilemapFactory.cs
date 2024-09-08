
using UnityEngine.Tilemaps;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Map 
{
    public class TilemapFactory
    {
        private readonly GameObject _tilemapPrefab;
        private readonly Grid _grid;

        public TilemapFactory(GameObject tilemapPrefab, Grid grid)
        {
            _tilemapPrefab = tilemapPrefab;
            _grid = grid;
        }

        public Tilemap CreateTilemap(Tilemap tilemap)
        {
            if (tilemap == null)
            {
                Debug.LogError("Tilemap is null, cannot create Tilemap instance.");
                return null;
            }

            var tilemapInstance = GameObject.Instantiate(_tilemapPrefab, _grid.transform);
            var tilemapComponent = tilemapInstance.GetComponent<Tilemap>();

            return tilemapComponent;
        }

    }
}

