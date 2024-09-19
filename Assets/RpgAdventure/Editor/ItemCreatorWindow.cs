using UnityEngine;
using UnityEditor;
using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;

public class ItemCreatorWindow : EditorWindow
{
    private string itemName = "New Items";
    private Sprite itemIcon;
    private ItemType itemType;
    private ItemRariryType itemRareType;
    private int baseValue = 0;
    private float weight = 0f;
    private EquipmentSlot equipmentSlot;
    private int weaponDamage = 0;
    private CharacterAttributes characterAttributes = new CharacterAttributes();
    private int effectStrength = 0;
    private bool isInfinite = false;
    private EffectType effectType;
    private int maxStack = 1;

    private ItemConfig itemConfig;

    [MenuItem("TandC Tools/Items Creator")]
    public static void ShowWindow()
    {
        GetWindow<ItemCreatorWindow>("Items Creator");
    }

    private void OnEnable()
    {
        if (itemConfig == null)
        {
            itemConfig = AssetDatabase.LoadAssetAtPath<ItemConfig>("Assets/Assets/Confings/ItemConfig.asset");
            itemRareType = ItemRariryType.Common;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Items Creator", EditorStyles.boldLabel);

        itemConfig = (ItemConfig)EditorGUILayout.ObjectField("Items Config", itemConfig, typeof(ItemConfig), false);
        itemName = EditorGUILayout.TextField("Items Name", itemName);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Items Icon", itemIcon, typeof(Sprite), false);
        itemType = (ItemType)EditorGUILayout.EnumPopup("Items Type", itemType);
        itemRareType = (ItemRariryType)EditorGUILayout.EnumPopup("Items Rarity", itemRareType);
        baseValue = EditorGUILayout.IntField("Base Value", baseValue);
        weight = EditorGUILayout.FloatField("Weight", weight);
        maxStack = EditorGUILayout.IntField("Max Stack", maxStack);
        if (itemType == ItemType.Weapon)
        {
            equipmentSlot = (EquipmentSlot)EditorGUILayout.EnumPopup("Equipment Slot", equipmentSlot);
            weaponDamage = EditorGUILayout.IntField("Weapon Damage", weaponDamage);
        }
        else if (itemType == ItemType.Armor || itemType == ItemType.Accessory)
        {
            equipmentSlot = (EquipmentSlot)EditorGUILayout.EnumPopup("Equipment Slot", equipmentSlot);
            EditorGUILayout.LabelField("Character Attributes");
            if (characterAttributes == null)
            {
                characterAttributes = new CharacterAttributes();
            }
            EditorGUI.indentLevel++;
            characterAttributes.Strength = EditorGUILayout.IntField("Strength", characterAttributes.Strength);
            characterAttributes.Agility = EditorGUILayout.IntField("Agility", characterAttributes.Agility);
            characterAttributes.Intelligence = EditorGUILayout.IntField("Intelligence", characterAttributes.Intelligence);
            characterAttributes.Charisma = EditorGUILayout.IntField("Charisma", characterAttributes.Charisma);
            characterAttributes.Luck = EditorGUILayout.IntField("Luck", characterAttributes.Luck);
            characterAttributes.Perception = EditorGUILayout.IntField("Perception", characterAttributes.Perception);
            characterAttributes.Health = EditorGUILayout.IntField("Health", characterAttributes.Health);
            characterAttributes.Stamina = EditorGUILayout.IntField("Stamina", characterAttributes.Stamina);
            EditorGUI.indentLevel--;
        }
        else if (itemType == ItemType.Consumable)
        {
            effectStrength = EditorGUILayout.IntField("Effect Strength", effectStrength);
            effectType = (EffectType)EditorGUILayout.EnumPopup("Effect Type", effectType);
            isInfinite = EditorGUILayout.Toggle("Is Infinite", isInfinite);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Add Items"))
        {
            if (itemConfig != null)
            {
                AddNewItem();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign an ItemConfig!", "OK");
            }
        }
    }

    private void AddNewItem()
    {
        ItemData newItem = new ItemData(0, itemName, itemType)
        {
            itemIcon = itemIcon,
            baseValue = baseValue,
            weight = weight,
            slot = equipmentSlot,
            weaponDamage = weaponDamage,
            characterAttributes = characterAttributes,
            itemRarity = itemRareType,
            effectStrength = effectStrength,
            isInfinite = isInfinite,
            effectType = effectType,
            maxStack = maxStack,
        };

        itemConfig.AddItem(newItem);

        EditorUtility.DisplayDialog("Success", "Items added successfully!", "OK");
    }

    private void ClearWindow() 
    {
        itemName = "New Items";
        itemIcon = null;
        itemType = ItemType.None;
        baseValue = 0;
        weight = 0f;
        equipmentSlot = EquipmentSlot.None;
        weaponDamage = 0;
        characterAttributes = null;
        effectStrength = 0;
        isInfinite = false;
    }
}
