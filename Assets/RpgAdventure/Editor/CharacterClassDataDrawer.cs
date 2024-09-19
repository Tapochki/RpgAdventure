using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Settings;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterClassData))]
public class CharacterClassDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var raceType = property.FindPropertyRelative("characterClass");
        var portrait = property.FindPropertyRelative("portrait");

        label = new GUIContent(((ClassType)raceType.enumValueIndex).ToString());

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
