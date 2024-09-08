using UnityEngine;
using UnityEngine.SceneManagement;

namespace TandC.RpgAdventure.Settings
{
    public class AppConstants
    {
        public const string LOCAL_APP_DATA_FILE_PATH = "/BARAKUDA0002AGENT.data";
        public const string LOCAL_PURCHASE_DATA_FILE_PATH = "/BARAKUDA0003AGENT.data";
        public const string LOCAL_MAP_DATA_FILE_PATH = "/BARAKUDA0004AGENT.data";


        public const string ADDITIONAL_LOCAL_DATA_FILE_PATH = "/15FDFTG842SDJTN248STH.data";
        public const string ENCRYPT_KEY_DATA = "PRIVATE_KEY_GAME_NAME_noetonetochno";

        public static string PATH_TO_GAMES_CACHE = $"{Application.persistentDataPath}/Game/Cache";

        public static bool IS_TEST_MODE = false;

        public static bool DEBUG_ENABLE = true;

        public static bool LANGUAGE_CAN_CHANGE_IN_GAME = true;

        public static Vector3 TILE_STEP = new Vector3(0, -1.25f);

        public static class Scenes
        {
            public static readonly int Bootstrap = SceneUtility.GetBuildIndexByScenePath("0.Bootstrap");
            public static readonly int Loading = SceneUtility.GetBuildIndexByScenePath("1.Loading");
            public static readonly int Meta = SceneUtility.GetBuildIndexByScenePath("2.Meta");
            public static readonly int Core = SceneUtility.GetBuildIndexByScenePath("3.Core");
            public static readonly int Empty = SceneUtility.GetBuildIndexByScenePath("4.Empty");
        }
    }
}