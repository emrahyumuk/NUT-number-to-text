using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters {
    public sealed class RussianConverter : BaseConverter {

        private static readonly Lazy<RussianConverter> Lazy = new Lazy<RussianConverter>(() => new RussianConverter());
        public static RussianConverter Instance { get { return Lazy.Value; } }

        public override string CultureName {
            get { return "ru-RU"; }
        }

        public RussianConverter() {
            Initialize();
        }

        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit) {
            switch (currencyModel.Currency) {
                case Currency.RUB:
                    TextStrings[2] = isMainUnit ? "два" : "две";
                    break;
                case Currency.EUR:
                    TextStrings[2] = isMainUnit ? "два" : "две";
                    break;
                default:
                    TextStrings[2] = "два";
                    break;
            }
            return ToText(num);
        }

        protected override long Append(long num, long scale, StringBuilder builder) {
            if (num > scale - 1) {
                var baseScale = num / scale;

                var textType = GetTextType(baseScale);
                var baseUnitNumber = baseScale % 10;
                if (scale == 1000 && textType < 3 && (baseUnitNumber == 1 || baseUnitNumber == 2)) {
                    AppendLessThanOneThousandForAdditional(baseScale, builder);
                }
                else {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                switch (textType) {
                    case 1:
                        builder.AppendFormat("{0} ", AdditionalStrings[scale]);
                        break;
                    case 2:
                        builder.AppendFormat("{0} ", AdditionalStrings[2 * scale]);
                        break;
                    default:
                        builder.AppendFormat("{0} ", Scales[scale]);
                        break;
                }

                num = num - (baseScale * scale);
            }
            return num;
        }

        private void AppendLessThanOneThousandForAdditional(long num, StringBuilder builder) {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnitsForAdditional(num, builder);
        }

        protected override long AppendHundreds(long num, StringBuilder builder) {
            if (num > 99) {
                var hundreds = num / 100 * 100;
                builder.AppendFormat("{0} ", TextStrings[hundreds]);
                num = num - hundreds;
            }
            return num;
        }

        private byte GetTextType(long num) {
            const int femmeMinBaseScale = 2;
            const int pluralMinBaseScale = 5;

            var baseUnitNumber = num % 10;
            var baseTens = num % 100;

            if (baseTens < 10 || baseTens > 20) {
                if (baseUnitNumber == 1)
                    return 1;
                if (baseUnitNumber >= femmeMinBaseScale && baseUnitNumber < pluralMinBaseScale)
                    return 2;
            }
            return 3;
        }

        private void Initialize() {
            TextStrings.Add(0, "ноль");
            TextStrings.Add(1, "один");
            TextStrings.Add(2, "два");
            TextStrings.Add(3, "три");
            TextStrings.Add(4, "четыре");
            TextStrings.Add(5, "пять");
            TextStrings.Add(6, "шесть");
            TextStrings.Add(7, "семь");
            TextStrings.Add(8, "восемь");
            TextStrings.Add(9, "девять");
            TextStrings.Add(10, "десять");
            TextStrings.Add(11, "одиннадцать");
            TextStrings.Add(12, "двенадцать");
            TextStrings.Add(13, "тринадцать");
            TextStrings.Add(14, "четырнадцать");
            TextStrings.Add(15, "пятнадцать");
            TextStrings.Add(16, "шестнадцать");
            TextStrings.Add(17, "семнадцать");
            TextStrings.Add(18, "восемнадцать");
            TextStrings.Add(19, "девятнадцать");
            TextStrings.Add(20, "двадцать");
            TextStrings.Add(30, "тридцать");
            TextStrings.Add(40, "сорок");
            TextStrings.Add(50, "пятьдесят");
            TextStrings.Add(60, "шестьдесят");
            TextStrings.Add(70, "семьдесят");
            TextStrings.Add(80, "восемьдесят");
            TextStrings.Add(90, "девяносто");
            TextStrings.Add(100, "сто");
            TextStrings.Add(200, "двести");
            TextStrings.Add(300, "триста");
            TextStrings.Add(400, "четыреста");
            TextStrings.Add(500, "пятьсот");
            TextStrings.Add(600, "шестьсот");
            TextStrings.Add(700, "семьсот");
            TextStrings.Add(800, "восемьсот");
            TextStrings.Add(900, "девятьсот");

            AdditionalStrings.Add(1, "одна");
            AdditionalStrings.Add(2, "две");
            AdditionalStrings.Add(1000, "тысяча");
            AdditionalStrings.Add(2000, "тысячи");
            AdditionalStrings.Add(1000000, "миллион");
            AdditionalStrings.Add(2000000, "миллиона");
            AdditionalStrings.Add(1000000000, "миллиард");
            AdditionalStrings.Add(2000000000, "миллиарда");

            Scales.Add(1000000000, "миллиардов");
            Scales.Add(1000000, "миллионов");
            Scales.Add(1000, "тысяч");
        }

        #region Currency

        protected override string GetCurrencyText(long num, CurrencyModel currency) {
            var textType = GetTextType(num);
            return currency.Names[textType - 1];
        }

        protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency) {
            var textType = GetTextType(num);
            return currency.SubUnitCurrency.Names[textType - 1];
        }

        protected override CurrencyModel GetCurrencyModel(string currency) {
            switch (currency) {
                case Currency.EUR:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "евро", "евро", "евро" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "евроцент", "евроцента", "евроцентов" } }
                    };
                case Currency.USD:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "доллар", "доллара", "долларов" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "цент", "цента", "центов" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "рубль", "рубля", "рублей" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "турецкая лира", "турецких лир", "турецких лир" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруша", "курушей" } }
                    };
            }
            return null;
        }

        #endregion
    }
}
