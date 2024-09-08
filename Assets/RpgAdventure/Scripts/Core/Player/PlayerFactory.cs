using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Player 
{
    public class PlayerFactory : MonoBehaviour //Chage to normal factory
    {
        private readonly GameObject _playerPrefab;

        public PlayerFactory(GameObject playerPrefabs)
        {
            _playerPrefab = playerPrefabs;
        }

        public GameObject CreatePlayer(RaceType race)
        {
            return _playerPrefab;
        }
    }
}

