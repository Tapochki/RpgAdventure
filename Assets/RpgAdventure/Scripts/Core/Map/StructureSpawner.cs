using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Core.HexGrid;
using TandC.RpgAdventure.Settings;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    private readonly TilemapViewModel _tilemapViewModel;
    private readonly StructureConfig _config;
    private readonly List<Vector3Int> _spawnedPositions = new List<Vector3Int>();

    public StructureSpawner(TilemapViewModel tilemapViewModel, StructureConfig config)
    {
        _tilemapViewModel = tilemapViewModel;
        _config = config;
    }

    public void SpawnStructures()
    {
        SpawnStructure(StructureTileType.City, _config.NumberOfCities);

        SpawnStructure(StructureTileType.Village, _config.NumberOfVillages);

        SpawnStructure(StructureTileType.Tavern, _config.NumberOfTaverns);

        SpawnStructure(StructureTileType.Cave, _config.NumberOfCaves);

        SpawnStructure(StructureTileType.Portal, _config.NumberOfCaves);
    }

    private void SpawnStructure(StructureTileType structureType, int count)
    {
        var eligibleTiles = _tilemapViewModel.Tiles
            .Where(t => t.Type == TileType.Land || t.Type == TileType.Sand)
            .ToList();

        for (int i = 0; i < count; i++)
        {
            if (eligibleTiles.Count == 0) break;

            TileModel selectedTile = null;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                var randomTile = eligibleTiles[Random.Range(0, eligibleTiles.Count)];

                if (IsTooCloseToOtherStructures(randomTile.Position)) continue;

                selectedTile = randomTile;
                break;
            }

            if (selectedTile == null)
            {
                Debug.LogWarning($"Failed to place structure {structureType}, not enough space!");
                continue;
            }

            _spawnedPositions.Add(selectedTile.Position);

            selectedTile.ChengeStructureType(structureType);
        }
    }

    private bool IsTooCloseToOtherStructures(Vector3Int tilePosition)
    {
        foreach (var spawnedPosition in _spawnedPositions)
        {
            if (Vector3Int.Distance(tilePosition, spawnedPosition) < _config.MinDistanceBetweenStructures)
            {
                return true;
            }
        }
        return false;
    }
}
