
namespace TandC.RpgAdventure.Core.Player.Inventory 
{
    public class WalletModel
    {
        public int Gold { get; private set; }

        public WalletModel(int initialGold = 0)
        {
            Gold = initialGold;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }

        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }
    }
}

