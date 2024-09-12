using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Settings;
using UnityEngine;
using VContainer;

namespace TandC.RpgAdventure.Core.Map.MapObject
{
    public class MapObjectViewModel : ILoadUnit
    {
        public Action<Vector3Int> StructureDeleteEvent;
        public Action<MapObjectModel> NewStructureCreateEvent;

        [Inject] private readonly TilemapViewModel _tilemapViewModel;
        [Inject] private readonly StructureConfig _config;

        private readonly List<MapObjectModel> _spawnedObjects = new List<MapObjectModel>();
        private readonly List<LiftTimeNarrativeObjectModel> _activeNarratives = new List<LiftTimeNarrativeObjectModel>();

        private float _spawnChance = 0.05f;

        public async UniTask Load() 
        {
            SpawnStructures();
            await UniTask.CompletedTask;
        }

        private void SpawnStructures()
        {
            CreateCity(_config.NumberOfCities);
            CreateMapObject(StructureTileType.Volcano, _config.NumberOfVolcano);
            CreateNarrativeEvents();
        }

        private void CreateNarrativeEvents()
        {
            CreateNarrative(StructureTileType.UniqueQuestionMark, _config.NumberOfUniqueEvent);
            CreateNarrative(StructureTileType.GoldQuestionMark, _config.NumberOfGoldEvent);
        }

        private void CreateMapObject(StructureTileType structureType, int count) 
        {
            for (int i = 0; i < count; i++)
            {
                TileModel selectedTile = GetRandomEligibleTile();
                if (selectedTile == null) continue;

                CreateMapObject(structureType, selectedTile);
            }
        }

        private StructureObjectModel CreateMapObject(StructureTileType structureType, TileModel selectedTile)
        {
            var mapObject = new StructureObjectModel(structureType, selectedTile.Position, _config.MinDistanceBetweenStructures);
            AddSpawnedObject(mapObject, selectedTile);
            return mapObject;
        }

        private void CreateCity(int count)
        {
            for (int i = 0; i < count; i++)
            {
                TileModel selectedTile = GetRandomEligibleTile();
                if (selectedTile == null) continue;

                CreateCity(selectedTile);
            }
        }

        private CityObjectModel CreateCity(TileModel selectedTile)
        {
            var city = new CityObjectModel("CityName", selectedTile.Position, _config.MinDistanceBetweenStructures);
            AddSpawnedObject(city, selectedTile);
            return city;
        }

        private void CreateNarrative(StructureTileType structureType, int count)
        {
            for (int i = 0; i < count; i++)
            {
                TileModel selectedTile = GetRandomEligibleTile();
                if (selectedTile == null) continue;

                CreateNarrative(structureType, selectedTile, "Narrative");
            }
        }

        private NarrativeObjectModel CreateNarrative(StructureTileType structureType, TileModel tileModel, string description, int minSpawnDistance = 1) 
        {
            var narrativeEvent = new NarrativeObjectModel(structureType, tileModel.Position, description, minSpawnDistance);
            AddSpawnedObject(narrativeEvent, tileModel);
            return narrativeEvent;
        }

        private LiftTimeNarrativeObjectModel CreateLifeTimeNarrative(StructureTileType structureType, TileModel tileModel, string description, int lifeTime = 10, int minSpawnDistance = 1)
        {
            var narrativeEvent = new LiftTimeNarrativeObjectModel(structureType, tileModel.Position, description, minSpawnDistance, lifeTime);
            _activeNarratives.Add(narrativeEvent);

            AddSpawnedObject(narrativeEvent, tileModel);
            return narrativeEvent;
        }

        public void CreateRandomMarker(StructureTileType type, Vector3Int? originPosition = null, int radius = 1)
        {
            TileModel selectedTile = (originPosition.HasValue && radius > 0)
                ? _tilemapViewModel.GetRandomTilesInRadius(originPosition.Value, radius)
                : _tilemapViewModel.GetRandomTile();

            if (selectedTile != null)
            {
                CreateNarrative(type, selectedTile, "New Narrative", 1);
            }
            else
            {
                Debug.LogWarning("Failed to place a random marker, no eligible tiles found.");
            }
        }

        public void TryCreateNarrativeMark(List<TileModel> openedPosition)
        {
            var eligibleTiles = openedPosition.Where(tile => !IsTooCloseToOtherStructures(tile.Position)).ToList();

            if (eligibleTiles.Count == 0)
            {
                IncreaseSpawnChance(0.01f);
                Debug.LogWarning("No eligible tiles for narrative mark creation.");
                return;
            }

            float chance = UnityEngine.Random.Range(0f, 1f);
            Debug.LogError($"Roll: {chance} MinimalChange: {_spawnChance}");
            if (chance <= _spawnChance)
            {
                var randomTile = openedPosition[UnityEngine.Random.Range(0, eligibleTiles.Count)];
                CreateLifeTimeNarrative(StructureTileType.DefaultQuestionMark, randomTile, "Default", 20, 1);
                SetStartSpawnChance();
            }
            else
            {
                IncreaseSpawnChance();
            }
        }

        private void SetStartSpawnChance() 
        {
            _spawnChance = 0.05f;
        }

        private void IncreaseSpawnChance(float value = 0.05f) 
        {
            _spawnChance += value;
        }

        private void AddSpawnedObject(MapObjectModel newMapObject, TileModel tileModel)
        {
            _spawnedObjects.Add(newMapObject);
            tileModel.SetMapObject(newMapObject);
            NewStructureCreateEvent?.Invoke(newMapObject);
        }

        private TileModel GetRandomEligibleTile()
        {
            var eligibleTiles = _tilemapViewModel.GetAccessibleTiles();

            for (int attempt = 0; attempt < 100; attempt++)
            {
                var randomTile = eligibleTiles[UnityEngine.Random.Range(0, eligibleTiles.Count)];

                if (IsTooCloseToOtherStructures(randomTile.Position)) continue;

                return randomTile;
            }

            return null;
        }

        private bool IsTooCloseToOtherStructures(Vector3Int tilePosition)
        {
            foreach (var spawnedObject in _spawnedObjects)
            {
                if (Vector3Int.Distance(tilePosition, spawnedObject.Position) < spawnedObject.MinSpawnDistance)
                {
                    return true;
                }
            }
            return false;
        }

        public List<MapObjectModel> GetSpawnedObjects()
        {
            return _spawnedObjects;
        }

        public void UpdateNarratives()
        {
            foreach (var narrative in _activeNarratives)
            {
                narrative.DecreaseLifetime();
                if (narrative.IsExpired())
                {
                    var tile = _tilemapViewModel.GetTileAtPosition(narrative.Position);
                    tile.ClearMapObject();
                    StructureDeleteEvent?.Invoke(narrative.Position);
                }
            }

            for (int i = 0; i < _activeNarratives.Count; i++)
            {
                _activeNarratives[i].DecreaseLifetime();

                if (_activeNarratives[i].IsExpired())
                {
                    var tile = _tilemapViewModel.GetTileAtPosition(_activeNarratives[i].Position);
                    tile.ClearMapObject();
                    StructureDeleteEvent?.Invoke(_activeNarratives[i].Position);

                    _activeNarratives.RemoveAt(i);
                }
            }
        }
    }
}

