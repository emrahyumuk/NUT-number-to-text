using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class BulgarianConverter : BaseConverter
    {
        private static Lazy<BulgarianConverter> Lazy = new Lazy<BulgarianConverter>(() => new BulgarianConverter());
        public static BulgarianConverter Instance => Lazy.Value;
        public override string CultureName { get; } = Culture.Bulgarian;

        protected override string NegativeSign => "минус";

        private int textType;

        public BulgarianConverter()
        {
            Initialize();
        }

        // Set only on the short-lived per-conversion instance; null on the shared singleton.
        private string _one;
        private string _two;

        private BulgarianConverter(BulgarianConverter template, CurrencyModel currencyModel, bool isMainUnit)
            : base(template)
        {
            var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
            // Bulgarian marks all three: един / една / едно, and neuter two matches the feminine.
            _one = gender == GenderGroup.Feminine ? "една"
                 : gender == GenderGroup.Neuter ? "едно"
                 : "един";
            _two = gender == GenderGroup.Masculine ? "два" : "две";
        }

        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
        {
            // Gendered forms are per-conversion; see RussianConverter for why this is not
            // written into the shared singleton. This also gives textType, which Append
            // rewrites as it walks the number, an instance of its own.
            return new BulgarianConverter(this, currencyModel, isMainUnit).ToText(num);
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;

                // Singular for exactly one of a scale, plural otherwise: хиляда/хиляди,
                // милион/милиона. The old expression tested "scale < 999", never true here
                // since the smallest scale is 1000, and fell back to a test on the whole
                // number that got millions wrong.
                textType = baseScale == 1 ? 0 : 1;

                var baseUnitNumber = baseScale % 10;

                if (scale == 1000 && baseUnitNumber == 1 && num > 10000)
                    textType = 3;

                if (scale == 1000 && textType < 3 && (baseUnitNumber == 1 || baseUnitNumber == 2))
                {
                    AppendLessThanOneThousandForAdditional(baseScale, builder);
                }
                else if (scale == 1000)
                {
                    // Still the thousands prefix; textType carries the feminine form here.
                    AppendLessThanOneThousand(baseScale, builder);
                }
                else
                {
                    AppendLessThanOneThousandForScale(baseScale, builder);
                }

                builder.AppendFormat("{0} ", ScaleTexts[scale][textType > 0 ? 1 : 0]);
                num = num - (baseScale * scale);
                textType = 0;
            }
            return base.Append(num, scale, builder);
        }

        protected override void AppendLessThanOneThousand(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnits(num, builder);
        }

        /// <summary>Bulgarian says "хиляда", not "една хиляда", so a leading one before
        /// the thousands word is dropped. That elision belongs to this path only — applying
        /// it to the trailing unit is what made 1 render as an empty string.</summary>
        private void AppendLessThanOneThousandForAdditional(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            if (num == 1 && builder.ToString().Trim().Length == 0) return;
            AppendUnits(num, builder);
        }

        /// <summary>милион and милиард are masculine and, unlike хиляда, are counted
        /// explicitly: "един милион", "два милиона".</summary>
        private void AppendLessThanOneThousandForScale(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            if (num > 0)
            {
                if (builder.ToString().Trim().Length > 0) builder.Append("и ");
                builder.AppendFormat("{0} ", NumberTexts[num][0]);
            }
        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                if (num % 100 == 0 && builder.ToString().Trim().Length > 0)
                    builder.Append("и ");
                var hundreds = num / 100 * 100;
                builder.AppendFormat("{0} ", NumberTexts[hundreds][0]);
                num = num - hundreds;
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20 && num % 10 == 0 && builder.ToString().Trim().Length > 0)
                builder.Append("и ");
            return base.AppendTens(num, builder);
        }

        protected override void AppendUnits(long num, StringBuilder builder)
        {
            if (num > 0)
            {
                if (builder.ToString().Trim().Length > 0)
                    builder.Append("и ");
                var targetType = Math.Min(NumberTexts[num].Length - 1, textType);
                // In the default slot the unit being counted decides the gender; the other
                // slots are picked by textType for the scale words and stay as they are.
                var byCurrency = targetType == 0 ? (num == 1 ? _one : num == 2 ? _two : null) : null;
                builder.AppendFormat("{0} ", byCurrency ?? NumberTexts[num][targetType]);
            }
        }

        protected override void AppendUnitsForAdditional(long num, StringBuilder builder)
        {
            if (num != 0 && builder.ToString().Trim().Length > 0)
                builder.Append("и ");
            // Same substitution as AppendUnits: entry 2's default form used to be
            // overwritten in the shared table per currency.
            if (num == 2 && _two != null) builder.AppendFormat("{0} ", _two);
            else base.AppendUnitsForAdditional(num, builder);
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "нула" });
            NumberTexts.Add(1, new[] { "един", "едно", "една" });
            NumberTexts.Add(2, new[] { "два", "две" });
            NumberTexts.Add(3, new[] { "три" });
            NumberTexts.Add(4, new[] { "четири" });
            NumberTexts.Add(5, new[] { "пет" });
            NumberTexts.Add(6, new[] { "шест" });
            NumberTexts.Add(7, new[] { "седем" });
            NumberTexts.Add(8, new[] { "осем" });
            NumberTexts.Add(9, new[] { "девет" });
            NumberTexts.Add(10, new[] { "десет" });
            NumberTexts.Add(11, new[] { "единадесет" });
            NumberTexts.Add(12, new[] { "дванадесет" });
            NumberTexts.Add(13, new[] { "тринадесет" });
            NumberTexts.Add(14, new[] { "четиринадесет" });
            NumberTexts.Add(15, new[] { "петнадесет" });
            NumberTexts.Add(16, new[] { "шестнадесет" });
            NumberTexts.Add(17, new[] { "седемнадесет" });
            NumberTexts.Add(18, new[] { "осемнадесет" });
            NumberTexts.Add(19, new[] { "деветнадесет" });
            NumberTexts.Add(20, new[] { "двадесет" });
            NumberTexts.Add(30, new[] { "тридесет" });
            NumberTexts.Add(40, new[] { "четиридесет" });
            NumberTexts.Add(50, new[] { "петдесет" });
            NumberTexts.Add(60, new[] { "шестдесет" });
            NumberTexts.Add(70, new[] { "седемдесет" });
            NumberTexts.Add(80, new[] { "осемдесет" });
            NumberTexts.Add(90, new[] { "деветдесет" });
            NumberTexts.Add(100, new[] { "сто" });
            NumberTexts.Add(200, new[] { "двеста" });
            NumberTexts.Add(300, new[] { "триста" });
            NumberTexts.Add(400, new[] { "четиристотин" });
            NumberTexts.Add(500, new[] { "петстотин" });
            NumberTexts.Add(600, new[] { "шестстотин" });
            NumberTexts.Add(700, new[] { "седемстотин" });
            NumberTexts.Add(800, new[] { "осемстотин" });
            NumberTexts.Add(900, new[] { "деветстотин" });

            ScaleTexts.Add(1000000000, new[] { "милиард", "милиарда" });
            ScaleTexts.Add(1000000, new[] { "милион", "милиона" });
            ScaleTexts.Add(1000, new[] { "хиляда", "хиляди" });
        }

        #region Currency

        protected override string GetCurrencyText(long num, CurrencyModel currency)
        {
            var textType = num == 1 ? 0 : 1;
            return currency.Names[textType];
        }

        protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency)
        {
            var textType = num == 1 ? 0 : 1;
            return currency.SubUnitCurrency.Names[textType];
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
                        Gender = GenderGroup.Neuter,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "евроцент", "евроцента", "евроцента" } }
                    };
                case Currency.GBP:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "британска лира", "британски лири", "британски лири" },
                    Gender = GenderGroup.Feminine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Neuter, Names = new[] { "пени", "пенита", "пенита" } }
                  };
                case Currency.ARS:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "аржентинско песо", "аржентинско песо", "аржентинско песо" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентаво", "сентаво", "сентаво" } }
                  };
                case Currency.BRL:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "бразилски реал", "бразилски реала", "бразилски реала" },
                    Gender = GenderGroup.Masculine,
                    SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентаво", "сентаво", "сентаво" } }
                  };
                // UZS is gone. The sum's sub-unit was written here as "тиин",
                // which is in no dictionary — the same invented wording that had Uzbek
                // naming the Turkish lira's sub-unit "tiyin".
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "долар", "долара", "долара" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "цент", "цента", "цента" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "рубли", "рубли", "рубли" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копейки", "копейки", "копейки" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "турска лира", "турска лира", "турска лира" },
                        Gender = GenderGroup.Feminine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "куруш", "куруши", "куруши" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "гривня", "гривни", "гривни" },
                        Gender = GenderGroup.Feminine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копейка", "копейки", "копейки" } }
                    };
                case Currency.BGN:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] {"лев", "лева", "лева"},
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel {Gender = GenderGroup.Feminine, Names = new[] {"стотинка", "стотинки", "стотинки"}}
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "бр", "бр", "бр" },
                        Gender = GenderGroup.Masculine,
                        // The birr divides into сантими, not стотинки. Masculine inanimate,
                        // so it takes the counting form -а after a number.
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сантим", "сантима", "сантима" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "злоти", "злоти", "злоти" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "гроз", "гроз", "гроз" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "белоруски рубли", "белоруски рубли", "белоруски рубли" },
                        Gender = GenderGroup.Masculine,
                        SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копейки", "копейки", "копейки" } }
                    };
            }
            return null;
        }

        protected override string GetUnitSeparator(CurrencyModel currency)
        {
            return " и ";
        }

        #endregion
    }
}
