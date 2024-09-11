using System.Collections.Generic;
using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Config
{
    [CreateAssetMenu(fileName = "StructureConfig", menuName = "TandC/Game/Configs/StructureConfig", order = 1)]
    public class StructureConfig : ScriptableObject
    {
        [SerializeField] private int _numberOfCities = 1;
        [SerializeField] private int _numberOfVolcano = 1;
        [SerializeField] private int _numberOfUniqueQuestionEvent = 1;
        [SerializeField] private int _numberOfGoldQuestionEvent = 1;

        [SerializeField] private int _minDistanceBetweenStructures = 3;

        [SerializeField] private GameObject _cityPrefab;
        [SerializeField] private GameObject _volcanoPrefab;
        [SerializeField] private GameObject _uniqueEventQuestionPrefab;
        [SerializeField] private GameObject _uniqueEventExclamationPrefab;
        [SerializeField] private GameObject _goldEventQuestionPrefab;
        [SerializeField] private GameObject _goldEventExclamationPrefab;
        [SerializeField] private GameObject _defaultEventQuestionPrefab;
        [SerializeField] private GameObject _defaultEventExclamationPrefab;

        public int NumberOfCities { get => _numberOfCities; }
        public int NumberOfVolcano { get => _numberOfVolcano; }
        public int NumberOfUniqueEvent { get => _numberOfUniqueQuestionEvent; }
        public int NumberOfGoldEvent { get => _numberOfGoldQuestionEvent; }
        public int MinDistanceBetweenStructures { get => _minDistanceBetweenStructures; }

        [SerializeField]  private Dictionary<StructureTileType, GameObject> _structurePrefabs;

        private void OnEnable()
        {
            InitializeTileMap();
        }

        private void InitializeTileMap()
        {
            _structurePrefabs = new Dictionary<StructureTileType, GameObject> 
            {
                { StructureTileType.City, _cityPrefab },
                { StructureTileType.Volcano, _volcanoPrefab },
                { StructureTileType.UniqueQuestionMark, _uniqueEventQuestionPrefab },
                { StructureTileType.UniqueExclamationMark, _uniqueEventExclamationPrefab },
                { StructureTileType.GoldQuestionMark, _goldEventQuestionPrefab },
                { StructureTileType.GoldExclamationMark, _goldEventExclamationPrefab },
                { StructureTileType.DefaultQuestionMark, _defaultEventQuestionPrefab },
                { StructureTileType.DefaultExclamationMark, _defaultEventExclamationPrefab },
            };
        }

        public GameObject GetStructureGameobject(StructureTileType structureTileType)
        {
            return _structurePrefabs.TryGetValue(structureTileType, out var tile) ? tile : null;
        }
    }
}


