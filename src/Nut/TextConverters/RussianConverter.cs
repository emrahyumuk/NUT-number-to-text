using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  public sealed class RussianConverter : BaseConverter
  {

    private static readonly Lazy<RussianConverter> Lazy = new Lazy<RussianConverter>(() => new RussianConverter());
    public static RussianConverter Instance => Lazy.Value;

    public override string CultureName => Culture.Russian;

    public RussianConverter()
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
      NumberTexts.Add(0, new[] { "ноль" });
      NumberTexts.Add(1, new[] { "один", "одна" });
      NumberTexts.Add(2, new[] { "два", "две" });
      NumberTexts.Add(3, new[] { "три" });
      NumberTexts.Add(4, new[] { "четыре" });
      NumberTexts.Add(5, new[] { "пять" });
      NumberTexts.Add(6, new[] { "шесть" });
      NumberTexts.Add(7, new[] { "семь" });
      NumberTexts.Add(8, new[] { "восемь" });
      NumberTexts.Add(9, new[] { "девять" });
      NumberTexts.Add(10, new[] { "десять" });
      NumberTexts.Add(11, new[] { "одиннадцать" });
      NumberTexts.Add(12, new[] { "двенадцать" });
      NumberTexts.Add(13, new[] { "тринадцать" });
      NumberTexts.Add(14, new[] { "четырнадцать" });
      NumberTexts.Add(15, new[] { "пятнадцать" });
      NumberTexts.Add(16, new[] { "шестнадцать" });
      NumberTexts.Add(17, new[] { "семнадцать" });
      NumberTexts.Add(18, new[] { "восемнадцать" });
      NumberTexts.Add(19, new[] { "девятнадцать" });
      NumberTexts.Add(20, new[] { "двадцать" });
      NumberTexts.Add(30, new[] { "тридцать" });
      NumberTexts.Add(40, new[] { "сорок" });
      NumberTexts.Add(50, new[] { "пятьдесят" });
      NumberTexts.Add(60, new[] { "шестьдесят" });
      NumberTexts.Add(70, new[] { "семьдесят" });
      NumberTexts.Add(80, new[] { "восемьдесят" });
      NumberTexts.Add(90, new[] { "девяносто" });
      NumberTexts.Add(100, new[] { "сто" });
      NumberTexts.Add(200, new[] { "двести" });
      NumberTexts.Add(300, new[] { "триста" });
      NumberTexts.Add(400, new[] { "четыреста" });
      NumberTexts.Add(500, new[] { "пятьсот" });
      NumberTexts.Add(600, new[] { "шестьсот" });
      NumberTexts.Add(700, new[] { "семьсот" });
      NumberTexts.Add(800, new[] { "восемьсот" });
      NumberTexts.Add(900, new[] { "девятьсот" });

      ScaleTexts.Add(1000000000, new[] { "миллиардов", "миллиард", "миллиарда" });
      ScaleTexts.Add(1000000, new[] { "миллионов", "миллион", "миллиона" });
      ScaleTexts.Add(1000, new[] { "тысяч", "тысяча", "тысячи" });
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
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "евроцент", "евроцента", "евроцентов" } }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "доллар", "доллара", "долларов" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "цент", "цента", "центов" } }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "рубль", "рубля", "рублей" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
          };
        case Currency.TRY:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "турецкая лира", "турецких лир", "турецких лир" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруша", "курушей" } }
          };
        case Currency.UAH:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "гривня", "гривні", "гривень" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
          };
        case Currency.ETB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "быр", "быр" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
          };
      }
      return null;
    }

    #endregion
  }
}
