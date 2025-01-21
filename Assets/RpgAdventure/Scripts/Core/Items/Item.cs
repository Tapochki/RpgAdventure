using TandC.RpgAdventure.Config.Player;
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

        protected Item(ItemData itemData)
        {
            ItemID = itemData.itemID;
            ItemName = itemData.itemName;
            ItemIcon = itemData.itemIcon;
            ItemRarity = itemData.itemRarity;
            BaseValue = itemData.baseValue;
            Type = itemData.type;
            Weight = itemData.weight;
            MaxStack = itemData.maxStack;
        }
    }

    public abstract class EquippableItem : Item
    {
        public EquipmentSlot Slot { get; private set; }
        public CharacterAttributes Attributes { get; private set; }

        protected EquippableItem(ItemData itemData)
            : base(itemData)
        {
            Slot = itemData.slot;
            Attributes = itemData.characterAttributes;
        }
    }

    public class WeaponItem : EquippableItem
    {
        public int WeaponDamage { get; private set; }

        public WeaponItem(ItemData itemData) :
            base(itemData)
        {
            WeaponDamage = itemData.weaponDamage;
        }
    }

    public class ArmorItem : EquippableItem
    {
        public ArmorItem(ItemData itemData) :
            base(itemData)
        {
        }
    }

    public class AccessoryItem : EquippableItem
    {
        public AccessoryItem(ItemData itemData) :
            base(itemData)
        {
        }
    }

    public class ConsumableItem : Item
    {
        private IItemEffect _effect;

        public ConsumableItem(ItemData itemData, IItemEffect itemEffect) :
            base(itemData)
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
        public MiscellaneousItem(ItemData itemData) :
            base(itemData)
        {

        }
    }
}

