using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Settings;

namespace TandC.RpgAdventure.Core.Player.Effect
{
    public class ItemEffectFactory
    {
        public IItemEffect CreateEffect(ItemData itemData)
        {
            switch (itemData.effectType)
            {
                case EffectType.Heal:
                    return new HealEffect(itemData.effectStrength);
                case EffectType.StaminaRecovery:
                    return new StaminaRecoveryEffect(itemData.effectStrength);
                default:
                    throw new System.Exception("Unknown effect type");
            }
        }
    }
}

