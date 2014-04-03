using System;
using Nut.TextConverters;

namespace Nut {
    public static class Extentions {

        public static string ToText(this long num, Language lang = Language.en) 
        {
            string text = null;
            switch (lang) 
            {
                case Language.en:
                    text = EnglishConverter.Instance.ToText(num);
                    break;
                case Language.ru:
                    text = RussianConverter.Instance.ToText(num);
                    break;
                case Language.es:
                    text = SpanishConverter.Instance.ToText(num);
                    break;
                case Language.tr:
                    text = TurkishConverter.Instance.ToText(num);
                    break;
            }
            return text;
        }

        public static string ToText(this int num, Language lang = Language.en) 
        {
            return ToText(Convert.ToInt64(num), lang);
        }

        public static string ToText(this long num, string lang) 
        {
            return string.IsNullOrEmpty(lang) ? ToText(num) : ToText(num, (Language)Enum.Parse(typeof(Language), lang.ToLower()));
        }

        public static string ToText(this int num, string lang) 
        {
            return string.IsNullOrEmpty(lang) ? ToText(num) : ToText(num, (Language)Enum.Parse(typeof(Language), lang.ToLower()));
        }

        public static string ToText(this decimal num, Currency currency, Language lang = Language.en) 
        {
            string text = null;
            switch (lang) {
                case Language.en:
                    text = EnglishConverter.Instance.ToText(num, currency);
                    break;
                case Language.ru:
                    text = RussianConverter.Instance.ToText(num, currency);
                    break;
                case Language.es:
                    text = SpanishConverter.Instance.ToText(num, currency);
                    break;
                case Language.tr:
                    text = TurkishConverter.Instance.ToText(num, currency);
                    break;
            }
            return text;
        }

        public static string ToText(this int num, Currency currency, Language lang = Language.en) 
        {
            return ToText(Convert.ToDecimal(num), currency, lang);
        }

        public static string ToText(this decimal num, string currency, string lang) 
        {
            if (currency == null) return null;
            if (currency == "try") currency = "tl";
            var currencyEnum = (Currency)Enum.Parse(typeof(Currency), currency.ToLower());
            var langEnum = (Language)Enum.Parse(typeof(Language), lang.ToLower());
            return string.IsNullOrEmpty(lang)
                ? ToText(num, currencyEnum)
                : ToText(num, currencyEnum, langEnum);
        }

        public static string ToText(this int num, string currency, string lang) 
        {
            return ToText(Convert.ToDecimal(num), currency, lang);
        }

    }
}
