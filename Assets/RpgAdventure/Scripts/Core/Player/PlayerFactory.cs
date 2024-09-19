using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Items;
using TandC.RpgAdventure.Settings;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Player 
{
    public class PlayerFactory
    {
        [Inject] private readonly ItemFactory _itemFactory;
        [Inject] private readonly PlayerViewModel _playerViewModel;
        [Inject] private readonly CharacterConfig _characterConfig;

        public PlayerModel CreatePlayer(RaceType raceType, ClassType classType, Tilemap tileMap)
        {
            var raceData = _characterConfig.GetRaceData(raceType);
            var classData = raceData.GetClassData(classType);

            var baseAttributes = classData.baseAttributes;
            var finalAttributes = baseAttributes;

            var playerModel = new PlayerModel(finalAttributes);

            foreach (var itemId in classData.startPlayerItemsId)
            {
                var item = _itemFactory.CreateItem(itemId);
                if (item is EquippableItem equippableItem)
                {
                    playerModel.Equipment.EquipItem(equippableItem);
                }
            }

            _playerViewModel.SetPlayerPrefab(raceData.characterObject);
            _playerViewModel.Initialize(tileMap);

            return playerModel;
        }
    }
}

