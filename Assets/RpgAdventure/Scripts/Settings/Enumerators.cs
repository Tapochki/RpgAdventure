namespace TandC.RpgAdventure.Settings 
{
    public enum TileType
    {
        Land, 
        Water,
        Sand,
        Mountain,
    }

    public enum StructureTileType 
    {
        None,
        City,
        Volcano,
        UniqueQuestionMark,
        UniqueExclamationMark,
        GoldQuestionMark,
        GoldExclamationMark,
        DefaultQuestionMark,
        DefaultExclamationMark,
    }

    public enum RaceType 
    {
        Human
    }

    public enum ClassType 
    {

    }

    public enum Languages // value of enum you can see in SystemLanguage class
    {
        Ukrainian = 38,
        Russian = 30,
        English = 10,
    }

    public enum CacheType
    {
        MapData,
        AppSettingsData,
        PurchaseData,
    }
}
