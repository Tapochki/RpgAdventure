using TandC.RpgAdventure.Core.Items;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Player.Inventory 
{
    public class ItemSlot
    {
        private Item _item;
        private int _quantity;

        public Item Item => _item;
        public int Quantity => _quantity;

        public bool CanAddItem(Item item, int quantity)
        {
            if (_item == null || _item.ItemID == item.ItemID)
            {
                return _item != null || quantity <= item.MaxStack;
            }
            return false;
        }

        public bool CanRemoveItem(int quantity)
        {
            return _item != null && _quantity >= quantity;
        }

        public void AddItem(Item item, int quantity)
        {
            if (_item == null || _item.ItemID == item.ItemID)
            {
                _item = item;
                _quantity = Mathf.Min(quantity, item.MaxStack);
            }
        }

        public int RemoveItem(int quantity)
        {
            if (_item == null) return 0;

            var removedQuantity = Mathf.Min(_quantity, quantity);
            _quantity -= removedQuantity;

            if (_quantity <= 0)
            {
                _item = null;
            }
            return removedQuantity;
        }
    }
}

