using UnityEditor;
using UnityEditor.SceneManagement;

namespace TandC.RpgAdventure.Editor
{
    internal static class MenuItems
    {
        private static string _pathToBootScene = "Assets/Scenes/0.Bootstrap.unity";

        [MenuItem("Tools/Scenes/Set Boot scene default")]
        public static void DefaultScene()
        {
            SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(_pathToBootScene);

            EditorSceneManager.playModeStartScene = myWantedStartScene;
        }
    }
}