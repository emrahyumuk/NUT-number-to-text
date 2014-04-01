using System;
using System.Runtime.CompilerServices;
using Nut.TextConverters;

namespace Nut {
    public static class Extentions {

        public static string ToText(this long num, string lang) 
        {
            return string.IsNullOrEmpty(lang) ? ToText(num) : ToText(num, (Language)Enum.Parse(typeof(Language), lang.ToLower()));
        }

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

        public static string ToTextWithCurrency(this decimal num, string currency, string lang) 
        {
            if (currency == null) return null;
            if (currency == "try") currency = "tl";
            var currencyEnum = (Currency)Enum.Parse(typeof(Currency), currency.ToLower());
            var langEnum = (Language)Enum.Parse(typeof(Language), lang.ToLower());
            return string.IsNullOrEmpty(lang)
                ? ToTextWithCurrency(num, currencyEnum)
                : ToTextWithCurrency(num, currencyEnum, langEnum);
        }

        public static string ToTextWithCurrency(this decimal num, Currency currency, Language lang = Language.en) 
        {
            string text = null;
            switch (lang) {
                case Language.en:
                    text = EnglishConverter.Instance.ToTextWithCurrency(num, currency);
                    break;
                case Language.ru:
                    text = RussianConverter.Instance.ToTextWithCurrency(num, currency);
                    break;
                case Language.es:
                    text = SpanishConverter.Instance.ToTextWithCurrency(num, currency);
                    break;
                case Language.tr:
                    text = TurkishConverter.Instance.ToTextWithCurrency(num, currency);
                    break;
            }
            return text;
        }

    }
}
