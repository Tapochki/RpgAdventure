using UnityEditor;
using UnityEngine;
using TandC.RpgAdventure.Data;

[CustomEditor(typeof(LocalizationData))]
public class LocalizationDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LocalizationData localizationData = (LocalizationData)target;

        if (GUILayout.Button("Update Localization"))
        {
            localizationData.RefreshData(new GoogleSheetsService());
        }
    }
}
