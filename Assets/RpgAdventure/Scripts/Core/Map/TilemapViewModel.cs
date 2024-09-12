using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Map
{
    public class TilemapViewModel : ILoadUnit
    {
        public List<TileModel> Tiles { get; private set; }

        public TileModel CurrentCentralTile { get; private set; }
        private Tilemap _tilemap;

        [Inject] private DataService _dataService;

        private StructureConfig _structureConfig;

        public TilemapViewModel(StructureConfig structureConfig) 
        {
            _structureConfig = structureConfig;
        }

        public void SetTilemap(Tilemap tilemap) 
        {
            _tilemap = tilemap;
        }

        public async UniTask Load()
        {
            if (_dataService.MapData.HasSaveData())
            {
                LoadWorldState();
            }
            else
            {
                InitializeTiles();
            }
            await UniTask.CompletedTask;
        }

        public void LoadWorldState()
        {
            Debug.LogError("Start World Load");
            MapData mapData = _dataService.MapData;

            var loadedTiles = new List<TileModel>();

            foreach (var tileData in mapData.Tiles)
            {
                var tile = new TileModel(
                    tileData.Position,
                    tileData.Type,
                    tileData.IsOpen,
                    tileData.MapObject
                );
                loadedTiles.Add(tile);
            }

            Tiles = loadedTiles;

            SetCurrentCentralTile(mapData.PlayerPosition);
        }

        public void SaveWorldState()
        {
            MapData mapData = _dataService.GetDefaultMapData();
            foreach (var tile in Tiles)
            {
                var tileData = new TileSaveData
                {
                    Position = tile.Position,
                    Type = tile.Type,
                    MapObject = tile.MapObject,
                    IsOpen = tile.IsOpen
                };
                mapData.Tiles.Add(tileData);
            }
            mapData.PlayerPosition = CurrentCentralTile.Position;
            mapData.LevelId = 1;

            _dataService.SaveCache(CacheType.MapData);
        }

        public void SetTileOpen(Vector3Int position)
        {
            TileModel tile = GetTileAtPosition(position);
            if(tile != null)
            {
                tile.SetTileOpen();
            }
            else 
            {
                Debug.LogError($"Error at position {position}");
            }

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
                var tileModel = new TileModel(tilePosition, type, false);

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
            TileModel spawnTile = GetRandomTile();
            CurrentCentralTile = spawnTile;
            return spawnTile;
        }

        public List<TileModel> GetAccessibleTiles() 
        {
            return Tiles.Where(t => t.Type == TileType.Land || t.Type == TileType.Sand).ToList();
        }

        public TileModel GetRandomTile()
        {
            var spawnableTiles = GetAccessibleTiles();

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

        public List<TileModel> GetSurroundingTilesFromCentral()
        {
            return GetSurroundingTilesFromPosition(CurrentCentralTile.Position);
        }

        public List<TileModel> GetSurroundingTilesFromSelectedPosition(Vector3Int position)
        {
            return GetSurroundingTilesFromPosition(position);
        }

        private List<TileModel> GetSurroundingTilesFromPosition(Vector3Int position)
        {
            Vector3Int[] directions = GetDirectionsFromPosition(position);
            var surroundingTiles = new List<TileModel>();
            var tilePositionSet = new HashSet<Vector3Int>(Tiles.Select(t => t.Position));

            foreach (var direction in directions)
            {
                var neighborPosition = position + direction;

                if (tilePositionSet.Contains(neighborPosition))
                {
                    var tile = GetTileAtPosition(neighborPosition);
                    if (tile != null && CanHighlightTile(tile))
                    {
                        surroundingTiles.Add(tile);
                    }
                }
            }

            return surroundingTiles.Distinct().ToList();
        }

        private Vector3Int[] GetDirectionsFromCenttal()
        {
            return GetDirectionsFromPosition(CurrentCentralTile.Position);
        }

        public Vector3Int[] GetDirectionsFromSelectedPosition(Vector3Int position)
        {
            return GetDirectionsFromPosition(position);
        }

        public List<Vector3Int> GetFirstCircle(Vector3Int centerPosition)
        {
            Vector3Int[] directions = GetDirectionsFromPosition(centerPosition);

            var firstCircle = new List<Vector3Int>();

            foreach (var direction in directions)
            {
                firstCircle.Add(centerPosition + direction);
            }

            return firstCircle;
        }

        public List<Vector3Int> GetSecondCircle(Vector3Int centerPosition)
        {
            var firstCircle = GetFirstCircle(centerPosition);

            var secondCircle = new List<Vector3Int>();

            foreach (var position in firstCircle)
            {
                secondCircle.AddRange(GetFirstCircle(position));
            }

            return secondCircle.Distinct().ToList();
        }

        private Vector3Int[] GetDirectionsFromPosition(Vector3Int position)
        {
            Vector3Int[] directions = new Vector3Int[]
            {
        new Vector3Int(1, 0, 0),   // Right
        new Vector3Int(-1, 0, 0),  // Left
        new Vector3Int(0, -1, 0),  // Down
        new Vector3Int(0, 1, 0),   // Up
        new Vector3Int(-1, -1, 0), // Bottom-Left
        new Vector3Int(-1, 1, 0)   // Top-Left
            };

            bool isEvenRow = position.y % 2 == 0;

            if (!isEvenRow)
            {
                directions[2] += new Vector3Int(1, 0, 0);  // Down-Left становится Down-Right
                directions[3] += new Vector3Int(1, 0, 0);  // Up становится Up-Right
                directions[4] += new Vector3Int(1, 0, 0);  // Bottom-Left становится Bottom-Right
                directions[5] += new Vector3Int(1, 0, 0);  // Top-Left становится Top-Right
            }

            return directions;
        }

        public void SetCurrentCentralTile(Vector3Int position)
        {
            var tile = GetTileAtPosition(position);
            if (tile != null)
            {
                CurrentCentralTile = tile;
            }
        }

        public int GetHexDistance(Vector3Int a, Vector3Int b)
        {
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
        }

        public TileModel GetRandomTilesInRadius(Vector3Int center, int radius)
        {
            var radiusTile = GetTilesInRadius(center, radius);

            if (radiusTile.Count == 0)
            {
                Debug.LogError("No valid spawnable tiles found!");
                return null;
            }

            return radiusTile[Random.Range(0, radiusTile.Count)];
        }

        private List<TileModel> GetTilesInRadius(Vector3Int center, int radius)
        {
            return GetAccessibleTiles().Where(tile => GetHexDistance(center, tile.Position) <= radius).ToList();
        }

        public List<TileModel> GetTilesByPositions(List<Vector3Int> positions)
        {
            var tiles = new List<TileModel>();

            foreach (var position in positions)
            {
                var tile = GetTileAtPosition(position);
                if (tile != null)
                {
                    tiles.Add(tile);
                }
            }

            return tiles;
        }
    }
}

