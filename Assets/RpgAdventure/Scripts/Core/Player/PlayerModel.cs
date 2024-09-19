using TandC.RpgAdventure.Core.Items;
using TandC.RpgAdventure.Core.Player.Inventory;
using UniRx;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Player 
{
    public class PlayerModel
    {
        public ReactiveProperty<int> CurrentHealth { get; private set; }
        public ReactiveProperty<int> CurrentStamina { get; private set; }

        public CharacterAttributes BaseAttributes { get; private set; }

        public ReactiveProperty<CharacterAttributes> FinalAttributes { get; private set; }

        public Equipment Equipment { get; private set; }

        public PlayerModel(CharacterAttributes baseAttributes)
        {
            BaseAttributes = baseAttributes;
            FinalAttributes = new ReactiveProperty<CharacterAttributes>(BaseAttributes);

            CurrentHealth = new ReactiveProperty<int>(FinalAttributes.Value.Health);
            CurrentStamina = new ReactiveProperty<int>(FinalAttributes.Value.Stamina);

            FinalAttributes = new ReactiveProperty<CharacterAttributes>(baseAttributes);

            Equipment = new Equipment();
            Equipment.OnEquipmentChanged.Subscribe(_ => RecalculateFinalAttributes()); 
        }

        private void RecalculateFinalAttributes()
        {
            var equipmentAttributes = Equipment.GetTotalEquipmentAttributes(); 
            FinalAttributes.Value = BaseAttributes + equipmentAttributes;

            CurrentHealth.Value = Mathf.Min(CurrentHealth.Value, FinalAttributes.Value.Health);
            CurrentStamina.Value = Mathf.Min(CurrentStamina.Value, FinalAttributes.Value.Stamina);
        }

        public void UseItem(ConsumableItem item)
        {
            item.Use(this);
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth.Value = Mathf.Max(CurrentHealth.Value - damage, 0);
        }

        public void Heal(int amount)
        {
            CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + amount, FinalAttributes.Value.Health);
        }

        public void UseStamina(int amount)
        {
            CurrentStamina.Value = Mathf.Max(CurrentStamina.Value - amount, 0);
        }

        public void RecoverStamina(int amount)
        {
            CurrentStamina.Value = Mathf.Min(CurrentStamina.Value + amount, FinalAttributes.Value.Stamina);
        }
    }
}

