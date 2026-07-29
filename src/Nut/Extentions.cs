using Nut.TextConverters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nut
{
    public static class Extentions
    {
        /// <summary>
        /// Language and culture codes to converters. Matching ignores case, so "en-US",
        /// "en-us" and "EN-US" all resolve — previously the int overloads lowercased the
        /// argument before comparing it against the mixed-case Culture constants, so every
        /// culture code silently produced an empty string on those overloads while working
        /// on the others.
        /// </summary>
        private static readonly Dictionary<string, BaseConverter> Converters =
            new Dictionary<string, BaseConverter>(StringComparer.OrdinalIgnoreCase)
            {
                { Language.English, EnglishConverter.Instance },
                { Culture.EnglishUS, EnglishConverter.Instance },
                { Culture.EnglishGB, EnglishConverter.Instance },
                { Language.French, FrenchConverter.Instance },
                { Culture.French, FrenchConverter.Instance },
                { Language.German, GermanConverter.Instance },
                { Culture.GermanDE, GermanConverter.Instance },
                { Language.Spanish, SpanishConverter.Instance },
                { Culture.Spanish, SpanishConverter.Instance },
                { Language.Portuguese, PortugueseConverter.Instance },
                { Culture.PortugueseBR, PortugueseConverter.Instance },
                { Language.Turkish, TurkishConverter.Instance },
                { Culture.Turkish, TurkishConverter.Instance },
                { Language.Russian, RussianConverter.Instance },
                { Culture.Russian, RussianConverter.Instance },
                { Language.Ukrainian, UkrainianConverter.Instance },
                { Culture.Ukrainian, UkrainianConverter.Instance },
                { Language.Belarusian, BelarusianConverter.Instance },
                { Culture.Belarusian, BelarusianConverter.Instance },
                { Language.Bulgarian, BulgarianConverter.Instance },
                { Culture.Bulgarian, BulgarianConverter.Instance },
                { Language.Polish, PolishConverter.Instance },
                { Culture.Polish, PolishConverter.Instance },
                { Language.Uzbek, UzbekConverter.Instance },
                { Culture.Uzbek, UzbekConverter.Instance },
                { Language.Amharic, AmharicConverter.Instance },
                { Culture.EthiopianAM, AmharicConverter.Instance },
            };

        /// <summary>Returns null for an unknown language, which callers render as "".</summary>
        private static BaseConverter Resolve(string lang)
        {
            BaseConverter converter;
            return lang != null && Converters.TryGetValue(lang, out converter) ? converter : null;
        }

        public static string ToText(this long num, string lang = Language.Default, GenderGroup genderGroup = GenderGroup.None)
        {
            var converter = Resolve(lang);
            return converter == null ? string.Empty : converter.ToText(num, genderGroup);
        }

        public static string ToText(this decimal num, string currency, string lang = Language.Default, Options options = new Options(), GenderGroup genderGroup = GenderGroup.None)
        {
            var converter = Resolve(lang);
            return converter == null ? string.Empty : converter.ToText(num, currency, options, genderGroup);
        }

        public static string ToText(this int num, string lang = Language.Default, GenderGroup genderGroup = GenderGroup.None)
        {
            return ToText(Convert.ToInt64(num), lang, genderGroup);
        }

        public static string ToText(this int num, string currency, string lang)
        {
            return ToText(Convert.ToDecimal(num), currency, lang);
        }

        internal static string ToFirstLetterUpper(this string text, string culture = null)
        {
            var cultureInfo = string.IsNullOrEmpty(culture) ? CultureInfo.InvariantCulture : new CultureInfo(culture);
            return text.First().ToString().ToUpper(cultureInfo) + text.Substring(1);
        }

    }
}
