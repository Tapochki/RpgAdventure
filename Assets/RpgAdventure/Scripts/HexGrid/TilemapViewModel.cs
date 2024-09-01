using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.HexGrid
{
    public class TilemapViewModel
    {
        public List<TileModel> Tiles { get; private set; }

        private TileModel _currentCentralTile;
        private Tilemap _tilemap;

        public TilemapViewModel(Tilemap tilemap)
        {
            _tilemap = tilemap;
            InitializeTiles();
        }

        private void InitializeTiles()
        {
            var tilesToAdd = new List<TileModel>();

            foreach (var position in _tilemap.cellBounds.allPositionsWithin)
            {
                if (!_tilemap.HasTile(position)) continue;
                Vector3Int tilePosition = new Vector3Int(position.x, position.y);
                var tileBase = _tilemap.GetTile(position);
                TileType type = DetermineTileType(tileBase);
                var tileModel = new TileModel(tilePosition, type);

                var existingTile = tilesToAdd.FirstOrDefault(t => t.Position == tilePosition);

                if (existingTile != null)
                {
                    if (position.z > existingTile.Position.z)
                    {
                        tilesToAdd.Remove(existingTile);
                        tilesToAdd.Add(tileModel);
                    }
                }
                else
                {
                    tilesToAdd.Add(tileModel);
                }
            }

            Tiles = tilesToAdd;
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
            var spawnableTiles = Tiles
                           .Where(t => t.Type == TileType.Land || t.Type == TileType.Sand)
                           .ToList();

            if (spawnableTiles.Count == 0)
            {
                Debug.LogError("No valid spawnable tiles found!");
                return null;
            }

            return spawnableTiles[Random.Range(0, spawnableTiles.Count)];
        }

        public TileModel GetTileAtPosition(Vector3Int position)
        {
            return Tiles.FirstOrDefault(t => t.Position == position);
        }

        private bool CanHighlightTile(TileModel tile)
        {
            return tile.Type != TileType.Water && tile.Type != TileType.Mountain;
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
            var tilePositionSet = new HashSet<Vector3Int>(Tiles.Select(t => t.Position));

            foreach (var direction in directions)
            {
                var neighborPosition = _currentCentralTile.Position + direction;

                if (tilePositionSet.Contains(neighborPosition))
                {
                    var tile = GetTileAtPosition(neighborPosition);
                    if (tile != null && CanHighlightTile(tile))
                    {
                        surroundingTiles.Add(tile);
                    }
                }
            }

            return surroundingTiles;
        }

        public void SetCurrentCentralTile(Vector3Int position)
        {
            var tile = GetTileAtPosition(position);
            if (tile != null)
            {
                _currentCentralTile = tile;
            }
        }
    }
}

