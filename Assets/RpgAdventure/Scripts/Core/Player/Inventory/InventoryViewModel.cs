using System.Linq;
using TandC.RpgAdventure.Core.Items;
using UniRx;

namespace TandC.RpgAdventure.Core.Player.Inventory
{
    public class InventoryViewModel
    {
        private readonly InventoryModel _inventory;
        private readonly Equipment _equipment;

        public ReactiveCollection<InventorySlotViewModel> Slots { get; private set; }
        public ReactiveCommand<InventorySlotViewModel> EquipItemCommand { get; private set; }
        public ReactiveCommand<MoveItemCommandParameter> MoveItemCommand { get; private set; }
        public ReactiveCommand<int> RemoveItemCommand { get; private set; }

        public InventoryViewModel(InventoryModel inventory, Equipment equipment)
        {
            _inventory = inventory;
            _equipment = equipment;

            Slots = new ReactiveCollection<InventorySlotViewModel>();
            UpdateSlots();

            EquipItemCommand = new ReactiveCommand<InventorySlotViewModel>();
            EquipItemCommand.Subscribe(EquipItem);

            MoveItemCommand = new ReactiveCommand<MoveItemCommandParameter>();
            MoveItemCommand.Subscribe(MoveItem);

            RemoveItemCommand = new ReactiveCommand<int>();
            RemoveItemCommand.Subscribe(RemoveItem);
        }

        private void UpdateSlots()
        {
            Slots.Clear();
            for (int i = 0; i < _inventory.Slots.Count(); i++)
            {
                var slot = _inventory.Slots.ElementAt(i);
                Slots.Add(new InventorySlotViewModel(slot.Item, slot.Quantity, i));
            }
        }

        private void EquipItem(InventorySlotViewModel slotViewModel)
        {
            if (slotViewModel.Item.Value is EquippableItem equipItem)
            {
                _equipment.EquipItem(equipItem);
            }
        }

        private void MoveItem(MoveItemCommandParameter parameter)
        {
            if (_inventory.MoveItem(parameter.FromSlot, parameter.ToSlot, parameter.Quantity))
            {
                UpdateSlots();
            }
        }

        private void RemoveItem(int slotIndex)
        {
            if (_inventory.RemoveItem(slotIndex, 1))
            {
                UpdateSlots();
            }
        }
    }

    public class MoveItemCommandParameter
    {
        public int FromSlot { get; }
        public int ToSlot { get; }
        public int Quantity { get; }

        public MoveItemCommandParameter(int fromSlot, int toSlot, int quantity)
        {
            FromSlot = fromSlot;
            ToSlot = toSlot;
            Quantity = quantity;
        }
    }

    public class InventorySlotViewModel
    {
        public ReactiveProperty<Item> Item { get; private set; }
        public ReactiveProperty<int> Quantity { get; private set; }
        public int SlotIndex { get; private set; }

        public InventorySlotViewModel(Item item, int quantity, int slotIndex)
        {
            Item = new ReactiveProperty<Item>(item);
            Quantity = new ReactiveProperty<int>(quantity);
            SlotIndex = slotIndex;
        }

        public void SetQuantity(int quantity)
        {
            Quantity.Value = quantity;
        }

        public void SetItem(Item newItem)
        {
            Item.Value = newItem;
        }
    }
}

