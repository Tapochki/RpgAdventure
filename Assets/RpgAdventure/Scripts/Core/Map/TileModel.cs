using TandC.RpgAdventure.Settings;
using UnityEngine;
using TandC.RpgAdventure.Core.Map.MapObject;

namespace TandC.RpgAdventure.Core.Map
{
    public class TileModel
    {
        public Vector3Int Position { get; }
        public TileType Type { get; private set; }
        public MapObjectModel MapObject { get; private set; }
        public bool IsOpen { get; private set; }

        public TileModel(Vector3Int position, TileType type, bool isOpen, MapObjectModel mapObject = null)
        {
            Position = position;
            Type = type;
            IsOpen = isOpen;
            MapObject = mapObject;
        }

        public void SetMapObject(MapObjectModel mapObject) 
        {
            MapObject = mapObject;
        }

        public void ClearMapObject()
        {
            MapObject = null;
        }

        public void SetTileOpen() 
        {
            IsOpen = true;
        }
    }
}

