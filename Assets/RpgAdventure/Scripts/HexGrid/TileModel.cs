using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.HexGrid
{
    public class TileModel
    {
        public Vector3Int Position { get; }
        public TileType Type { get; set; }

        public TileModel(Vector3Int position, TileType type)
        {
            Position = new Vector3Int(position.x, position.y);
            Type = type;
        }
    }
}

