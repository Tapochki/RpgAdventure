using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexMapData
{
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public List<HexTileData> tiles;
}

[Serializable]
public class HexTileData
{
    public int q;
    public int r;
    public HexTileType tileType;

    public HexTileData(int q, int r, HexTileType tileType)
    {
        this.q = q;
        this.r = r;
        this.tileType = tileType;
    }
}
