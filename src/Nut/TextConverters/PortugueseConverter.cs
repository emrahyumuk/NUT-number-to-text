using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
    public sealed class PortugueseConverter : BaseConverter
    {
        private static readonly Lazy<PortugueseConverter> Lazy = new Lazy<PortugueseConverter>(() => new PortugueseConverter());
        public static PortugueseConverter Instance => Lazy.Value;

        public override string CultureName => Culture.Spanish;

        public PortugueseConverter()
        {
            Initialize();
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num < 0)
            {
                builder.AppendFormat("menos ");
                num = -num;
            }
            if (num > scale - 1)
            {
                var baseScale = num / scale;
                if (scale > 999 && baseScale == 1)
                {
                    AppendUnitsForAdditional(baseScale, builder);
                }
                else
                {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                if ((scale == 1000000 || scale == 1000000000) && num / scale > 1)
                {
                    if (num % scale > 0)
                        builder.AppendFormat("{0} e ", ScaleTexts[scale][1]);
                    else
                        builder.AppendFormat("{0} ", ScaleTexts[scale][1]);
                }
                else
                {
                    if (num % scale > 0)
                        builder.AppendFormat("{0} e ", ScaleTexts[scale][0]);
                    else
                        builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
                }

                num = num - (baseScale * scale);
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20)
            {
                var tens = ((int)(num / 10)) * 10;

                builder.AppendFormat(num == tens ? "{0} " : "{0} e ", NumberTexts[tens][0]);

                num = num - tens;
            }
            return num;
        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100 * 100;
                if (num % 100 > 0)
                {
                    builder.AppendFormat("{0} e ", NumberTexts[hundreds][num < 200 ? 1 : 0]);
                }
                else
                {
                    builder.AppendFormat("{0} ", NumberTexts[hundreds][0]);
                }

                num = num - hundreds;
            }
            return num;
        }

        protected override string GetUnitSeparator(CurrencyModel currency)
        {
            return " com ";
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "zero" });
            NumberTexts.Add(1, new[] { "um" });
            NumberTexts.Add(2, new[] { "dois" });
            NumberTexts.Add(3, new[] { "três" });
            NumberTexts.Add(4, new[] { "quatro" });
            NumberTexts.Add(5, new[] { "cinco" });
            NumberTexts.Add(6, new[] { "seis" });
            NumberTexts.Add(7, new[] { "sete" });
            NumberTexts.Add(8, new[] { "oito" });
            NumberTexts.Add(9, new[] { "nove" });
            NumberTexts.Add(10, new[] { "dez" });
            NumberTexts.Add(11, new[] { "onze" });
            NumberTexts.Add(12, new[] { "doze" });
            NumberTexts.Add(13, new[] { "treze" });
            NumberTexts.Add(14, new[] { "quatorze" });
            NumberTexts.Add(15, new[] { "quinze" });
            NumberTexts.Add(16, new[] { "dezesseis" });
            NumberTexts.Add(17, new[] { "dezessete" });
            NumberTexts.Add(18, new[] { "dezoito" });
            NumberTexts.Add(19, new[] { "dezanove" });
            NumberTexts.Add(20, new[] { "vinte" });
            NumberTexts.Add(30, new[] { "trinta" });
            NumberTexts.Add(40, new[] { "quarenta" });
            NumberTexts.Add(50, new[] { "cinquenta" });
            NumberTexts.Add(60, new[] { "sessenta" });
            NumberTexts.Add(70, new[] { "setenta" });
            NumberTexts.Add(80, new[] { "oitenta" });
            NumberTexts.Add(90, new[] { "noventa" });
            NumberTexts.Add(100, new[] { "cem", "cento" });
            NumberTexts.Add(200, new[] { "duzentos" });
            NumberTexts.Add(300, new[] { "trezentos", });
            NumberTexts.Add(400, new[] { "quatrocentos" });
            NumberTexts.Add(500, new[] { "quinhentos" });
            NumberTexts.Add(600, new[] { "seiscentos" });
            NumberTexts.Add(700, new[] { "setecentos" });
            NumberTexts.Add(800, new[] { "oitocentos" });
            NumberTexts.Add(900, new[] { "novecentos" });

            ScaleTexts.Add(1000000000, new[] { "bilhão", "bilhões" });
            ScaleTexts.Add(1000000, new[] { "milhão", "milhões" });
            ScaleTexts.Add(1000, new[] { "mil" });
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
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo de euro", "centavos de euro" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "dólar", "dólares" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "rublo", "rublos" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "lira turco", "liras turco" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "grivna ucraniana", "grivnas ucraniana" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "birr", "birr" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "zloty", "zloty" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "groszy", "groszy" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "rublo bielorruso", "rublos bielorrusos" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.ARS:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "peso", "pesos" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
                case Currency.BRL:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "real", "reais" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
            }
            return null;
        }
    }
}
