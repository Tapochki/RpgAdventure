using UnityEngine;

public class MapUI : MonoBehaviour
{
    public HexMapGenerator mapGenerator;

    public void SaveMap()
    {
        string filePath = Application.persistentDataPath + "/map.json";
        mapGenerator.SaveMapToFile(filePath);
    }

    public void LoadMap()
    {
        string filePath = Application.persistentDataPath + "/map.json";
        mapGenerator.LoadMapFromFile(filePath);
    }

    public void GenerateMapFromSeed()
    {
        int seed = Random.Range(0, 10000);
        mapGenerator.GenerateMapFromSeed(seed);
    }
}
