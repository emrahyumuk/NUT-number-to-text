using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nut.Models;

namespace Nut.TextConverters
{
    public class BulgarianConverter : BaseConverter
    {
        private static Lazy<BulgarianConverter> Lazy = new Lazy<BulgarianConverter>(() => new BulgarianConverter());
        public static BulgarianConverter Instance => Lazy.Value;
        public override string CultureName { get; } = Culture.Bulgarian;

        protected int textType = 0;

        public BulgarianConverter()
        {
            Initialize();
        }

        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
        {
            switch (currencyModel.Currency)
            {
                case Currency.RUB:
                    NumberTexts[2][0] = isMainUnit ? "два" : "две";
                    break;
                case Currency.EUR:
                    NumberTexts[2][0] = isMainUnit ? "два" : "две";
                    break;
                default:
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

                textType = scale < 999 ? GetTextType(baseScale) : (num < 2000 ? 0 : 1);

                var baseUnitNumber = baseScale % 10;

                if (scale == 1000 && baseUnitNumber == 1 && num > 10000)
                    textType = 3;

                if (scale == 1000 && textType < 3 && (baseUnitNumber == 1 || baseUnitNumber == 2))
                {
                    AppendLessThanOneThousandForAdditional(baseScale, builder);
                }
                else
                {
                    AppendLessThanOneThousand(baseScale, builder);
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
            if (num == 1 && builder.ToString().Trim().Length == 0) return;
            AppendUnits(num, builder);
        }

        private void AppendLessThanOneThousandForAdditional(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            if (num == 1 && builder.ToString().Trim().Length == 0) return;
            AppendUnits(num, builder);
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
                builder.AppendFormat("{0} ", NumberTexts[num][targetType]);
            }
        }

        protected override void AppendUnitsForAdditional(long num, StringBuilder builder)
        {
            if (num != 0 && builder.ToString().Trim().Length > 0)
                builder.Append("и ");
            base.AppendUnitsForAdditional(num, builder);
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
                    return 0;
                if (baseUnitNumber >= femmeMinBaseScale && baseUnitNumber < pluralMinBaseScale)
                    return 1;
            }
            return 0;
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

        protected override string GetCurrencyText(long num, CurrencyModel currency, bool useShort)
        {
            if (useShort)
                return currency.ShortUnitCurrency;
            var textType = num == 1 ? 0 : 1;
            return currency.Names[textType];
        }

        protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency, bool useShort)
        {
            if (useShort)
                return currency.ShortSubUnitCurrency;
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
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "евроцент", "евроцента", "евроцента" } },
                        ShortUnitCurrency = "ев.",
                        ShortSubUnitCurrency = "цт."
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "долар", "долара", "долара" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "цент", "цента", "цента" } },
                        ShortUnitCurrency = "дл.",
                        ShortSubUnitCurrency = "цт."
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "рубли", "рубли", "рубли" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейки", "копейки", "копейки" } },
                        ShortUnitCurrency = "рб.",
                        ShortSubUnitCurrency = "кп."
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "турска лира", "турска лира", "турска лира" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруши", "куруши" } },
                        ShortUnitCurrency = "тл.",
                        ShortSubUnitCurrency = "кр."
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "гривня", "гривни", "гривни" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копейки" } },
                        ShortUnitCurrency = "гр.",
                        ShortSubUnitCurrency = "кп."
                    };
                case Currency.BGN:
                    return new CurrencyModel {
                        Currency = currency,
                        Names = new[] {"лев", "лева", "лева"},
                        SubUnitCurrency = new BaseCurrencyModel {Names = new[] {"стотинка", "стотинки", "стотинки"}},
                        ShortUnitCurrency = "лв.",
                        ShortSubUnitCurrency = "ст."
                    };
            }
            return null;
        }

        protected override string GetUnitSeparator(CurrencyModel currency, bool addAndAsUnitSeparator)
        {
            return addAndAsUnitSeparator ? " и " : " ";
        }

        #endregion
    }
}
