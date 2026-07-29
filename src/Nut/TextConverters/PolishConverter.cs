using Nut.Models;
using System;
using System.Text;

namespace Nut.TextConverters
{
    public sealed class PolishConverter : BaseConverter
    {
        private static readonly Lazy<PolishConverter> Lazy = new Lazy<PolishConverter>(() => new PolishConverter());
        public static PolishConverter Instance { get { return Lazy.Value; } }

        public override string CultureName
        {
            get { return "pl-PL"; }
        }

        protected override string NegativeSign => "minus";

        public PolishConverter()
        {
            Initialize();
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;


                var textType = GetTextType(baseScale);
                if ((scale == 1000000 || scale == 1000000000) && textType < 3 && (baseScale == 1 || baseScale == 2))
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
                        if (baseScale == 1)
                        {
                            builder.AppendFormat("{0} ", ScaleTexts[scale][1]);
                        }
                        else
                        {
                            builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
                        }
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
                builder.AppendFormat("{0} ", NumberTexts[hundreds]);
                num -= hundreds;
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
                    return 2; //   miliardów,milionów,tysięcy
            }
            return 3;
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "zero" });
            NumberTexts.Add(1, new[] { "jeden" });
            NumberTexts.Add(2, new[] { "dwa" });
            NumberTexts.Add(3, new[] { "trzy" });
            NumberTexts.Add(4, new[] { "cztery" });
            NumberTexts.Add(5, new[] { "pięć" });
            NumberTexts.Add(6, new[] { "sześć" });
            NumberTexts.Add(7, new[] { "siedem" });
            NumberTexts.Add(8, new[] { "osiem" });
            NumberTexts.Add(9, new[] { "dziewięć" });
            NumberTexts.Add(10, new[] { "dziesięć" });
            NumberTexts.Add(11, new[] { "jedenaście" });
            NumberTexts.Add(12, new[] { "dwanaście" });
            NumberTexts.Add(13, new[] { "trzynaście" });
            NumberTexts.Add(14, new[] { "czternaście" });
            NumberTexts.Add(15, new[] { "piętnaście" });
            NumberTexts.Add(16, new[] { "szesnaście" });
            NumberTexts.Add(17, new[] { "siedemnaście" });
            NumberTexts.Add(18, new[] { "osiemnaście" });
            NumberTexts.Add(19, new[] { "dziewiętnaście" });
            NumberTexts.Add(20, new[] { "dwadzieścia" });
            NumberTexts.Add(30, new[] { "trzydzieści" });
            NumberTexts.Add(40, new[] { "czterdzieści" });
            NumberTexts.Add(50, new[] { "pięćdziesiąt" });
            NumberTexts.Add(60, new[] { "sześćdziesiąt" });
            NumberTexts.Add(70, new[] { "siedemdziesiąt" });
            NumberTexts.Add(80, new[] { "osiemdziesiąt" });
            NumberTexts.Add(90, new[] { "dziewięćdziesiąt" });
            NumberTexts.Add(100, new[] { "sto" });
            NumberTexts.Add(200, new[] { "dwieście" });
            NumberTexts.Add(300, new[] { "trzysta" });
            NumberTexts.Add(400, new[] { "czterysta" });
            NumberTexts.Add(500, new[] { "pięćset" });
            NumberTexts.Add(600, new[] { "sześćset" });
            NumberTexts.Add(700, new[] { "siedemset" });
            NumberTexts.Add(800, new[] { "osiemset" });
            NumberTexts.Add(900, new[] { "dziewięćset" });

            ScaleTexts.Add(1000000000, new[] { "miliardów", "miliard", "miliardy" });
            ScaleTexts.Add(1000000, new[] { "milionów", "milion", "miliony" });
            ScaleTexts.Add(1000, new[] { "tysięcy", "tysiąc", "tysiące" });
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
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "złoty", "złote", "złotych" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "grosz", "grosze", "groszy" } }
                    };
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "euro", "euro", "euro" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "eurocent", "eurocenty", "eurocentów" } }
                    };
                case Currency.GBP:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "funt szterling", "funty szterlingi", "funtów szterlingów" },
                    SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "pens", "pensy", "pensów" } }
                  };
                case Currency.ARS:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "peso argentyńskie", "pesa argentyńskie", "pesos argentyńskich" },
                    SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavo", "centavo" } }
                  };
                case Currency.BGN:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "lew", "lewy", "lewów" },
                    SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "stotinka", "stotinki", "stotinek" } }
                  };
                case Currency.BRL:
                  return new CurrencyModel
                  {
                    Currency = currency,
                    Names = new[] { "real brazylijski", "reale brazylijskie", "reali brazylijskich" },
                    SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavo", "centavo" } }
                  };
                // UZS is gone. The sum's sub-unit was written here as "tijin",
                // which is in no dictionary — the same invented wording that had Uzbek
                // naming the Turkish lira's sub-unit "tiyin".
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "dolar", "dolary", "dolarów" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "cent", "centy", "centów" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "rubel", "ruble", "rubli" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiejka", "kopiejki", "kopiejek" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "turecki lir", "tureckiego lira", "tureckich lirów" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруша", "курушей" } }
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "hrywna", "hrywny", "hrywien" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiejka", "kopiejki", "kopiejek" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "берр", "берр", "берр" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копійка", "копійки", "копійок" } }
                    };
                case Currency.BYN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "rubel białoruski", "ruble białoruskie", "rubli białoruskich" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "kopiejka", "kopiejki", "kopiejek" } }
                    };
            }
            return null;
        }

        #endregion
    }
}
