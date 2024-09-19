using System;
using System.Collections.Generic;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Config.Player
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "TandC/Game/Configs/ItemConfig", order = 4)]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private List<ItemData> weapons = new List<ItemData>();
        [SerializeField] private List<ItemData> armors = new List<ItemData>();
        [SerializeField] private List<ItemData> accessories = new List<ItemData>();
        [SerializeField] private List<ItemData> consumables = new List<ItemData>();
        [SerializeField] private List<ItemData> companions = new List<ItemData>();
        [SerializeField] private List<ItemData> miscellaneous = new List<ItemData>();

        private List<ItemData> allItems = new List<ItemData>();

        public List<ItemData> GetAllItems()
        {
            allItems.Clear();
            allItems.AddRange(weapons);
            allItems.AddRange(armors);
            allItems.AddRange(accessories);
            allItems.AddRange(consumables);
            allItems.AddRange(companions);
            allItems.AddRange(miscellaneous);

            return allItems;
        }

        private void RecalculateIDs()
        {
            int id = 1;
            foreach (var item in GetAllItems())
            {
                item.itemID = id;
                id++;
            }
        }

        public ItemData GetItemByID(int itemID)
        {
            return GetAllItems().Find(item => item.itemID == itemID);
        }

        public void AddItem(ItemData newItem)
        {
            switch (newItem.type)
            {
                case ItemType.Weapon:
                    weapons.Add(newItem);
                    break;
                case ItemType.Armor:
                    armors.Add(newItem);
                    break;
                case ItemType.Accessory:
                    accessories.Add(newItem);
                    break;
                case ItemType.Consumable:
                    consumables.Add(newItem);
                    break;
                case ItemType.Companion:
                    companions.Add(newItem);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneous.Add(newItem);
                    break;
                default:
                    Debug.LogError("Unknown item type!");
                    break;
            }

            RecalculateIDs();
        }
    }

    [Serializable]
    public class ItemData
    {
        public int itemID;
        public string itemName;
        public Sprite itemIcon;
        public ItemRariryType itemRarity;
        public int baseValue;
        public ItemType type;

        public EquipmentSlot slot;

        public int weaponDamage;

        public CharacterAttributes characterAttributes;

        public int effectStrength;
        public bool isInfinite;

        public float weight;
        public int maxStack;

        public EffectType effectType;

        public ItemData(int id, string name, ItemType type)
        {
            itemID = id;
            itemName = name;
            this.type = type;
        }
    }
}
