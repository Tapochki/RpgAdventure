#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMapGenerator))]
public class HexMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexMapGenerator hexMapGenerator = (HexMapGenerator)target;

        if (GUILayout.Button("Generate Random Map"))
        {
            hexMapGenerator.GenerateRandomMap();
        }

        if (GUILayout.Button("Generate Map from Seed"))
        {
            hexMapGenerator.GenerateMapFromSeed();
        }

        if (GUILayout.Button("Save Map"))
        {
            hexMapGenerator.SaveMapMenu();
        }

        if (GUILayout.Button("Load Map"))
        {
            hexMapGenerator.LoadMapMenu();
        }

        if (GUILayout.Button("Clear Map"))
        {
            hexMapGenerator.ClearMapMenu();
        }
    }
}
#endif