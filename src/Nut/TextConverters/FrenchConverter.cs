using System;
using System.Linq;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class FrenchConverter : BaseConverter
    {

        private static readonly Lazy<FrenchConverter> Lazy = new Lazy<FrenchConverter>(() => new FrenchConverter());
        public static FrenchConverter Instance => Lazy.Value;

        public override string CultureName => Culture.French;

        public FrenchConverter()
        {
            Initialize();
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;

                if (scale != 1000 || baseScale != 1)
                {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20)
            {

                if (num == 80)
                {
                    builder.AppendFormat("{0}", NumberTexts[num][1]);
                    return 0;
                }

                var tens = ((int)(num / 10)) * 10;

                if (tens == 70 || tens == 90)
                {
                    tens = tens - 10;

                    if (num - tens == 11)
                    {
                        builder.AppendFormat("{0} ", NumberTexts[tens][0]);
                        builder.AppendFormat("{0} ", NumberTexts[num - tens][1]);
                        return 0;
                    }

                    builder.AppendFormat("{0}-", NumberTexts[tens][0]);

                }
                else {
                    builder.AppendFormat("{0} ", NumberTexts[tens][0]);

                    var etUnList = new long[] { 21, 31, 41, 51, 61 };
                    if (etUnList.Contains(num))
                    {
                        builder.AppendFormat("{0} ", NumberTexts[num - tens][1]);
                        return 0;
                    }
                }

                num = num - tens;
            }
            return num;
        }
        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "zéro" });
            NumberTexts.Add(1, new[] { "un", "et un" });
            NumberTexts.Add(2, new[] { "deux" });
            NumberTexts.Add(3, new[] { "trois" });
            NumberTexts.Add(4, new[] { "quatre" });
            NumberTexts.Add(5, new[] { "cinq" });
            NumberTexts.Add(6, new[] { "six" });
            NumberTexts.Add(7, new[] { "sept" });
            NumberTexts.Add(8, new[] { "huit" });
            NumberTexts.Add(9, new[] { "neuf" });
            NumberTexts.Add(10, new[] { "dix" });
            NumberTexts.Add(11, new[] { "onze", "et onze" });
            NumberTexts.Add(12, new[] { "douze" });
            NumberTexts.Add(13, new[] { "treize" });
            NumberTexts.Add(14, new[] { "quatorze" });
            NumberTexts.Add(15, new[] { "quinze" });
            NumberTexts.Add(16, new[] { "seize" });
            NumberTexts.Add(17, new[] { "dix-sept" });
            NumberTexts.Add(18, new[] { "dix-huit" });
            NumberTexts.Add(19, new[] { "dix-neuf" });
            NumberTexts.Add(20, new[] { "vingt" });
            NumberTexts.Add(30, new[] { "trente" });
            NumberTexts.Add(40, new[] { "quarante" });
            NumberTexts.Add(50, new[] { "cinquante " });
            NumberTexts.Add(60, new[] { "soixante" });
            NumberTexts.Add(80, new[] { "quatre-vingt", "quatre-vingts" });
            NumberTexts.Add(100, new[] { "cent" });

            ScaleTexts.Add(1000000000, new[] { "milliard" });
            ScaleTexts.Add(1000000, new[] { "million" });
            ScaleTexts.Add(1000, new[] { "mille" });
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
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centime", "centimes" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "dollar", "dollars" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centime", "centimes" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "rouble", "roubles" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopeck ", "kopecks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "livre turques", "livres turques" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "hryvnia ukrainienne", "hryvnias ukrainienne" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiyka", "kopiyka" } }
                    };
            }
            return null;
        }
    }
}