using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Core.Item;
using TandC.RpgAdventure.Settings;
using UniRx;

namespace TandC.RpgAdventure.Core.Player 
{
    public class Equipment
    {
        private Dictionary<EquipmentSlot, EquippableItem> _equippedItems;
        public ReactiveCommand OnEquipmentChanged { get; private set; }

        public Equipment()
        {
            _equippedItems = new Dictionary<EquipmentSlot, EquippableItem>();
            OnEquipmentChanged = new ReactiveCommand();
        }

        public void EquipItem(EquippableItem item)
        {
            var slots = GetRequiredSlots(item);
            foreach (var slot in slots)
            {
                if (_equippedItems.ContainsKey(slot))
                {
                    UnequipItem(slot);
                }
            }

            _equippedItems[item.Slot] = item;

            OnEquipmentChanged.Execute();
        }

        public void UnequipItem(EquipmentSlot slot, bool isChanging = false)
        {
            if (_equippedItems.TryGetValue(slot, out var item))
            {
                var slotsToRemove = GetRequiredSlots(item).ToList();
                foreach (var s in slotsToRemove)
                {
                    _equippedItems.Remove(s);
                }
                if(isChanging)
                    OnEquipmentChanged.Execute();
            }
        }

        public CharacterAttributes GetTotalEquipmentAttributes()
        {
            CharacterAttributes totalAttributes = new CharacterAttributes();

            foreach (var item in _equippedItems.Values.OfType<EquippableItem>())
            {
                totalAttributes += item.Attributes;
            }

            return totalAttributes;
        }

        private IEnumerable<EquipmentSlot> GetRequiredSlots(EquippableItem item)
        {
            switch (item.Slot)
            {
                case EquipmentSlot.BothHands:
                    return new[] { EquipmentSlot.LeftHand, EquipmentSlot.RightHand };
                default:
                    return new[] { item.Slot };
            }
        }
    }
}

