using System;

namespace TandC.RpgAdventure.Core.Player
{
    [Serializable]
    public class CharacterAttributes
    {
        public int Strength;
        public int Agility;
        public int Intelligence;
        public int Charisma;
        public int Luck;
        public int Perception;

        public int Health;
        public int Stamina;

        public CharacterAttributes()
        {
        }

        public CharacterAttributes(int strength, int agility, int intelligence, int charisma, int luck, int perception, int health, int stamina)
        {
            Strength = strength;
            Agility = agility;
            Intelligence = intelligence;
            Charisma = charisma;
            Luck = luck;
            Perception = perception;
            Health = health;
            Stamina = stamina;
        }

        public CharacterAttributes Clone()
        {
            return (CharacterAttributes)MemberwiseClone();
        }

        public static CharacterAttributes operator +(CharacterAttributes a, CharacterAttributes b)
        {
            return new CharacterAttributes(
        a.Strength + b.Strength,
        a.Agility + b.Agility,
        a.Intelligence + b.Intelligence,
        a.Charisma + b.Charisma,
        a.Luck + b.Luck,
        a.Perception + b.Perception,
        a.Health + b.Health,
        a.Stamina + b.Stamina
    );
        }
    }
}

