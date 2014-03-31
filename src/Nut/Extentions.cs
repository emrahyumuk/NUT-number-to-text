using System;
using Nut.TextConverters;

namespace Nut
{
    public static class Extentions
    {

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
    }
}
