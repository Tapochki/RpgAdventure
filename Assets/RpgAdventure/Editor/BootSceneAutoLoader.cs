using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TandC.RpgAdventure.Editor 
{
    [InitializeOnLoad]
    public static class BootSceneAutoLoader
    {
        static BootSceneAutoLoader() 
        {
            EditorBuildSettings.sceneListChanged += SetBootAsStartScene;

            SetBootAsStartScene();          
        }

        private static void SetBootAsStartScene()
        {
            SceneAsset bootSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);

            if(!bootSceneAsset.name.Contains("Boot")) 
            {
                Debug.LogError("First scene in the build settings must be Boot");
                EditorSceneManager.playModeStartScene = default;
            }

            EditorSceneManager.playModeStartScene = bootSceneAsset;
        }
    }
}

