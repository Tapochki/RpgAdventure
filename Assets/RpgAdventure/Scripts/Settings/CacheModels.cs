using System.Collections.Generic;
using TandC.RpgAdventure.Core.Map.MapObject;
using UnityEngine;

namespace TandC.RpgAdventure.Settings
{
    public class PurchaseData
    {
        public bool isRemovedAds;
    }

    public class AppSettingsData
    {
        public bool isFirstRun;
        public float soundVolume;
        public float musicVolume;
        public Languages appLanguage;
    }

    public class LocalisationSheetData
    {
        public string Key;
        public string English;
        public string Ukrainian;
        public string Russian;
    }

    public class MapData
    {
        public int LevelId;
        public List<TileSaveData> Tiles;
        public Vector3Int PlayerPosition;

        public bool HasSaveData() 
        {
            return LevelId > 0;
        }
    }

    public class PlayerConfig
    {
        public ClassType ClassType;
        public RaceType RaceType;
        public PlayerCharacteristics PlayerCharacteristics;
    }

    public class PlayerCharacteristics
    {
        public int Strength;
        public int Dexterity;
        public int Intelligence;
        public int Charisma;
        public int Luck;
        public int Perception;
    }

    public class TileSaveData
    {
        public Vector3Int Position;
        public TileType Type;
        public MapObjectModel MapObject;
        public bool IsOpen;
    }
}