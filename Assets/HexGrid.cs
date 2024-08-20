using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab; // Assign the HexCell prefab in the inspector
    public int gridWidth = 5; // Default width
    public int gridHeight = 5; // Default height

    public float xOffset = 0.866f; // Default x offset (cos(30 degrees))
    public float yOffset = 1.5f;   // Default y offset (height of the hexagon)

    private Dictionary<Vector2Int, HexCell> hexCells = new Dictionary<Vector2Int, HexCell>();


    private void Start()
    {
        CreateGrid(gridWidth, gridHeight);
    }

    // Method to create the initial grid
    public void CreateGrid(int width, int height)
    {
        gridWidth = width;
        gridHeight = height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                CreateCell(coords);
            }
        }
    }

    // Method to create a single cell
    private void CreateCell(Vector2Int coords)
    {
        Vector3 position = CalculatePosition(coords);
        GameObject hexCellObject = Instantiate(hexPrefab, position, Quaternion.identity, transform);
        HexCell hexCell = hexCellObject.GetComponent<HexCell>();
        hexCell.Initialize(coords, false);
        hexCells.Add(coords, hexCell);
    }

    // Calculate the world position of the cell
    private Vector3 CalculatePosition(Vector2Int coords)
    {
        float x = coords.x * xOffset;
        float y = (coords.y * yOffset) + (coords.x % 2 == 0 ? 0 : yOffset / 2);

        return new Vector3(x, y, 0);
    }

    // Method to expand the grid in a specific direction
    public void ExpandGrid(Vector2Int direction)
    {
        List<Vector2Int> newCoords = new List<Vector2Int>();

        foreach (var cell in hexCells.Values)
        {
            Vector2Int newCoord = cell.coordinates + direction;
            if (!hexCells.ContainsKey(newCoord))
            {
                newCoords.Add(newCoord);
            }
        }

        foreach (var coords in newCoords)
        {
            CreateCell(coords);
        }
    }

    // Load grid data from a file
    public void LoadGridFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            bool isActive = bool.Parse(parts[2]);

            Vector2Int coords = new Vector2Int(x, y);
            if (hexCells.ContainsKey(coords))
            {
                hexCells[coords].Initialize(coords, isActive);
            }
            else
            {
                CreateCell(coords);
                hexCells[coords].Initialize(coords, isActive);
            }
        }
    }

    // Save grid data to a file
    public void SaveGridToFile(string filePath)
    {
        List<string> lines = new List<string>();
        foreach (var cell in hexCells.Values)
        {
            lines.Add(cell.SaveData());
        }
        File.WriteAllLines(filePath, lines);
    }
}
