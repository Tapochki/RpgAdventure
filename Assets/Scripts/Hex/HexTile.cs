using UnityEngine;

public enum HexTileType
{
    Water,
    Land,
    Forest,
    Mountain
}

public class HexTile
{
    public int Q { get; private set; }
    public int R { get; private set; }
    public Vector3 Position { get; private set; }
    public HexTileType TileType { get; set; }

    public HexTile(int q, int r, Vector3 position, HexTileType tileType)
    {
        this.Q = q;
        this.R = r;
        this.Position = position;
        this.TileType = tileType;
    }
}