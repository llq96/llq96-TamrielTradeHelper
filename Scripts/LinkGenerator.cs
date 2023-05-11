namespace TamrielTradeApp
{
    public static class LinkGenerator
    {
        private static readonly string startURL =
            "https://eu.tamrieltradecentre.com/pc/Trade/SearchResult?SearchType=Sell";

        private static readonly string linkPart_langRU = "&lang=ru-RU";

        //static readonly Dictionary<string, string> traitsDict = new() {
        //    { "Любая особенность", "" },
        //    { "Бодрость", "12" },
        //    { "Воодушевление", "7" },
        //    { "Гармония", "22" },
        //    { "Жажда крови", "21" },
        //    { "Защита", "23" },
        //    { "Крепкое здоровье", "18" },
        //    { "Магический потенциал", "17" },
        //    { "Мастерство", "15" },
        //    { "Насыщенность", "3" },
        //    { "Непробиваемость", "9" },
        //    { "Неутомимость", "19" },
        //    { "Оберег", "4" },
        //    { "Острота", "6" },
        //    { "Помощь богов", "13" },
        //    { "Прочность", "10" },
        //    { "Развитие", "5" },
        //    { "Сила нирна", "14" },
        //    { "Сила стихий", "1" },
        //    { "Спасение", "0" },
        //    { "Стойкость", "8" },
        //    { "Точность", "2" },
        //    { "Триединство", "25" },
        //    { "Удобство", "11" },
        //    { "Ускорение", "24" },
        //    { "Ценность", "16" },
        //    { "Aggressive", "26" },
        //    { "Augmented", "27" },
        //    { "Bolstered", "28" },
        //    { "Focused", "29" },
        //    { "Prolific", "30" },
        //    { "Quickened", "31" },
        //    { "Shattering", "32" },
        //    { "Soothing", "33" },
        //    { "Special", "20" },
        //    { "Vigorous", "34" }
        //};

        public static string GetURL(FullInfo info) =>
            GetURL(info.BaseInfo, info.LevelInfo, info.AmountInfo, info.PriceInfo);

        public static string GetURL(BaseInfo baseInfo, LevelInfo levelInfo, AmountInfo amountInfo, PriceInfo priceInfo)
        {
            string linkPart_ItemNamePattern = "", linkPart_ItemQualityID = "";
            string linkPart_LevelMin = "", linkPart_LevelMax = "", linkPart_IsChampionPoint = "";
            string linkPart_AmountMin = "", linkPart_AmountMax = "";
            string linkPart_PriceMin = "", linkPart_PriceMax = "";

            linkPart_ItemNamePattern = $"&ItemNamePattern={baseInfo.NamePattern}";
            linkPart_ItemQualityID =
                (baseInfo.Quality == ItemQuality.Any) ? "" : $"&ItemQualityID={(int)baseInfo.Quality}";

            if (levelInfo.IsActiveRestrictions)
            {
                linkPart_IsChampionPoint = $"&IsChampionPoint={(levelInfo.IsChampionPointsType ? "true" : "false")}";
                linkPart_LevelMin = $"&LevelMin={levelInfo.LevelMin}";
                linkPart_LevelMax = $"&LevelMax={levelInfo.LevelMax}";
            }

            if (amountInfo.IsActiveRestrictions)
            {
                linkPart_AmountMin = $"&AmountMin={amountInfo.AmountMin}";
                linkPart_AmountMax = $"&AmountMax={amountInfo.AmountMax}";
            }

            if (priceInfo.IsActiveRestrictions)
            {
                linkPart_PriceMin = $"&PriceMin={priceInfo.PriceMin}";
                linkPart_PriceMax = $"&PriceMax={priceInfo.PriceMax}";
            }

            string result = startURL + linkPart_ItemNamePattern + linkPart_ItemQualityID +
                            linkPart_IsChampionPoint + linkPart_LevelMin + linkPart_LevelMax +
                            linkPart_AmountMin + linkPart_AmountMax +
                            linkPart_PriceMin + linkPart_PriceMax + linkPart_langRU;
            return result;
        }


        public class FullInfo
        {
            public readonly BaseInfo BaseInfo;
            public readonly LevelInfo LevelInfo;
            public readonly AmountInfo AmountInfo;
            public readonly PriceInfo PriceInfo;

            public FullInfo(BaseInfo baseInfo, LevelInfo levelInfo, AmountInfo amountInfo, PriceInfo priceInfo)
            {
                BaseInfo = baseInfo;
                LevelInfo = levelInfo;
                AmountInfo = amountInfo;
                PriceInfo = priceInfo;
            }
        }


        public class BaseInfo
        {
            public readonly string NamePattern;
            public readonly ItemQuality Quality;

            public BaseInfo(string namePattern = "", ItemQuality quality = ItemQuality.Any)
            {
                NamePattern = namePattern;
                Quality = quality;
            }
        }


        public class LevelInfo
        {
            public readonly bool IsActiveRestrictions;
            public readonly LevelType Type;
            public readonly int LevelMin;
            public readonly int LevelMax;

            public LevelInfo(bool isActiveRestrictions = false, LevelType type = LevelType.ChampionPoints,
                int minLevel = 160, int maxLevel = 160)
            {
                IsActiveRestrictions = isActiveRestrictions;
                Type = type;
                LevelMin = minLevel;
                LevelMax = maxLevel;
            }

            public enum LevelType
            {
                Level,
                ChampionPoints
            }

            public bool IsChampionPointsType => Type == LevelType.ChampionPoints;
        }


        public class AmountInfo
        {
            public readonly bool IsActiveRestrictions;
            public readonly int AmountMin;
            public readonly int AmountMax;

            public AmountInfo(bool isActiveRestrictions = false, int amountMin = 1, int amountMax = 9999)
            {
                IsActiveRestrictions = isActiveRestrictions;
                AmountMin = amountMin;
                AmountMax = amountMax;
            }
        }


        public class PriceInfo
        {
            public readonly bool IsActiveRestrictions;
            public readonly int PriceMin;
            public readonly int PriceMax;

            public PriceInfo(bool isActiveRestrictions = false, int priceMin = 0, int priceMax = int.MaxValue)
            {
                IsActiveRestrictions = isActiveRestrictions;
                PriceMin = priceMin;
                PriceMax = priceMax;
            }
        }
    }


    public enum ItemQuality
    {
        /// <summary> Любое </summary>
        Any = -1,

        /// <summary> Обычное(серый) </summary>
        Q0_Normal = 0,

        /// <summary> Улучшенное(зелёный) </summary>
        Q1_Fine = 1,

        /// <summary> Превосходное(синий) </summary>
        Q2_Superior = 2,

        /// <summary> Эпическое(фиолетовый) </summary>
        Q3_Epic = 3,

        /// <summary> Легендарное(золотой) </summary>
        Q4_Legendary = 4,
    }
}