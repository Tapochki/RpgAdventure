using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.HexGrid
{
    public class TilemapViewModel
    {
        public IReadOnlyDictionary<Vector3Int, TileModel> Tiles { get; private set; }

        private TileModel _currentCentralTile;
        private Tilemap tilemap;

        public TilemapViewModel(Tilemap tilemap)
        {
            this.tilemap = tilemap;
            InitializeTiles();
        }

        private void InitializeTiles()
        {
            var tilesDictionary = new Dictionary<Vector3Int, TileModel>();

            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(position)) continue;

                var tileBase = tilemap.GetTile(position);
                TileType type = DetermineTileType(tileBase);
                var tileModel = new TileModel(position, type);
                tilesDictionary.Add(position, tileModel);
            }

            Tiles = tilesDictionary;
        }

        private TileType DetermineTileType(TileBase tile)
        {
            if (tile.name.Contains("Mountain"))
                return TileType.Mountain;
            if (tile.name.Contains("Sand"))
                return TileType.Sand;
            if (tile.name.Contains("Water"))
                return TileType.Water;

            return TileType.Land;
        }

        public TileModel GetSpawnPlayerTile()
        {
            TileModel spawnTile = GetRandomSpawnableTile();
            _currentCentralTile = spawnTile;
            return spawnTile;
        }

        private TileModel GetRandomSpawnableTile()
        {
            var spawnableTiles = Tiles.Values
                .Where(t => t.Type == TileType.Land || t.Type == TileType.Sand)
                .Where(t => !HasMountainTileAbove(t.Position))
                .ToList();

            if (spawnableTiles.Count == 0)
            {
                Debug.LogError("No valid spawnable tiles found!");
                return null;
            }

            return spawnableTiles[Random.Range(0, spawnableTiles.Count)];
        }

        private bool HasMountainTileAbove(Vector3Int position)
        {
            var tileAbove = tilemap.GetTile(position);
            return tileAbove != null && DetermineTileType(tileAbove) == TileType.Mountain;
        }

        public List<TileModel> GetSurroundingTiles()
        {
            Vector3Int[] directions = new Vector3Int[]
            {
            new Vector3Int(1, 0, 0),   // Top
            new Vector3Int(-1, 0, 0),  // Bottom
            new Vector3Int(0, -1, 0),  // Top-Left (even Y), Bottom-Left (odd Y)
            new Vector3Int(0, 1, 0),   // Top-Right (even Y), Bottom-Right (odd Y)
            new Vector3Int(-1, -1, 0), // Bottom-Right (even Y)
            new Vector3Int(-1, 1, 0)   // Bottom-Left (even Y)
            };

            bool isEvenRow = _currentCentralTile.Position.y % 2 == 0;

            if (!isEvenRow)
            {
                directions[2] += new Vector3Int(1, 0, 0);  // Top-Left becomes Top-Right
                directions[3] += new Vector3Int(1, 0, 0);  // Top-Right becomes Bottom-Right
                directions[4] += new Vector3Int(1, 0, 0);  // Bottom-Right becomes Top-Left
                directions[5] += new Vector3Int(1, 0, 0);  // Bottom-Left becomes Bottom-Right
            }

            var surroundingTiles = new List<TileModel>();

            foreach (var direction in directions)
            {
                var neighborPosition = _currentCentralTile.Position + direction;
                if (Tiles.TryGetValue(neighborPosition, out var tile))
                {
                    surroundingTiles.Add(tile);
                }
            }

            return surroundingTiles;
        }

        public void SetCurrentCentralTile(Vector3Int position)
        {
            if (Tiles.TryGetValue(position, out var tile))
            {
                _currentCentralTile = tile;
            }
        }
    }
}

