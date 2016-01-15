using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters {
    public sealed class UkrainianConverter : BaseConverter {

        private static readonly Lazy<UkrainianConverter> Lazy = new Lazy<UkrainianConverter>(() => new UkrainianConverter());
        public static UkrainianConverter Instance { get { return Lazy.Value; } }

        public override string CultureName {
            get { return "uk-UA"; }
        }

        public UkrainianConverter () {
            Initialize();
        }

        protected override long Append (long num, long scale, StringBuilder builder) {
            if (num > scale - 1) {
                var baseScale = num / scale;

                var textType = GetTextType(baseScale);
                if ((scale == 1000000 || scale == 1000000000) && textType < 3 && (baseScale == 1 || baseScale == 2)) {
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
                        builder.AppendFormat("{0} ", AdditionalScales[scale]);
                        break;
                    default:
                        builder.AppendFormat("{0} ", Scales[scale]);
                        break;
                }

                num = num - (baseScale * scale);
            }
            return num;
        }

        private void AppendLessThanOneThousandForAdditional (long num, StringBuilder builder) {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnitsForAdditional(num, builder);
        }

        protected override long AppendHundreds (long num, StringBuilder builder) {
            if (num > 99) {
                var hundreds = num / 100 * 100;
                builder.AppendFormat("{0} ", TextStrings[hundreds]);
                num = num - hundreds;
            }
            return num;
        }

        private byte GetTextType (long num) {
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

        private void Initialize () {
            TextStrings.Add(0, "Нуль");
            TextStrings.Add(1, "Одна");
            TextStrings.Add(2, "Дві");
            TextStrings.Add(3, "Три");
            TextStrings.Add(4, "Чотири");
            TextStrings.Add(5, "П'ять");
            TextStrings.Add(6, "Шість");
            TextStrings.Add(7, "Сім");
            TextStrings.Add(8, "Вісім");
            TextStrings.Add(9, "Дев'ять");
            TextStrings.Add(10, "Десять");
            TextStrings.Add(11, "Одинадцять");
            TextStrings.Add(12, "Дванадцять");
            TextStrings.Add(13, "Тринадцять");
            TextStrings.Add(14, "Чотирнадцять");
            TextStrings.Add(15, "П'ятнадцять");
            TextStrings.Add(16, "Шістнадцять");
            TextStrings.Add(17, "Сімнадцять");
            TextStrings.Add(18, "Вісімнадцять");
            TextStrings.Add(19, "Дев'ятнадцять");
            TextStrings.Add(20, "Двадцять");
            TextStrings.Add(30, "Тридцять");
            TextStrings.Add(40, "Сорок");
            TextStrings.Add(50, "П'ятдесят");
            TextStrings.Add(60, "Шістдесят");
            TextStrings.Add(70, "Сімдесят");
            TextStrings.Add(80, "Вісімдесят");
            TextStrings.Add(90, "Дев'яносто");
            TextStrings.Add(100, "Сто");
            TextStrings.Add(200, "Двісті");
            TextStrings.Add(300, "Триста");
            TextStrings.Add(400, "Чотириста");
            TextStrings.Add(500, "П'ятсот");
            TextStrings.Add(600, "Шістсот");
            TextStrings.Add(700, "Сімсот");
            TextStrings.Add(800, "Вісімсот");
            TextStrings.Add(900, "Дев'ятсот");

            AdditionalStrings.Add(1, "Один");
            AdditionalStrings.Add(2, "Два");
            AdditionalStrings.Add(1000, "тисяча");
            AdditionalStrings.Add(1000000, "мільйон");
            AdditionalStrings.Add(1000000000, "мільярд");

            Scales.Add(1000000000, "мільярдів");
            Scales.Add(1000000, "мільйонів");
            Scales.Add(1000, "тисяч");

            AdditionalScales.Add(1000000000, "мільярди");
            AdditionalScales.Add(1000000, "мільйони");
            AdditionalScales.Add(1000, "тисячі");
        }

        #region Currency

        protected override string GetCurrencyText (long num, CurrencyModel currency) {
            var textType = GetTextType(num);
            return currency.Names[textType - 1];
        }

        protected override string GetSubUnitCurrencyText (long num, CurrencyModel currency) {
            var textType = GetTextType(num);
            return currency.SubUnitCurrency.Names[textType - 1];
        }

        protected override CurrencyModel GetCurrencyModel (string currency) {
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
                case Currency.UAH:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] { "гривня", "гривні", "гривень" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копійка", "копійки", "копійок" } }
                    };
            }
            return null;
        }

        #endregion
    }
}
