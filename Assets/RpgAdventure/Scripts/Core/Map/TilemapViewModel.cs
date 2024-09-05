using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class TilemapViewModel : ILoadUnit
    {
        public List<TileModel> Tiles { get; private set; }

        public TileModel CurrentCentralTile { get; private set; }
        private Tilemap _tilemap;

        private DataService _dataService;//Inject

        private StructureConfig _structureConfig;

        public TilemapViewModel(Tilemap tilemap, DataService dataService, StructureConfig structureConfig) 
        {
            _tilemap = tilemap;
            _dataService = dataService;
            _structureConfig = structureConfig;
            LoadTileMapViewModel();
        }

        public void LoadTileMapViewModel() 
        {
            if (AppConstants.DEBUG_ENABLE)
            {
                if (_dataService.MapData.HasSaveData())
                {
                    LoadWorldState();
                }
                else
                {
                    InitializeTiles();
                }
            }
        }

        public async UniTask Load()
        {
            //if (_dataService.MapData.HasSaveData())
            //{
            //    LoadWorldState();
            //}
            //else
            //{
            //    InitializeTiles();
            //    await UniTask.CompletedTask;
            //}
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
                    tileData.StructureType
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
                    StructureType = tile.StructureTileType,
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
            Debug.LogError(position);
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

            var structureSpawner = new StructureSpawner(this, _structureConfig);
            structureSpawner.SpawnStructures();
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
            CurrentCentralTile = spawnTile;
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

            bool isEvenRow = CurrentCentralTile.Position.y % 2 == 0;

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
                var neighborPosition = CurrentCentralTile.Position + direction;

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
                CurrentCentralTile = tile;
            }
        }
    }
}

