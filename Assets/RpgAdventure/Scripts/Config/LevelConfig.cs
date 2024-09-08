using UnityEngine;
using UnityEngine.Tilemaps;

namespace TandC.RpgAdventure.Config
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "TandC/Game/Configs/LevelConfig", order = 1)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private LevelTilemaps[] levels;

        public Tilemap GetRandomTileMapForLevel(int levelId)
        {
            foreach (var level in levels)
            {
                if (level.LevelId == levelId)
                {
                    if (level.TileMap == null || level.TileMap.Length == 0)
                    {
                        Debug.LogWarning($"No tilemaps available for level {levelId}");
                        return null;
                    }
                    int randomIndex = Random.Range(0, level.TileMap.Length);
                    return level.TileMap[randomIndex];
                }
            }

            Debug.LogWarning($"Level {levelId} not found");
            return null;
        }

        public bool LevelExists(int levelId)
        {
            foreach (var level in levels)
            {
                if (level.LevelId == levelId)
                {
                    return true;
                }
            }
            return false;
        }

        [System.Serializable]
        private class LevelTilemaps
        {
            public int LevelId;
            public Tilemap[] TileMap;
        }
    }

}
