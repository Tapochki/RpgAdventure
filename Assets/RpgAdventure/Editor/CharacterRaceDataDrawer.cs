using UnityEditor;
using UnityEngine;
using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Settings;

[CustomPropertyDrawer(typeof(CharacterRaceData))]
public class CharacterRaceDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var raceType = property.FindPropertyRelative("characterRace");

        Color raceColor = GetColorByRace((RaceType)raceType.enumValueIndex);
        GUI.backgroundColor = raceColor;

        label = new GUIContent(((RaceType)raceType.enumValueIndex).ToString());

        EditorGUI.PropertyField(position, property, label, true);

        GUI.backgroundColor = Color.white;
    }

    private Color GetColorByRace(RaceType race)
    {
        switch (race)
        {
            case RaceType.Human:
                return new Color(0.12f, 0.56f, 1.00f);
            case RaceType.Elf:
                return Color.green;
            default:
                return Color.gray;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
