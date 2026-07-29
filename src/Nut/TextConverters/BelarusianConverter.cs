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

        protected override string NegativeSign => "мінус";

        public BelarusianConverter()
        {
            Initialize();
        }

        // Set only on the short-lived per-conversion instance; null on the shared singleton.
        private string _one;
        private string _two;

        private BelarusianConverter(BelarusianConverter template, CurrencyModel currencyModel, bool isMainUnit)
            : base(template)
        {
            var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
            var feminine = gender == GenderGroup.Feminine;
            _one = feminine ? "адна" : "адзін";
            _two = feminine ? "дзве" : "два";
        }

        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
        {
            // Gendered forms are per-conversion; see RussianConverter for why this is not
            // written into the shared singleton.
            return new BelarusianConverter(this, currencyModel, isMainUnit).ToText(num);
        }

        protected override void AppendUnits(long num, StringBuilder builder)
        {
            if (!AppendGendered(num, builder)) base.AppendUnits(num, builder);
        }

        // Append only routes here for scale 1000, and the count before тысяча agrees with
        // тысяча, which is feminine whatever currency is being counted: "сорак адна
        // тысяча", not "сорак адзін тысяча". Entry [1] holds the feminine form.
        protected override void AppendUnitsForAdditional(long num, StringBuilder builder)
        {
            if (num == 1 || num == 2) builder.AppendFormat("{0} ", NumberTexts[num][1]);
            else base.AppendUnitsForAdditional(num, builder);
        }

        private bool AppendGendered(long num, StringBuilder builder)
        {
            var word = num == 1 ? _one : num == 2 ? _two : null;
            if (word == null) return false;
            builder.AppendFormat("{0} ", word);
            return true;
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
                    AppendLessThanOneThousandForScale(baseScale, builder);
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

        // мільён and мільярд are masculine, so their count must not pick up the currency's
        // gender the way the trailing unit does: "адзін мільён грыўняў", not "адна мільён".
        private void AppendLessThanOneThousandForScale(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            base.AppendUnits(num, builder);
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

        protected override string GetCurrencyText(long num, CurrencyModel currency)
        {
            var textType = GetTextType(num);
            return currency.Names[textType - 1];
        }

        protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency)
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
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "евроцэнт", "евроцэнта", "евроцэнтаў" } }
                    };
                case Currency.GBP:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "фунт стэрлінгаў", "фунты стэрлінгаў", "фунтаў стэрлінгаў" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "пені", "пені", "пені" } }
                  };
                case Currency.ARS:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "аргентынскае песа", "аргентынскіх песа", "аргентынскіх песа" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентава", "сентава", "сентава" } }
                  };
                case Currency.BGN:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "леў", "левы", "леваў" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "стацінка", "стацінкі", "стацінак" } }
                  };
                case Currency.BRL:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "бразільскі рэал", "бразільскія рэалы", "бразільскіх рэалаў" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентава", "сентава", "сентава" } }
                  };
                case Currency.UZS:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "узбекскі сум", "узбекскія сумы", "узбекскіх сумаў" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "тыйін", "тыйіны", "тыйінаў" } }
                  };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "даляр", "даляра", "даляраў" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "цэнт", "цэнта", "цэнтаў" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "рубель", "рубля", "рублёў" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "турэцкая ліра", "турэцкіх лір", "турэцкіх лір" },
                        Gender = GenderGroup.Feminine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "куруш", "курушы", "курушей" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "грыўна", "грыўны", "грыўняў" },
                        Gender = GenderGroup.Feminine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "быр", "быр", "быр" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "злоты", "злотых", "злотых" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "грош", "гроша", "грошаў" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "беларускі рубель", "беларускіх рубля", "беларускіх рублёў" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "капейка", "капейкі", "капеек" } }
                    };
            }
            return null;
        }

        #endregion
    }
}
