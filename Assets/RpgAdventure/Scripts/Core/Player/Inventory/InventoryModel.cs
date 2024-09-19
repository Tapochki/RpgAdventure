using System.Collections.Generic;
using TandC.RpgAdventure.Core.Items;

namespace TandC.RpgAdventure.Core.Player.Inventory 
{
    public class InventoryModel
    {
        private readonly List<ItemSlot> _slots;
        private readonly float _maxWeight;

        public InventoryModel(int slotCount, float maxWeight)
        {
            _slots = new List<ItemSlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _slots.Add(new ItemSlot());
            }
            _maxWeight = maxWeight;
        }

        public IEnumerable<ItemSlot> Slots => _slots;

        public bool AddItem(Item item, int quantity)
        {
            if (item.Weight * quantity > _maxWeight)
            {
                return false;
            }

            foreach (var slot in _slots)
            {
                if (slot.CanAddItem(item, quantity))
                {
                    slot.AddItem(item, quantity);
                    return true;
                }
            }
            return false;
        }

        public bool MoveItem(int fromIndex, int toIndex, int quantity)
        {
            if (fromIndex < 0 || fromIndex >= _slots.Count || toIndex < 0 || toIndex >= _slots.Count)
            {
                return false;
            }

            var fromSlot = _slots[fromIndex];
            var toSlot = _slots[toIndex];

            if (fromSlot.CanRemoveItem(quantity))
            {
                var item = fromSlot.Item;
                var remainingQuantity = fromSlot.RemoveItem(quantity);

                if (toSlot.CanAddItem(item, remainingQuantity))
                {
                    toSlot.AddItem(item, remainingQuantity);
                    return true;
                }
                else
                {
                    fromSlot.AddItem(item, remainingQuantity);
                    return false;
                }
            }
            return false;
        }

        public bool RemoveItem(int slotIndex, int quantity)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
            {
                return false;
            }

            var slot = _slots[slotIndex];
            return slot.RemoveItem(quantity) > 0;
        }
    }
}

