using TandC.RpgAdventure.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using TandC.RpgAdventure.Settings;

namespace TandC.RpgAdventure.Services
{
    public class SceneManager
    {
        private const string LogTag = "SCENE";
        
        public async UniTask LoadScene(int toLoadIndex)
        {
            int currentSceneIndex = UnitySceneManager.GetActiveScene().buildIndex;
            bool isSkipEmpty = currentSceneIndex == AppConstants.Scenes.Loading || currentSceneIndex == AppConstants.Scenes.Bootstrap || toLoadIndex == currentSceneIndex;

            if (isSkipEmpty)
            {
                Log.Default.D(LogTag, $"Empty scene skipped. {SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
                UnitySceneManager.LoadScene(toLoadIndex);
                return;
            }
            
            bool needLoadEmpty = toLoadIndex == AppConstants.Scenes.Meta || toLoadIndex == AppConstants.Scenes.Core || toLoadIndex == AppConstants.Scenes.Loading;

            if (needLoadEmpty)
            {
                Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(AppConstants.Scenes.Empty)} is loading.");
                UnitySceneManager.LoadScene(AppConstants.Scenes.Empty);
            }
            
            await UniTask.NextFrame();
            
            Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
            UnitySceneManager.LoadScene(toLoadIndex);
        }
    }
}