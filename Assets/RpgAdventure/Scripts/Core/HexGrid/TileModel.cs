using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class TileModel
    {
        public Vector3Int Position { get; }
        public TileType Type { get; set; }

        public TileModel(Vector3Int position, TileType type)
        {
            Position = position;
            Type = type;
        }
    }
}

