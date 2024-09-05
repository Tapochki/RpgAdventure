using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Core.HexGrid
{
    public class TileModel
    {
        public Vector3Int Position { get; }
        public TileType Type { get; private set; }
        public StructureTileType StructureTileType { get; private set; }
        public bool IsOpen { get; private set; }

        public TileModel(Vector3Int position, TileType type, bool isOpen, StructureTileType structureTileType = StructureTileType.None)
        {
            Position = position;
            Type = type;
            IsOpen = isOpen;
            StructureTileType = structureTileType;
        }

        public void ChengeStructureType(StructureTileType structureTileType) 
        {
            StructureTileType = structureTileType;
        }

        public void SetTileOpen() 
        {
            IsOpen = true;
        }
    }
}

