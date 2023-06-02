using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class TurkishConverter : BaseConverter
    {

        private static readonly Lazy<TurkishConverter> Lazy = new Lazy<TurkishConverter>(() => new TurkishConverter());
        public static TurkishConverter Instance => Lazy.Value;

        public override string CultureName => Culture.Turkish;

        public TurkishConverter()
        {
            Initialize();
        }
        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num < 0)
            {
                builder.AppendFormat("eksi ");
                num = -num;
            }
            if (num > scale - 1)
            {
                var baseScale = num / scale;
                if (!(baseScale == 1 && (scale == 100 || scale == 1000)))
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
            if (num > 10)
            {
                var tens = num / 10 * 10;
                builder.AppendFormat("{0} ", NumberTexts[tens][0]);
                num = num - tens;
            }
            return num;
        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100;
                if (hundreds != 1)
                {
                    builder.AppendFormat("{0} {1} ", NumberTexts[hundreds][0], NumberTexts[100][0]);
                }
                else
                {
                    builder.AppendFormat("{0} ", NumberTexts[100][0]);
                }
                num = num - (hundreds * 100);
            }
            return num;
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "sıfır" });
            NumberTexts.Add(1, new[] { "bir" });
            NumberTexts.Add(2, new[] { "iki" });
            NumberTexts.Add(3, new[] { "üç" });
            NumberTexts.Add(4, new[] { "dört" });
            NumberTexts.Add(5, new[] { "beş" });
            NumberTexts.Add(6, new[] { "altı" });
            NumberTexts.Add(7, new[] { "yedi" });
            NumberTexts.Add(8, new[] { "sekiz" });
            NumberTexts.Add(9, new[] { "dokuz" });
            NumberTexts.Add(10, new[] { "on" });
            NumberTexts.Add(20, new[] { "yirmi" });
            NumberTexts.Add(30, new[] { "otuz" });
            NumberTexts.Add(40, new[] { "kırk" });
            NumberTexts.Add(50, new[] { "elli" });
            NumberTexts.Add(60, new[] { "altmış" });
            NumberTexts.Add(70, new[] { "yetmiş" });
            NumberTexts.Add(80, new[] { "seksen" });
            NumberTexts.Add(90, new[] { "doksan" });
            NumberTexts.Add(100, new[] { "yüz" });

            ScaleTexts.Add(1000000000, new[] { "milyar" });
            ScaleTexts.Add(1000000, new[] { "milyon" });
            ScaleTexts.Add(1000, new[] { "bin" });
        }

        protected override CurrencyModel GetCurrencyModel(string currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "avro", "avro" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "avro sent", "avro sent" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "dolar", "dolar" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "sent", "sent" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ruble", "ruble" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopek" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "türk lirası", "türk lirası" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "grivna", "grivna" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiyka", "kopiyka" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "birr", "birr" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "santim", "santim" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "zloti", "zloti" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "grosz", "grosz" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "Belarus rublesi", "Belarus rublesi" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopek" } }
                    };
            }
            return null;
        }
    }
}
