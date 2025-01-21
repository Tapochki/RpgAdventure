using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Items;
using TandC.RpgAdventure.Core.Player.Inventory;
using TandC.RpgAdventure.Settings;
using UnityEngine;
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

            CreateInv(classData.startPlayerItemsId);

            _playerViewModel.SetPlayerPrefab(raceData.characterObject);
            _playerViewModel.Initialize(tileMap);

            return playerModel;
        }

        private void CreateInv(int[] startPlayerItemsId)
        {
            InventoryModel inventoryModel = new InventoryModel(100, 100);
            Equipment equipment = new Equipment();
            foreach (var itemId in startPlayerItemsId)
            {
                var item = _itemFactory.CreateItem(itemId);
                if (item is EquippableItem equippableItem)
                {
                    equipment.EquipItem(equippableItem);
                }
                else
                {
                    inventoryModel.AddItem(item, 1);
                }
                InventoryViewModel inventorySlotViewModel = new InventoryViewModel(inventoryModel, equipment);
            }
            inventoryModel.LogInv();
        }
    }
}

