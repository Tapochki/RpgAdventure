using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates; // To store the coordinates of the cell
    public bool isActive; // Example property to interact with

    // Method to initialize the cell
    public void Initialize(Vector2Int coords, bool active)
    {
        coordinates = coords;
        isActive = active;
    }

    // Method for interaction with the cell
    public void ToggleActive()
    {
        isActive = !isActive;
        // Update the visual state of the cell here
    }

    // Method to save cell data (for example)
    public string SaveData()
    {
        return $"{coordinates.x},{coordinates.y},{isActive}";
    }
}
