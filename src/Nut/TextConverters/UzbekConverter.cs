using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  /// <summary>
  /// Uzbek in the Latin script. The number system is built from twenty-two basic words and
  /// otherwise works like Turkish: no "one" before yuz or ming, and the teens are written
  /// as two words ("o'n bir").
  /// </summary>
  public sealed class UzbekConverter : BaseConverter
  {
    private static readonly Lazy<UzbekConverter> Lazy = new Lazy<UzbekConverter>(() => new UzbekConverter());
    public static UzbekConverter Instance => Lazy.Value;

    public override string CultureName => Culture.Uzbek;

    protected override string NegativeSign => "minus";

    public UzbekConverter()
    {
      Initialize();
    }

    protected override long Append(long num, long scale, StringBuilder builder)
    {
      if (num > scale - 1)
      {
        var baseScale = num / scale;
        // "ming so'm", not "bir ming so'm".
        if (baseScale != 1 || scale != 1000)
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
        // "yuz", not "bir yuz".
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
      NumberTexts.Add(0, new[] { "nol" });
      NumberTexts.Add(1, new[] { "bir" });
      NumberTexts.Add(2, new[] { "ikki" });
      NumberTexts.Add(3, new[] { "uch" });
      NumberTexts.Add(4, new[] { "to'rt" });
      NumberTexts.Add(5, new[] { "besh" });
      NumberTexts.Add(6, new[] { "olti" });
      NumberTexts.Add(7, new[] { "yetti" });
      NumberTexts.Add(8, new[] { "sakkiz" });
      NumberTexts.Add(9, new[] { "to'qqiz" });
      NumberTexts.Add(10, new[] { "o'n" });
      NumberTexts.Add(20, new[] { "yigirma" });
      NumberTexts.Add(30, new[] { "o'ttiz" });
      NumberTexts.Add(40, new[] { "qirq" });
      NumberTexts.Add(50, new[] { "ellik" });
      NumberTexts.Add(60, new[] { "oltmish" });
      NumberTexts.Add(70, new[] { "yetmish" });
      NumberTexts.Add(80, new[] { "sakson" });
      NumberTexts.Add(90, new[] { "to'qson" });
      NumberTexts.Add(100, new[] { "yuz" });

      ScaleTexts.Add(1000000000, new[] { "milliard" });
      ScaleTexts.Add(1000000, new[] { "million" });
      ScaleTexts.Add(1000, new[] { "ming" });
    }

    protected override CurrencyModel GetCurrencyModel(string currency)
    {
      switch (currency)
      {
        case Currency.UZS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "so'm", "so'm" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "tiyin", "tiyin" } }
          };
        case Currency.GBP:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "funt sterling", "funt sterling" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "pens", "pens" } }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "dollar", "dollar" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "sent", "sent" } }
          };
        case Currency.EUR:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "yevro", "yevro" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "sent", "sent" } }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "rubl", "rubl" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "tiyin", "tiyin" } }
          };
        case Currency.TRY:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "turk lirasi", "turk lirasi" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "tiyin", "tiyin" } }
          };
      }
      return null;
    }
  }
}
