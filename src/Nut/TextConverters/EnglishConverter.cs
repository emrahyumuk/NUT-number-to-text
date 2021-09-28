using System;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class EnglishConverter : BaseConverter
    {

        private static readonly Lazy<EnglishConverter> Lazy = new Lazy<EnglishConverter>(() => new EnglishConverter());
        public static EnglishConverter Instance => Lazy.Value;

        public override string CultureName => Culture.EnglishUS;

        public EnglishConverter()
        {
            Initialize();
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "zero" });
            NumberTexts.Add(1, new[] { "one" });
            NumberTexts.Add(2, new[] { "two" });
            NumberTexts.Add(3, new[] { "three" });
            NumberTexts.Add(4, new[] { "four" });
            NumberTexts.Add(5, new[] { "five" });
            NumberTexts.Add(6, new[] { "six" });
            NumberTexts.Add(7, new[] { "seven" });
            NumberTexts.Add(8, new[] { "eight" });
            NumberTexts.Add(9, new[] { "nine" });
            NumberTexts.Add(10, new[] { "ten" });
            NumberTexts.Add(11, new[] { "eleven" });
            NumberTexts.Add(12, new[] { "twelve" });
            NumberTexts.Add(13, new[] { "thirteen" });
            NumberTexts.Add(14, new[] { "fourteen" });
            NumberTexts.Add(15, new[] { "fifteen" });
            NumberTexts.Add(16, new[] { "sixteen" });
            NumberTexts.Add(17, new[] { "seventeen" });
            NumberTexts.Add(18, new[] { "eighteen" });
            NumberTexts.Add(19, new[] { "nineteen" });
            NumberTexts.Add(20, new[] { "twenty" });
            NumberTexts.Add(30, new[] { "thirty" });
            NumberTexts.Add(40, new[] { "forty" });
            NumberTexts.Add(50, new[] { "fifty" });
            NumberTexts.Add(60, new[] { "sixty" });
            NumberTexts.Add(70, new[] { "seventy" });
            NumberTexts.Add(80, new[] { "eighty" });
            NumberTexts.Add(90, new[] { "ninety" });
            NumberTexts.Add(100, new[] { "hundred" });

            ScaleTexts.Add(1000000000, new[] { "billion" });
            ScaleTexts.Add(1000000, new[] { "million" });
            ScaleTexts.Add(1000, new[] { "thousand" });
        }

        protected override CurrencyModel GetCurrencyModel(string currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "euro", "euros" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "eurocent", "eurocents" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "dollar", "dollars" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "cent", "cents" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ruble", "rubles" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "turkish lira", "turkish lira" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kurus", "kurus" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ukrainian hryvnia", "ukrainian hryvnia" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiyka", "kopiyky", "kopiyok" } }
                    };

                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "birr", "birr" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "cent", "cents" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "zloty", "zloty" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "grosz", "grosze", "groszy" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Belarusian ruble", "Belarusian rubles" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
            }
            return null;
        }

        protected override string GetUnitSeparator(CurrencyModel currency)
        {
            return " and ";
        }
    }
}