namespace Nut
{
    internal static class Parameters
    {
        internal const long NumberLimit = 1000000000000;
    }
    public static class Language
    {
        internal const string Default = "en";
        public const string English = "en";
        public const string French = "fr";
        public const string Russian = "ru";
        public const string Spanish = "es";
        public const string Turkish = "tr";
        // ISO 639-1, which is what CultureInfo.TwoLetterISOLanguageName hands you. These two
        // used to be the ccTLDs "ua" and "by"; both are still accepted.
        public const string Ukrainian = "uk";
        public const string Bulgarian = "bg";
        public const string Amharic = "am";
        public const string Polish = "pl";
        public const string Belarusian = "be";
        public const string Portuguese = "pt";
        public const string German = "de";
        public const string Uzbek = "uz";
        public const string Latvian = "lv";
    }

    public static class Culture
    {
        internal const string Default = "en-US";
        public const string EnglishUS = "en-US";
        public const string EnglishGB = "en-GB";
        public const string French = "fr-FR";
        public const string Russian = "ru-RU";
        public const string Spanish = "es-ES";
        public const string Turkish = "tr-TR";
        public const string Ukrainian = "uk-UA";
        public const string Bulgarian = "bg-BG";
        public const string EthiopianAM = "am-ET";
        public const string Polish = "pl-PL";
        // "by-BY" is not a culture .NET knows; the name is be-BY. Still accepted.
        public const string Belarusian = "be-BY";
        public const string PortugueseBR = "pt-BR";
        public const string GermanDE = "de-DE";
        public const string Uzbek = "uz-UZ";
        public const string Latvian = "lv-LV";
    }

    public static class Currency
    {
        internal const string TL = "tl";
        public const string EUR = "eur";
        public const string RUB = "rub";
        /// <summary>Widely used alias for the Russian ruble; treated as <see cref="RUB"/>.</summary>
        public const string RUR = "rur";
        public const string TRY = "try";
        public const string USD = "usd";
        public const string UAH = "uah";
        public const string BGN = "bgn";
        public const string ETB = "etb";
        public const string PLN = "pln";
        public const string BYN = "byn";
        public const string ARS = "ars";
        public const string BRL = "brl";
        public const string UZS = "uzs";
        public const string GBP = "gbp";
    }
}
