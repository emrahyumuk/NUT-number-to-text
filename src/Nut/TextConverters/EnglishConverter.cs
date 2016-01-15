using System;
using Nut.Models;

namespace Nut.TextConverters {
    public sealed class EnglishConverter : BaseConverter {

        private static readonly Lazy<EnglishConverter> Lazy = new Lazy<EnglishConverter>(() => new EnglishConverter());
        public static EnglishConverter Instance { get { return Lazy.Value; } }

        public override string CultureName {
            get { return "en-US"; }
        }

        public EnglishConverter() {
            Initialize();
        }

        private void Initialize() {
            TextStrings.Add(0, "zero");
            TextStrings.Add(1, "one");
            TextStrings.Add(2, "two");
            TextStrings.Add(3, "three");
            TextStrings.Add(4, "four");
            TextStrings.Add(5, "five");
            TextStrings.Add(6, "six");
            TextStrings.Add(7, "seven");
            TextStrings.Add(8, "eight");
            TextStrings.Add(9, "nine");
            TextStrings.Add(10, "ten");
            TextStrings.Add(11, "eleven");
            TextStrings.Add(12, "twelve");
            TextStrings.Add(13, "thirteen");
            TextStrings.Add(14, "fourteen");
            TextStrings.Add(15, "fifteen");
            TextStrings.Add(16, "sixteen");
            TextStrings.Add(17, "seventeen");
            TextStrings.Add(18, "eighteen");
            TextStrings.Add(19, "nineteen");
            TextStrings.Add(20, "twenty");
            TextStrings.Add(30, "thirty");
            TextStrings.Add(40, "forty");
            TextStrings.Add(50, "fifty");
            TextStrings.Add(60, "sixty");
            TextStrings.Add(70, "seventy");
            TextStrings.Add(80, "eighty");
            TextStrings.Add(90, "ninety");
            TextStrings.Add(100, "hundred");

            Scales.Add(1000000000, "billion");
            Scales.Add(1000000, "million");
            Scales.Add(1000, "thousand");
        }

        protected override CurrencyModel GetCurrencyModel(string currency) {
            switch (currency) {
                case Currency.EUR:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "euro", "euros" },
                        SubUnitCurrency =  new BaseCurrencyModel { Names = new[] { "eurocent", "eurocents" } }
                    };
                case Currency.USD:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "dollar", "dollars" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "cent", "cents" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "ruble", "rubles" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "turkish lira", "turkish lira" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kurus", "kurus" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "ukrainian hryvnia", "ukrainian hryvnia" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiyka", "kopiyky", "kopiyok" } }
                    };
            }
            return null;
        }
    }
}