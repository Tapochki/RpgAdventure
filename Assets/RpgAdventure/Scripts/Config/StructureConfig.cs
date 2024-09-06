using System.Collections.Generic;
using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Config
{
    [CreateAssetMenu(fileName = "StructureConfig", menuName = "TandC/Game/Configs/StructureConfig", order = 1)]
    public class StructureConfig : ScriptableObject
    {
        [SerializeField] private int _numberOfCities = 1;
        [SerializeField] private int _numberOfVillages = 1;
        [SerializeField] private int _numberOfTaverns = 1;
        [SerializeField] private int _numberOfCaves = 1;
        [SerializeField] private int _numberOfPortal = 1;
        [SerializeField] private int _minDistanceBetweenStructures = 3;

        [SerializeField] private Sprite _citySprite;
        [SerializeField] private Sprite _villageSprite;
        [SerializeField] private Sprite _tavernSprite;
        [SerializeField] private Sprite _caveSprite;
        [SerializeField] private Sprite _portalSprite;

        public int NumberOfCities { get => _numberOfCities; }
        public int NumberOfVillages { get => _numberOfVillages; }
        public int NumberOfTaverns { get => _numberOfTaverns; }
        public int NumberOfCaves { get => _numberOfCaves; }
        public int NumberOfPortals { get => _numberOfPortal; }
        public int MinDistanceBetweenStructures { get => _minDistanceBetweenStructures; }

        private Dictionary<StructureTileType, Sprite> _structureTileMap;

        [SerializeField]  private GameObject _structurePrefab;

        private void OnEnable()
        {
            InitializeTileMap();
        }

        private void InitializeTileMap()
        {
            _structureTileMap = new Dictionary<StructureTileType, Sprite> { 
            { StructureTileType.City, _citySprite },
            { StructureTileType.Village, _villageSprite },
            { StructureTileType.Tavern, _tavernSprite },
            { StructureTileType.Cave, _caveSprite },
            { StructureTileType.Portal, _portalSprite }
        };
        }

        public Sprite GetStructureSprite(StructureTileType structureTileType)
        {
            return _structureTileMap.TryGetValue(structureTileType, out var tile) ? tile : null;
        }

        public GameObject GetStructurePrefab(StructureTileType type)
        {
            return _structurePrefab;
        }
    }
}


