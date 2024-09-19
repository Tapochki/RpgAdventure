using System;
using System.Linq;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Settings;
using UnityEngine;

namespace TandC.RpgAdventure.Config.Player
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "TandC/Game/Configs/CharacterConfig", order = 3)]
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField]
        private CharacterRaceData[] _charactersRace;

        public CharacterRaceData GetRaceData(RaceType raceType)
        {
            return _charactersRace.FirstOrDefault(race => race.characterRace == raceType);
        }
    }
     
    [Serializable]
    public class CharacterRaceData
    {
        public RaceType characterRace;
        public GameObject characterObject;
        public CharacterClassData[] aviableCharacterClass;
        public CharacterClassData GetClassData(ClassType classType)
        {
            return aviableCharacterClass.FirstOrDefault(c => c.characterClass == classType);
        }
    }

    [Serializable]
    public class CharacterClassData
    {
        public ClassType characterClass;
        public CharacterAttributes baseAttributes;
        public Sprite portrait;
        public int[] startPlayerItemsId;
    }
}
