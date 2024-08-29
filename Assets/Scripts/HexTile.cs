using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.FindObjectOfType<HexGrid>().MovePlayerToTile(this);
    }
}