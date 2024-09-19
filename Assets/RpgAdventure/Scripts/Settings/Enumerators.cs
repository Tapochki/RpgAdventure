namespace TandC.RpgAdventure.Settings 
{
    public enum TileType
    {
        None = 0,
        Land = 1, 
        Water = 2,
        Sand = 3,
        Mountain = 4,
    }

    public enum StructureTileType 
    {
        None = 0,
        City = 1,
        Volcano = 2,
        UniqueQuestionMark = 3,
        UniqueExclamationMark = 4,
        GoldQuestionMark = 5,
        GoldExclamationMark = 6,
        DefaultQuestionMark = 7,
        DefaultExclamationMark = 8,
    }

    public enum RaceType 
    {
        None = 0,
        Human = 1,
        Elf = 2
    }

    public enum ClassType 
    {
        None = 0,
        Warrior = 1,
        Hunter = 2,
    }

    public enum Languages // value of enum you can see in SystemLanguage class
    {
        None = 0,
        Ukrainian = 38,
        Russian = 30,
        English = 10,
    }

    public enum CacheType
    {
        None = 0,
        MapData = 1,
        AppSettingsData = 2,
        PurchaseData = 3,
    }

    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Accessory = 3,
        Consumable = 4,
        Companion = 5,
        Miscellaneous = 6,
    }

    public enum ItemRariryType
    {
        None = 0,
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5,
    }

    public enum EquipmentSlot
    {
        None = 0,
        Head = 1,
        Chest = 2,
        Legs = 3,
        Feet = 4,
        LeftHand = 5,
        RightHand = 6,
        BothHands = 7,
        Neck = 8,
        AccessoryLeftHand = 9,
        AccessoryRightHand = 10,
        Companion = 11,
    }

    public enum ConsumableItemEffect 
    {
        None = 0,
        Healing = 1,
        RecoverStamina = 2,
    }

    public enum EffectType 
    {
        None = 0,
        Heal = 1,
        StaminaRecovery = 2,
    }
}
