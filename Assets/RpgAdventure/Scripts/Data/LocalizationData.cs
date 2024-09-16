using System;
using System.Collections;
using System.Collections.Generic;
using TandC.RpgAdventure.Settings;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace TandC.RpgAdventure.Data
{
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "TandC/LocalizationData", order = 2)]
    public class LocalizationData : ScriptableObject
    {
        [SerializeField]
        public List<LocalizationLanguageData> languages;

        public Languages defaultLanguage;

        public string localizationGoogleSpreadsheet;
        public bool refreshLocalizationAtStart = true;

        [Serializable]
        public class LocalizationLanguageData
        {
            public Languages language;
            [SerializeField]
            public List<LocalizationDataInfo> localizedTexts;
        }

        [Serializable]
        public class LocalizationDataInfo
        {
            public string key;
            [TextArea(1, 9999)]
            public string value;
        }

        public void RefreshData(GoogleSheetsService googleSheetsService)
        {
            if (!string.IsNullOrEmpty(localizationGoogleSpreadsheet))
            {
                string range = "Sheet1!A1:E";
                EditorCoroutineUtility.StartCoroutine(RefreshDataCoroutine(googleSheetsService, localizationGoogleSpreadsheet, range), this);
            }
            else
            {
                Debug.LogError("Localization Google Spreadsheet ID is missing.");
            }
        }

        private IEnumerator RefreshDataCoroutine(GoogleSheetsService googleSheetsService, string spreadsheetId, string range)
        {
            var task = googleSheetsService.GetSheetData(spreadsheetId, range);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Result != null)
            {
                ParseGoogleSheetData(task.Result);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                Debug.Log("Localization data updated.");
            }
            else
            {
                Debug.LogError("Failed to retrieve data from Google Sheets.");
            }
        }

        public void ParseGoogleSheetData(IList<IList<object>> values)
        {
            languages.Clear();

            List<string> headers = new List<string>();

            for (int i = 0; i < values[0].Count; i++)
            {
                headers.Add(values[0][i].ToString());
            }

            for (int i = 1; i < values.Count; i++)
            {
                var row = values[i];

                for (int langIndex = 1; langIndex < headers.Count; langIndex++)
                {
                    var language = (Languages)Enum.Parse(typeof(Languages), headers[langIndex], true);
                    var languageData = languages.Find(l => l.language == language);
                    if (languageData == null)
                    {
                        languageData = new LocalizationLanguageData
                        {
                            language = language,
                            localizedTexts = new List<LocalizationDataInfo>()
                        };
                        languages.Add(languageData);
                    }

                    languageData.localizedTexts.Add(new LocalizationDataInfo
                    {
                        key = row[0].ToString(),
                        value = row.Count > langIndex ? row[langIndex].ToString() : string.Empty
                    });
                }
            }

            Debug.Log("Localization data updated.");
        }
    }
}
