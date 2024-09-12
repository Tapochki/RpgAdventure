using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Map.MapObject
{
    public abstract class MapObjectModel
    {
        public StructureTileType Type { get; private set; }
        public float MinSpawnDistance { get; private set; }
        public Vector3Int Position { get; private set; }

        protected MapObjectModel(StructureTileType type, Vector3Int position, float minSpawnDistance)
        {
            Type = type;
            Position = position;
            MinSpawnDistance = minSpawnDistance;
        }
    }

    public class StructureObjectModel : MapObjectModel
    {

        public StructureObjectModel(StructureTileType type, Vector3Int position, float minSpawnDistance)
            : base(type, position, minSpawnDistance)
        {
            
        }
    }

    public class CityObjectModel : StructureObjectModel
    {
        public string CityName { get; private set; }
        //add city Info

        public CityObjectModel(string cityName, Vector3Int position, float minSpawnDistance)
            : base(StructureTileType.City, position, minSpawnDistance)
        {
            CityName = cityName;
        }
    }

    public class NarrativeObjectModel : MapObjectModel
    {
        public string NarrativeDescription { get; private set; }

        public NarrativeObjectModel(StructureTileType type, Vector3Int position, string narrativeDescription, float minSpawnDistance)
            : base(type, position, minSpawnDistance)
        {
            NarrativeDescription = narrativeDescription;
        }
    }

    public class LiftTimeNarrativeObjectModel : MapObjectModel
    {
        public string NarrativeDescription { get; private set; }
        public int Lifetime { get; private set; }

        public LiftTimeNarrativeObjectModel(StructureTileType type, Vector3Int position, string narrativeDescription, float minSpawnDistance, int initialLifetime = 20)
            : base(type, position, minSpawnDistance)
        {
            NarrativeDescription = narrativeDescription;
            Lifetime = initialLifetime;
        }

        public void DecreaseLifetime()
        {
            Lifetime--;
        }

        public bool IsExpired()
        {
            return Lifetime <= 0;
        }
    }
}

