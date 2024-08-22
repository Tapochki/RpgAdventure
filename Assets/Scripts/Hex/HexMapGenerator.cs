using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    //TODO move to config
    public int mapWidth = 50;
    public int mapHeight = 50;
    public float hexSize = 1f;
    public float noiseScale = 0.1f;
    public float waterThreshold = 0.3f;
    public float landThreshold = 0.6f;
    public float forestThreshold = 0.8f;
    public int Seed;
    public bool IsRiver;

    public GameObject waterPrefab;
    public GameObject landPrefab;
    public GameObject forestPrefab;
    public GameObject mountainPrefab;

    private Dictionary<Vector2Int, HexTile> hexTiles;


    [ContextMenu("Generate Random Map")]
    public void GenerateRandomMap()
    {
        Seed = Random.Range(0, 100000);
        GenerateMap(Seed);
    }

    [ContextMenu("Generate Map from Seed")]
    public void GenerateMapFromSeed()
    {
        GenerateMap(Seed);
    }

    [ContextMenu("Save Map")]
    public void SaveMapMenu()
    {
        string filePath = Application.persistentDataPath + "/map.json";
        SaveMapToFile(filePath);
    }

    [ContextMenu("Load Map")]
    public void LoadMapMenu()
    {
        string filePath = Application.persistentDataPath + "/map.json";
        LoadMapFromFile(filePath);
    }

    [ContextMenu("Clear Map")]
    public void ClearMapMenu()
    {
        ClearMap();
    }

    private void GenerateMap(int seed)
    {
        ClearMap();
        hexTiles = new Dictionary<Vector2Int, HexTile>();

        Random.InitState(seed);
        float offsetX = Random.Range(0f, 100f);
        float offsetY = Random.Range(0f, 100f);

        for (int r = 0; r < mapHeight; r++)
        {
            for (int q = 0; q < mapWidth; q++)
            {
                Vector3 hexPosition = CalculateHexPosition(q, r);
                HexTileType tileType = DetermineTileType(q, r, offsetX, offsetY);
                GameObject hexGO = Instantiate(GetPrefabForTileType(tileType), hexPosition, Quaternion.identity);
                hexGO.transform.SetParent(transform);
                hexGO.name = $"Hex_{q}_{r}";

                HexTile hexTile = new HexTile(q, r, hexPosition, tileType);
                hexTiles[new Vector2Int(q, r)] = hexTile;
            }
        }
    }

    private void ClearMap()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("No tiles to clear.");
            return;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }

        }
        hexTiles.Clear();
        Debug.Log("Map cleared.");
    }

    private Vector3 CalculateHexPosition(int q, int r)
    {
        float width = Mathf.Sqrt(3) * hexSize;
        float height = 2 * hexSize;
        float verticalSpacing = height * 0.75f;
        float horizontalSpacing = width;

        float x = q * horizontalSpacing;
        float y = r * verticalSpacing;

        if (r % 2 != 0)
        {
            x += horizontalSpacing / 2f;
        }

        return new Vector3(x, y, 0);
    }

    private HexTileType DetermineTileType(int q, int r, float offsetX, float offsetY)
    {
        float noiseValue = Mathf.PerlinNoise((q * noiseScale) + offsetX, (r * noiseScale) + offsetY);

        if (noiseValue < waterThreshold)
        {
            return HexTileType.Water;
        }
        else if (noiseValue < landThreshold)
        {
            return HexTileType.Land;
        }
        else if (noiseValue < forestThreshold)
        {
            return HexTileType.Forest;
        }
        else
        {
            return HexTileType.Mountain;
        }
    }

    private GameObject GetPrefabForTileType(HexTileType tileType)
    {
        switch (tileType)
        {
            case HexTileType.Water:
                return waterPrefab;
            case HexTileType.Land:
                return landPrefab;
            case HexTileType.Forest:
                return forestPrefab;
            case HexTileType.Mountain:
                return mountainPrefab;
            default:
                return landPrefab;
        }
    }

    public void GenerateMapFromSeed(int seed)
    {
        this.Seed = seed;
        GenerateMap(seed);
    }

    public void SaveMapToFile(string filePath)
    {
        HexMapData mapData = new HexMapData
        {
            mapWidth = mapWidth,
            mapHeight = mapHeight,
            seed = Seed,
            tiles = new List<HexTileData>()
        };

        foreach (var hexTile in hexTiles.Values)
        {
            mapData.tiles.Add(new HexTileData(hexTile.Q, hexTile.R, hexTile.TileType));
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Map saved to {filePath}");
    }

    public void LoadMapFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HexMapData mapData = JsonUtility.FromJson<HexMapData>(json);

            ClearMap();

            mapWidth = mapData.mapWidth;
            mapHeight = mapData.mapHeight;
            Seed = mapData.seed;

            hexTiles = new Dictionary<Vector2Int, HexTile>();

            foreach (var tileData in mapData.tiles)
            {
                Vector3 hexPosition = CalculateHexPosition(tileData.q, tileData.r);
                GameObject hexGO = Instantiate(GetPrefabForTileType(tileData.tileType), hexPosition, Quaternion.identity, transform);
                hexGO.name = $"Hex_{tileData.q}_{tileData.r}";

                HexTile hexTile = new HexTile(tileData.q, tileData.r, hexPosition, tileData.tileType);
                hexTiles[new Vector2Int(tileData.q, tileData.r)] = hexTile;
            }

            Debug.Log($"Map loaded from {filePath}");
        }
        else
        {
            Debug.LogError($"File {filePath} does not exist.");
        }
    }
}