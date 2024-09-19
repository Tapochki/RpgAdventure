namespace TandC.RpgAdventure.Core.Player.Effect 
{
    public interface IItemEffect
    {
        void Apply(PlayerModel player);
    }

    public class HealEffect : IItemEffect
    {
        private int _healAmount;

        public HealEffect(int healAmount)
        {
            _healAmount = healAmount;
        }

        public void Apply(PlayerModel player)
        {
            player.Heal(_healAmount);
        }
    }

    public class StaminaRecoveryEffect : IItemEffect
    {
        private int _staminaAmount;

        public StaminaRecoveryEffect(int staminaAmount)
        {
            _staminaAmount = staminaAmount;
        }

        public void Apply(PlayerModel player)
        {
            player.RecoverStamina(_staminaAmount);
        }
    }
}

