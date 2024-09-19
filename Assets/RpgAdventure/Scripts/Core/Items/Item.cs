using TandC.RpgAdventure.Core.Items.Effect;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Items
{
    public abstract class Item
    {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public Sprite ItemIcon { get; private set; }
        public ItemRariryType ItemRarity { get; private set; }
        public int BaseValue { get; private set; }
        public ItemType Type { get; private set; }
        public float Weight { get; private set; }
        public int MaxStack { get; private set; }

        protected Item(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, float weight, int maxStack)
        {
            ItemID = itemID;
            ItemName = itemName;
            ItemIcon = itemIcon;
            ItemRarity = itemRarity;
            BaseValue = baseValue;
            Type = type;
            Weight = weight;
            MaxStack = maxStack;
        }
    }

    public abstract class EquippableItem : Item
    {
        public EquipmentSlot Slot { get; private set; }
        public CharacterAttributes Attributes { get; private set; }

        protected EquippableItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, EquipmentSlot slot, CharacterAttributes attributes, float weight)
            : base(itemID, itemName, itemIcon, itemRarity, baseValue, type, weight, 1)
        {
            Slot = slot;
            Attributes = attributes;
        }
    }

    public class WeaponItem : EquippableItem
    {
        public int WeaponDamage { get; private set; }

        public WeaponItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, EquipmentSlot slot, CharacterAttributes attributes, float weight, int weaponDamage) :
            base(itemID, itemName, itemIcon, itemRarity, baseValue, type, slot, attributes, weight)
        {
            WeaponDamage = weaponDamage;
        }
    }

    public class ArmorItem : EquippableItem
    {
        public ArmorItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, EquipmentSlot slot, CharacterAttributes attributes, float weight) :
            base(itemID, itemName, itemIcon, itemRarity, baseValue, type, slot, attributes, weight)
        {
        }
    }

    public class AccessoryItem : EquippableItem
    {
        public AccessoryItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, EquipmentSlot slot, CharacterAttributes attributes, float weight) :
            base(itemID, itemName, itemIcon, itemRarity, baseValue, type, slot, attributes, weight)
        {
        }
    }

    public class ConsumableItem : Item
    {
        private IItemEffect _effect;

        public ConsumableItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, float weight, int maxStack, IItemEffect itemEffect) :
            base(itemID, itemName, itemIcon, itemRarity, baseValue, type, weight, maxStack)
        {
            _effect = itemEffect;
        }

        public void Use(PlayerModel player)
        {
            _effect.Apply(player);
        }
    }

    public class MiscellaneousItem : Item
    {
        public MiscellaneousItem(int itemID, string itemName, Sprite itemIcon, ItemRariryType itemRarity, int baseValue, ItemType type, float weight, int maxStack) :
            base(itemID, itemName, itemIcon, itemRarity, baseValue, type, weight, maxStack)
        {

        }
    }
}

