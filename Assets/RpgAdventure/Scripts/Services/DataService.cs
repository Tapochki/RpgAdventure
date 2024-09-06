using System;
using System.Collections.Generic;
using System.IO;
using TandC.Project.Utilities;
using TandC.RpgAdventure.Settings;
using TandC.RpgAdventure.Utilities.Logging;
using UnityEngine;

namespace TandC.RpgAdventure.Services
{
    public class DataService : MonoBehaviour
    {
        public event Action OnCacheLoadedEvent;

        public event Action<CacheType> OnCacheResetEvent;

        private Dictionary<CacheType, string> _cacheDataPathes;

        public AppSettingsData AppSettingsData { get; private set; }
        public PurchaseData PurchaseData { get; private set; }
        public MapData MapData { get; private set; }

        private void Construct()
        {
            
        }

        public void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            FillCacheDataPathes();

            if (!Directory.Exists(AppConstants.PATH_TO_GAMES_CACHE))
            {
                Directory.CreateDirectory(AppConstants.PATH_TO_GAMES_CACHE);
            }

            StartLoadCache();
        }

        private void StartLoadCache()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CacheType)).Length; i++)
            {
                LoadCachedData((CacheType)i);
            }

            OnCacheLoadedEvent?.Invoke();
        }

        public void SaveAllCache()
        {
            int count = Enum.GetNames(typeof(CacheType)).Length;
            for (int i = 0; i < count; i++)
            {
                SaveCache((CacheType)i);
            }
        }

        public void SaveCache(CacheType type)
        {
            if (!File.Exists(_cacheDataPathes[type]))
            {
                File.Create(_cacheDataPathes[type]).Close();
            }

            switch (type)
            {
                case CacheType.AppSettingsData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(AppSettingsData));
                    break;

                case CacheType.PurchaseData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(PurchaseData));
                    break;

                case CacheType.MapData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(MapData));
                    break;

                default:
                    Log.Default.W($"[{type}] is not implemented");
                    break;
            }
        }

        private void WriteTextToFile(string dataPath, string contents)
        {
            File.WriteAllText(dataPath, contents);
        }

        private void SetDefaultAppSettingData()
        {
            AppSettingsData = new AppSettingsData()
            {
                isFirstRun = true,
                appLanguage = (Languages)Application.systemLanguage,
                musicVolume = 1,
                soundVolume = 1,
            };
        }

        private void SetDefaultPurchaseData()
        {
            PurchaseData = new PurchaseData()
            {
                isRemovedAds = false,
            };
        }

        private void SetDefaultMapData()
        {
            MapData = new MapData()
            {
                LevelId = 0,
                Tiles = new List<TileSaveData>(),
                PlayerPosition = Vector3Int.zero
            };
        }

        public MapData GetDefaultMapData() 
        {
            SetDefaultMapData();
            return MapData;
        }

        private void LoadCachedData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    if (CheckIfPathExist(type, SetDefaultAppSettingData))
                    {
                        AppSettingsData = InternalTools.DeserializeData<AppSettingsData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.PurchaseData:
                    if (CheckIfPathExist(type, SetDefaultPurchaseData))
                    {
                        PurchaseData = InternalTools.DeserializeData<PurchaseData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;
                case CacheType.MapData:
                    if (CheckIfPathExist(type, SetDefaultMapData))
                    {
                        MapData = InternalTools.DeserializeData<MapData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;


                default:
                    {
                        Log.Default.W($"[{type}] is not implemented");
                        return;
                    }
            }
        }

        private bool CheckIfPathExist(CacheType type, Action SetDefault)
        {
            if (!File.Exists(_cacheDataPathes[type]))
            {
                SetDefault?.Invoke();
                SaveCache(type);
                return false;
            }
            return true;
        }

        private void FillCacheDataPathes()
        {
            _cacheDataPathes = new Dictionary<CacheType, string>
            {
                { CacheType.AppSettingsData, Application.persistentDataPath + AppConstants.LOCAL_APP_DATA_FILE_PATH },
                { CacheType.PurchaseData, Application.persistentDataPath + AppConstants.LOCAL_PURCHASE_DATA_FILE_PATH },
                { CacheType.MapData, Application.persistentDataPath + AppConstants.LOCAL_MAP_DATA_FILE_PATH },
            };
        }

        public void ResetData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    SetDefaultAppSettingData();
                    break;

                case CacheType.PurchaseData:
                    SetDefaultPurchaseData();
                    break;

                case CacheType.MapData:
                    SetDefaultMapData();
                    break;

                default:
                    {
                        Log.Default.W($"[{type}] is not implemented");
                        return;
                    }
            }

            SaveCache(type);
            OnCacheResetEvent?.Invoke(type);
        }
    }
}