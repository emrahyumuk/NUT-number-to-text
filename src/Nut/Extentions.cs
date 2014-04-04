using System;
using Nut.Constants;
using Nut.TextConverters;

namespace Nut {
    public static class Extentions {

        public static string ToText(this long num, string lang = Language.Default) 
        {
            string text = null;
            switch (lang) 
            {
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

        public static string ToText(this decimal num, string currency, string lang = Language.Default) 
        {
            string text = null;
            switch (lang) {
                case Language.English:
                    text = EnglishConverter.Instance.ToText(num, currency);
                    break;
                case Language.French:
                    text = FrenchConverter.Instance.ToText(num, currency);
                    break;
                case Language.Russian:
                    text = RussianConverter.Instance.ToText(num, currency);
                    break;
                case Language.Spanish:
                    text = SpanishConverter.Instance.ToText(num, currency);
                    break;
                case Language.Turkish:
                    text = TurkishConverter.Instance.ToText(num, currency);
                    break;
            }
            return text;
        }

        public static string ToText(this int num, string lang = Language.Default) 
        {
            return ToText(Convert.ToInt64(num), lang);
        }

        public static string ToText(this int num, string currency, string lang) 
        {
            return ToText(Convert.ToDecimal(num), currency, lang);
        }
    }
}
