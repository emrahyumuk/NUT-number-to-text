using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class GermanConverter : BaseConverter
    {

        private static readonly Lazy<GermanConverter> Lazy = new Lazy<GermanConverter>(() => new GermanConverter());
        public static GermanConverter Instance => Lazy.Value;

        public override string CultureName => Culture.GermanDE;

        protected override string NegativeSign => "minus";

        public GermanConverter()
        {
            Initialize();
        }

        // Set only on the short-lived per-conversion instance used for money.
        private readonly bool _beforeNoun;

        private GermanConverter(GermanConverter template, bool beforeNoun) : base(template)
        {
            _beforeNoun = beforeNoun;
        }

        /// <summary>
        /// A currency name is a noun, and German uses "ein" before a noun where a bare
        /// number ends in "eins": "ein Euro", but "eins" on its own.
        /// </summary>
        protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
        {
            return new GermanConverter(this, true).ToText(num);
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "null" });
            // [0] standing alone or ending a number, [1] inside a compound:
            // "eins", but "einundzwanzig" and "einhundert".
            NumberTexts.Add(1, new[] { "eins", "ein" });
            NumberTexts.Add(2, new[] { "zwei" });
            NumberTexts.Add(3, new[] { "drei" });
            NumberTexts.Add(4, new[] { "vier" });
            NumberTexts.Add(5, new[] { "fünf" });
            NumberTexts.Add(6, new[] { "sechs" });
            NumberTexts.Add(7, new[] { "sieben" });
            NumberTexts.Add(8, new[] { "acht" });
            NumberTexts.Add(9, new[] { "neun" });
            NumberTexts.Add(10, new[] { "zehn" });
            NumberTexts.Add(11, new[] { "elf" });
            NumberTexts.Add(12, new[] { "zwölf" });
            NumberTexts.Add(13, new[] { "dreizehn" });
            NumberTexts.Add(14, new[] { "vierzehn" });
            NumberTexts.Add(15, new[] { "fünfzehn" });
            NumberTexts.Add(16, new[] { "sechzehn" });
            NumberTexts.Add(17, new[] { "siebzehn" });
            NumberTexts.Add(18, new[] { "achtzehn" });
            NumberTexts.Add(19, new[] { "neunzehn" });
            NumberTexts.Add(20, new[] { "zwanzig" });
            NumberTexts.Add(30, new[] { "dreißig" });
            NumberTexts.Add(40, new[] { "vierzig" });
            NumberTexts.Add(50, new[] { "fünfzig" });
            NumberTexts.Add(60, new[] { "sechzig" });
            NumberTexts.Add(70, new[] { "siebzig" });
            NumberTexts.Add(80, new[] { "achtzig" });
            NumberTexts.Add(90, new[] { "neunzig" });
            NumberTexts.Add(100, new[] { "hundert" });


            ScaleTexts.Add(1000000000000000000, new[] { "Trillionen", "eine Trillion" });
            ScaleTexts.Add(1000000000000000, new[] { "Billiarden", "eine Billiarde" });
            ScaleTexts.Add(1000000000000, new[] { "Billionen", "eine Billion" });
            ScaleTexts.Add(1000000000, new[] { "Milliarden", "eine Milliarde" });
            ScaleTexts.Add(1000000, new[] { "Millionen", "eine Million" });
            ScaleTexts.Add(1000, new[] { "tausend", "eintausend" });
        }

        protected override CurrencyModel GetCurrencyModel(string currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Euro", "Euro" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Cent", "Cent" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Dollar", "Dollar" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Cent", "Cent" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Rubel", "Rubel" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Kopeke", "Kopeken" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "türkische Lire", "türkische Lire" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Kurus", "Kurus" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ukrainische Griwna", "ukrainische Griwna" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiyka", "kopiyky", "kopiyok" } }
                    };

                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Birr", "Birr" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Cent", "Cent" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Zloty", "Zloty" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "grosz", "grosze", "groszy" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "weißrussischer Rubel", "weißrussischer Rubel" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Kopeke", "Kopeken" } }
                    };
            }
            return null;
        }

        /// <summary>
        /// Duden: numbers below a million are written as one closed-up word, and only from
        /// a million upwards are the parts separated — "einhundertzwanzigtausend", but
        /// "zwei Millionen einhundertzwanzigtausendvierhundertneunzehn".
        /// </summary>
        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;
                if (scale >= 1000000)
                {
                    if (baseScale != 1)
                    {
                        AppendLessThanOneThousand(baseScale, builder);
                        builder.Append(" ");
                    }
                    builder.AppendFormat("{0} ", baseScale == 1 ? ScaleTexts[scale][1] : ScaleTexts[scale][0]);
                }
                else
                {
                    if (baseScale == 1)
                    {
                        builder.Append(ScaleTexts[scale][1]);
                    }
                    else
                    {
                        AppendLessThanOneThousand(baseScale, builder);
                        builder.Append(ScaleTexts[scale][0]);
                    }
                }
                num = num - (baseScale * scale);
            }
            return num;
        }

        protected override void AppendLessThanOneThousand(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            if (num <= 20)
            {
                AppendUnits(num, builder);
            }
            else
            {
                var units = num % 10;
                if (units > 0)
                {
                    builder.AppendFormat("{0}und", NumberTexts[units][units == 1 ? 1 : 0]);
                }

                AppendTens(num, builder);
            }

        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100;
                builder.Append(NumberTexts[hundreds][hundreds == 1 ? 1 : 0]);
                builder.Append(NumberTexts[100][0]);
                num = num - (hundreds * 100);
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20)
            {
                var tens = num / 10 * 10;
                builder.Append(NumberTexts[tens][0]);
                num = num - tens;
            }
            return num;
        }

        protected override void AppendUnits(long num, StringBuilder builder)
        {
            if (num > 0)
            {
                builder.Append(NumberTexts[num][_beforeNoun && num == 1 ? 1 : 0]);
            }
        }
    }
}