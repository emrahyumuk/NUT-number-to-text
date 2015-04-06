using System;
using System.Globalization;
using System.Linq;
using Nut.Models;
using Nut.TextConverters;

namespace Nut {
    public static class Extentions {

        public static string ToText(this long num, string lang = Language.Default) {
            var text = string.Empty;
            switch (lang) {
                case Language.English:
                    text = EnglishConverter.Instance.ToText(num);
                    break;
                case Language.French:
                    text = SpanishConverter.Instance.ToText(num);
                    break;
                case Language.Russian:
                    text = RussianConverter.Instance.ToText(num);
                    break;
                case Language.Spanish:
                    text = SpanishConverter.Instance.ToText(num);
                    break;
                case Language.Turkish:
                    text = TurkishConverter.Instance.ToText(num);
                    break;
            }
            return text;
        }

        public static string ToText(this decimal num, string currency, string lang = Language.Default, Configuration configuration = null) {
            configuration = configuration ?? new Configuration();
            var text = string.Empty;
            switch (lang) {
                case Language.English:
                    text = EnglishConverter.Instance.ToText(num, currency, configuration);
                    break;
                case Language.French:
                    text = FrenchConverter.Instance.ToText(num, currency, configuration);
                    break;
                case Language.Russian:
                    text = RussianConverter.Instance.ToText(num, currency, configuration);
                    break;
                case Language.Spanish:
                    text = SpanishConverter.Instance.ToText(num, currency, configuration);
                    break;
                case Language.Turkish:
                    text = TurkishConverter.Instance.ToText(num, currency, configuration);
                    break;
            }
            return text;
        }

        public static string ToText(this int num, string lang = Language.Default) {
            return ToText(Convert.ToInt64(num), lang.ToLower());
        }

        public static string ToText(this int num, string currency, string lang) {
            return ToText(Convert.ToDecimal(num), currency.ToLower(), lang.ToLower());
        }

        internal static string ToFirstLetterUpper(this string text, string culture = null) {
            var cultureInfo = string.IsNullOrEmpty(culture) ? CultureInfo.InvariantCulture : new CultureInfo(culture);
            return text.First().ToString().ToUpper(cultureInfo) + text.Substring(1);
        }

    }
}
