using System;
using System.Text;
using Nut.Models;

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
          return 2; //   miliardów,milionów,tysięcy
      }
      return 3;
    }

    private void Initialize()
    {
      NumberTexts.Add(0, new[] { "Zero" });
      NumberTexts.Add(1, new[] { "Jeden" });
      NumberTexts.Add(2, new[] { "Dwa" });
      NumberTexts.Add(3, new[] { "Trzy" });
      NumberTexts.Add(4, new[] { "Cztery" });
      NumberTexts.Add(5, new[] { "Pięć" });
      NumberTexts.Add(6, new[] { "Sześć" });
      NumberTexts.Add(7, new[] { "Siedem" });
      NumberTexts.Add(8, new[] { "Osiem" });
      NumberTexts.Add(9, new[] { "Dziewięć" });
      NumberTexts.Add(10, new[] { "Dziesięć" });
      NumberTexts.Add(11, new[] { "Jedenaście" });
      NumberTexts.Add(12, new[] { "Dwanaście" });
      NumberTexts.Add(13, new[] { "Trzynaście" });
      NumberTexts.Add(14, new[] { "Czternaście" });
      NumberTexts.Add(15, new[] { "Piętnaście" });
      NumberTexts.Add(16, new[] { "Szesnaście" });
      NumberTexts.Add(17, new[] { "Siedemnaście" });
      NumberTexts.Add(18, new[] { "Osiemnaście" });
      NumberTexts.Add(19, new[] { "Dziewiętnaście" });
      NumberTexts.Add(20, new[] { "Dwadzieścia" });
      NumberTexts.Add(30, new[] { "Trzydzieści" });
      NumberTexts.Add(40, new[] { "Czterdzieści" });
      NumberTexts.Add(50, new[] { "Pięćdziesiąt" });
      NumberTexts.Add(60, new[] { "Sześćdziesiąt" });
      NumberTexts.Add(70, new[] { "Siedemdziesiąt" });
      NumberTexts.Add(80, new[] { "Osiemdziesiąt" });
      NumberTexts.Add(90, new[] { "Dziewięćdziesiąt" });
      NumberTexts.Add(100, new[] { "Sto" });
      NumberTexts.Add(200, new[] { "Dwieście" });
      NumberTexts.Add(300, new[] { "Trzysta" });
      NumberTexts.Add(400, new[] { "Czterysta" });
      NumberTexts.Add(500, new[] { "Pięćset" });
      NumberTexts.Add(600, new[] { "Sześćset" });
      NumberTexts.Add(700, new[] { "Siedemset" });
      NumberTexts.Add(800, new[] { "Osiemset" });
      NumberTexts.Add(900, new[] { "Dziewięćset" });

      ScaleTexts.Add(1000000000, new[] { "miliardów", "miliard", "miliardy" });
      ScaleTexts.Add(1000000, new[] { "milionów", "milion", "miliony" });
      ScaleTexts.Add(1000, new[] { "tysięcy", "tysiąc", "tysiące" });

    }


    #region Currency

    protected override string GetCurrencyText(long num, CurrencyModel currency, bool useShort)
    {
      var textType = GetTextType(num);
      return currency.Names[textType - 1];
    }

    protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency, bool useshort)
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
            Names = new[] { "берр", "берр" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копійка", "копійки", "копійок" } }
          };
      }
      return null;
    }

    #endregion
  }
}
