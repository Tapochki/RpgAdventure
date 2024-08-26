using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _player;

    private Vector3Int _previousPlayerCell;

    private void Start()
    {
        _previousPlayerCell = _tilemap.WorldToCell(_player.transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = _tilemap.WorldToCell(mousePos);
            TileBase clickedTile = _tilemap.GetTile(cellPosition);

            if (clickedTile != null)
            {
                MovePlayerToTile(cellPosition);
            }
        }
    }

    private void MovePlayerToTile(Vector3Int cellPosition)
    {
        Vector3 worldPosition = _tilemap.CellToWorld(cellPosition) + _tilemap.tileAnchor + new Vector3(0, 0.3f, 0);
        _player.transform.position = worldPosition;

        _previousPlayerCell = cellPosition;
    }
}