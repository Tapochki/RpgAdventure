using UnityEngine;
using UnityEditor;
using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Settings;

[CustomPropertyDrawer(typeof(ItemData))]
public class ItemDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var itemName = property.FindPropertyRelative("itemName");
        var itemId = property.FindPropertyRelative("itemID");
        var itemRarity = property.FindPropertyRelative("itemRarity");


        Color itemColor = GetColorByRarity((ItemRariryType)itemRarity.enumValueIndex);
        GUI.backgroundColor = itemColor;

        label = new GUIContent($"{itemName.stringValue}-Id{itemId.intValue}");

        EditorGUI.PropertyField(position, property, label, true);

        GUI.backgroundColor = Color.white;
    }

    private Color GetColorByRarity(ItemRariryType rarity)
    {
        switch (rarity)
        {
            case ItemRariryType.Common:
                return Color.white;
            case ItemRariryType.Uncommon:
                return Color.green;
            case ItemRariryType.Rare:
                return Color.blue;
            case ItemRariryType.Epic:
                return new Color(0.64f, 0.21f, 0.93f);
            case ItemRariryType.Legendary:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
