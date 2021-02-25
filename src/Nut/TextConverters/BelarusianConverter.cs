using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class BelarusianConverter : BaseConverter
    {

        private static readonly Lazy<BelarusianConverter> Lazy = new Lazy<BelarusianConverter>(() => new BelarusianConverter());
        public static BelarusianConverter Instance => Lazy.Value;

        public override string CultureName => Culture.Belarusian;

        public BelarusianConverter()
        {
            Initialize();
        }

        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
        {
            switch (currencyModel.Currency)
            {
                case Currency.BYN:
                case Currency.RUB:
                case Currency.EUR:
                    NumberTexts[1][0] = isMainUnit ? "адзін" : "адна";
                    NumberTexts[2][0] = isMainUnit ? "два" : "дзве";
                    break;
                case Currency.UAH:
                    NumberTexts[1][0] = "адна";
                    NumberTexts[2][0] = "дзве";
                    break;
                default:
                    NumberTexts[1][0] = "адзін";
                    NumberTexts[2][0] = "два";
                    break;
            }
            return ToText(num);
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;

                var textType = GetTextType(baseScale);
                var baseUnitNumber = baseScale % 10;
                if (scale == 1000 && textType < 3 && (baseUnitNumber == 1 || baseUnitNumber == 2))
                {
                    AppendLessThanOneThousandForAdditional(baseScale, builder);
                }
                else
                {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                switch (textType)
                {
                    case 1:
                        builder.AppendFormat("{0} ", ScaleTexts[scale][1]);
                        break;
                    case 2:
                        builder.AppendFormat("{0} ", ScaleTexts[scale][2]);
                        break;
                    default:
                        builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
                        break;
                }

                num = num - (baseScale * scale);
            }
            return num;
        }

        private void AppendLessThanOneThousandForAdditional(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnitsForAdditional(num, builder);
        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100 * 100;
                builder.AppendFormat("{0} ", NumberTexts[hundreds][0]);
                num = num - hundreds;
            }
            return num;
        }

        private byte GetTextType(long num)
        {
            const int femmeMinBaseScale = 2;
            const int pluralMinBaseScale = 5;

            var baseUnitNumber = num % 10;
            var baseTens = num % 100;

            if (baseTens < 10 || baseTens > 20)
            {
                if (baseUnitNumber == 1)
                    return 1;
                if (baseUnitNumber >= femmeMinBaseScale && baseUnitNumber < pluralMinBaseScale)
                    return 2;
            }
            return 3;
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "нуль" });
            NumberTexts.Add(1, new[] { "адзін", "адна" });
            NumberTexts.Add(2, new[] { "два", "дзве" });
            NumberTexts.Add(3, new[] { "тры" });
            NumberTexts.Add(4, new[] { "чатыры" });
            NumberTexts.Add(5, new[] { "пяць" });
            NumberTexts.Add(6, new[] { "шэсць" });
            NumberTexts.Add(7, new[] { "сем" });
            NumberTexts.Add(8, new[] { "восем" });
            NumberTexts.Add(9, new[] { "дзевяць" });
            NumberTexts.Add(10, new[] { "дзесяць" });
            NumberTexts.Add(11, new[] { "адзінаццаць" });
            NumberTexts.Add(12, new[] { "дванаццаць" });
            NumberTexts.Add(13, new[] { "трынаццаць" });
            NumberTexts.Add(14, new[] { "чатырнаццаць" });
            NumberTexts.Add(15, new[] { "пятнаццаць" });
            NumberTexts.Add(16, new[] { "шаснаццаць" });
            NumberTexts.Add(17, new[] { "семнаццаць" });
            NumberTexts.Add(18, new[] { "васемнаццаць" });
            NumberTexts.Add(19, new[] { "дзевятнаццаць" });
            NumberTexts.Add(20, new[] { "дваццаць" });
            NumberTexts.Add(30, new[] { "трыццаць" });
            NumberTexts.Add(40, new[] { "сорак" });
            NumberTexts.Add(50, new[] { "пяцьдзесят" });
            NumberTexts.Add(60, new[] { "шэсцьдзесят" });
            NumberTexts.Add(70, new[] { "семдзесят" });
            NumberTexts.Add(80, new[] { "восемдзесят" });
            NumberTexts.Add(90, new[] { "дзевяноста" });
            NumberTexts.Add(100, new[] { "сто" });
            NumberTexts.Add(200, new[] { "дзвесце" });
            NumberTexts.Add(300, new[] { "трыста" });
            NumberTexts.Add(400, new[] { "чатырыста" });
            NumberTexts.Add(500, new[] { "пяцьсот" });
            NumberTexts.Add(600, new[] { "шэсцьсот" });
            NumberTexts.Add(700, new[] { "сямсот" });
            NumberTexts.Add(800, new[] { "восемсот" });
            NumberTexts.Add(900, new[] { "дзевяцьсот" });

            ScaleTexts.Add(1000000000, new[] { "мільярдаў", "мільярд", "мільярда" });
            ScaleTexts.Add(1000000, new[] { "мільёнаў", "мільён", "мільёна" });
            ScaleTexts.Add(1000, new[] { "тысяч", "тысяча", "тысячы" });
        }

        #region Currency

        protected override string GetCurrencyText(long num, CurrencyModel currency, bool useShort)
        {
            var textType = GetTextType(num);
            return currency.Names[textType - 1];
        }

        protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency, bool useShort)
        {
            var textType = GetTextType(num);
            return currency.SubUnitCurrency.Names[textType - 1];
        }

        protected override CurrencyModel GetCurrencyModel(string currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "евро", "евро", "евро" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "евроцэнт", "евроцэнта", "евроцэнтаў" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "даляр", "даляра", "даляраў" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "цэнт", "цэнта", "цэнтаў" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "рубель", "рубля", "рублёў" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "турэцкая ліра", "турэцкіх лір", "турэцкіх лір" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "курушы", "курушей" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "грыўна", "грыўны", "грыўняў" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "быр", "быр", "быр" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "злоты", "злотых", "злотых" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "грош", "гроша", "грошаў" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "беларускі рубель", "беларускіх рубля", "беларускіх рублёў" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
            }
            return null;
        }

        #endregion
    }
}
