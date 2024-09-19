using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Player.Effect;
using TandC.RpgAdventure.Settings;
using VContainer;

namespace TandC.RpgAdventure.Core.Item
{
    public class ItemFactory
    {
        [Inject] private ItemConfig _itemConfig;
        [Inject] private ItemEffectFactory _effectFactory;

        public Item CreateItem(int itemID)
        {
            var itemData = _itemConfig.GetItemByID(itemID);

            switch (itemData.type)
            {
                //add here weaponData attack damage and 
                case ItemType.Weapon:
                    return new WeaponItem(itemData.itemID, itemData.itemName, itemData.itemIcon, itemData.itemRarity, 
                        itemData.baseValue, itemData.type, itemData.slot, itemData.characterAttributes, itemData.weight, itemData.weaponDamage);

                case ItemType.Armor:
                    return new ArmorItem(itemData.itemID, itemData.itemName, itemData.itemIcon, itemData.itemRarity,
                        itemData.baseValue, itemData.type, itemData.slot, itemData.characterAttributes, itemData.weight);

                case ItemType.Accessory:
                    return new AccessoryItem(itemData.itemID, itemData.itemName, itemData.itemIcon, itemData.itemRarity,
                        itemData.baseValue, itemData.type, itemData.slot, itemData.characterAttributes, itemData.weight);

                //add here class Companion with companion effect 
                case ItemType.Consumable:
                    var effect = _effectFactory.CreateEffect(itemData);
                    return new ConsumableItem(itemData.itemID, itemData.itemName, itemData.itemIcon, itemData.itemRarity, 
                        itemData.baseValue, itemData.type, itemData.weight, itemData.maxStack, effect);

                case ItemType.Miscellaneous:
                    return new MiscellaneousItem(itemData.itemID, itemData.itemName, itemData.itemIcon, itemData.itemRarity,
                        itemData.baseValue, itemData.type, itemData.weight, itemData.maxStack);

                default:
                    throw new System.Exception("Unknown item type");
            }
        }
    }
}

