using System.Collections.Generic;
using TandC.RpgAdventure.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [SerializeField] private Tile _cityTile;
        [SerializeField] private Tile _villageTile;
        [SerializeField] private Tile _tavernTile;
        [SerializeField] private Tile _caveTile;
        [SerializeField] private Tile _portalTile;

        public int NumberOfCities { get => _numberOfCities; }
        public int NumberOfVillages { get => _numberOfVillages; }
        public int NumberOfTaverns { get => _numberOfTaverns; }
        public int NumberOfCaves { get => _numberOfCaves; }
        public int NumberOfPortals { get => _numberOfPortal; }
        public int MinDistanceBetweenStructures { get => _minDistanceBetweenStructures; }

        private Dictionary<StructureTileType, Tile> _structureTileMap;

        private void OnEnable()
        {
            InitializeTileMap();
        }

        private void InitializeTileMap()
        {
            _structureTileMap = new Dictionary<StructureTileType, Tile>
        {
            { StructureTileType.City, _cityTile },
            { StructureTileType.Village, _villageTile },
            { StructureTileType.Tavern, _tavernTile },
            { StructureTileType.Cave, _caveTile },
            { StructureTileType.Portal, _portalTile }
        };
        }

        public Tile GetStructureTile(StructureTileType structureTileType)
        {
            return _structureTileMap.TryGetValue(structureTileType, out var tile) ? tile : null;
        }
    }
}


