using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject overlayPrefab;

    public GameObject player;
    public GameObject playerPrefab;
    public Vector3Int startPosition = new Vector3Int(0, 0, 0);

    private Dictionary<Vector3Int, GameObject> overlays = new Dictionary<Vector3Int, GameObject>();

    private void Start()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
            {
                continue;
            }

            if (tilemap.GetTile(pos).name == "Water_up" ||
                tilemap.GetTile(pos).name == "Water_down" ||
                tilemap.GetTile(pos).name == "Water_small" ||
                tilemap.GetTile(pos).name == "Rock")
            {
                continue;
            }

            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;
            GameObject overlay = Instantiate(overlayPrefab, worldPos, Quaternion.identity, transform);

            overlay.transform.localPosition = worldPos;
            overlays[pos] = overlay;
        }

        player = Instantiate(playerPrefab, tilemap.CellToWorld(startPosition) + tilemap.tileAnchor, Quaternion.identity);
        HighlightTiles(startPosition);
    }

    public void MovePlayerToTile(HexTile tile)
    {
        Vector3Int newPos = Vector3Int.FloorToInt(tile.transform.position);
        player.transform.position = tilemap.CellToWorld(newPos) + tilemap.tileAnchor;
        HighlightTiles(newPos);
    }

    private Vector3Int[] GetHexNeighbors(Vector3Int position)
    {
        return new Vector3Int[]
        {
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(-1, 0, 0),
            position + new Vector3Int(0, 1, 0),
            position + new Vector3Int(0, -1, 0),
            position + new Vector3Int(1, -1, 0),
            position + new Vector3Int(-1, 1, 0)
        };
    }

    private void HighlightTiles(Vector3Int worldPos)
    {
        var neighbors = GetHexNeighbors(worldPos);

        foreach (var neighbor in neighbors)
        {
            foreach (var pos in overlays.Keys)
            {
                if (pos == worldPos)
                {
                    overlays[pos].GetComponent<SpriteRenderer>().color = Color.black;
                }
                else
                {
                    overlays[pos].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}