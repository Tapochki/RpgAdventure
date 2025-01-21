using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Items.Effect;
using TandC.RpgAdventure.Settings;
using VContainer;

namespace TandC.RpgAdventure.Core.Items
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
                    return new WeaponItem(itemData);

                case ItemType.Armor:
                    return new ArmorItem(itemData);

                case ItemType.Accessory:
                    return new AccessoryItem(itemData);

                //add here class Companion with companion effect 
                case ItemType.Consumable:
                    var effect = _effectFactory.CreateEffect(itemData);
                    return new ConsumableItem(itemData, effect);

                case ItemType.Miscellaneous:
                    return new MiscellaneousItem(itemData);

                default:
                    throw new System.Exception("Unknown item type");
            }
        }
    }
}

