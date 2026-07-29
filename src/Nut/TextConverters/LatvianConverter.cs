using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  /// <summary>
  /// Latvian. Numerals agree in gender with what they count — "viens eiro" but
  /// "viena mārciņa" — and the unit name takes three forms rather than two: singular after
  /// a count ending in one, plural otherwise, and the genitive plural after zero
  /// ("nulle centu", not "nulle centi").
  /// </summary>
  public sealed class LatvianConverter : BaseConverter
  {
    private static readonly Lazy<LatvianConverter> Lazy = new Lazy<LatvianConverter>(() => new LatvianConverter());
    public static LatvianConverter Instance => Lazy.Value;

    public override string CultureName => Culture.Latvian;

    protected override string NegativeSign => "mīnus";

    public LatvianConverter()
    {
      Initialize();
    }

    // Set only on the short-lived per-conversion instance used for money.
    private readonly bool _feminine;

    private LatvianConverter(LatvianConverter template, bool feminine) : base(template)
    {
      _feminine = feminine;
    }

    protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
    {
      var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
      return new LatvianConverter(this, gender == GenderGroup.Feminine).ToText(num);
    }

    /// <summary>Only one and two inflect: viens/viena, divi/divas.</summary>
    protected override void AppendUnits(long num, StringBuilder builder)
    {
      if (_feminine && (num == 1 || num == 2))
        builder.AppendFormat("{0} ", NumberTexts[num][1]);
      else
        base.AppendUnits(num, builder);
    }

    protected override long Append(long num, long scale, StringBuilder builder)
    {
      if (num > scale - 1)
      {
        var baseScale = num / scale;
        // The count is always spoken here: "viens tūkstotis", not a bare "tūkstotis".
        AppendLessThanOneThousand(baseScale, builder);
        builder.AppendFormat("{0} ", ScaleTexts[scale][baseScale == 1 ? 0 : 1]);
        num = num - (baseScale * scale);
      }
      return num;
    }

    protected override long AppendHundreds(long num, StringBuilder builder)
    {
      if (num > 99)
      {
        var hundreds = num / 100;
        // "simts" on its own, "divi simti" when multiplied.
        if (hundreds == 1)
        {
          builder.AppendFormat("{0} ", NumberTexts[100][0]);
        }
        else
        {
          builder.AppendFormat("{0} {1} ", NumberTexts[hundreds][0], NumberTexts[100][1]);
        }
        num = num - (hundreds * 100);
      }
      return num;
    }

    /// <summary>
    /// Singular after a count ending in one (but not eleven), genitive plural after zero,
    /// plural otherwise.
    /// </summary>
    private static string Inflect(long num, string[] names)
    {
      if (num == 0) return names.Length > 2 ? names[2] : names[1];
      if (num % 10 == 1 && num % 100 != 11) return names[0];
      return names[1];
    }

    protected override string GetCurrencyText(long num, CurrencyModel currency)
    {
      return Inflect(num, currency.Names);
    }

    protected override string GetSubUnitCurrencyText(long num, CurrencyModel currency)
    {
      return Inflect(num, currency.SubUnitCurrency.Names);
    }

    protected override string GetUnitSeparator(CurrencyModel currency)
    {
      return " un ";
    }

    private void Initialize()
    {
      NumberTexts.Add(0, new[] { "nulle" });
      NumberTexts.Add(1, new[] { "viens", "viena" });
      NumberTexts.Add(2, new[] { "divi", "divas" });
      NumberTexts.Add(3, new[] { "trīs" });
      NumberTexts.Add(4, new[] { "četri" });
      NumberTexts.Add(5, new[] { "pieci" });
      NumberTexts.Add(6, new[] { "seši" });
      NumberTexts.Add(7, new[] { "septiņi" });
      NumberTexts.Add(8, new[] { "astoņi" });
      NumberTexts.Add(9, new[] { "deviņi" });
      NumberTexts.Add(10, new[] { "desmit" });
      NumberTexts.Add(11, new[] { "vienpadsmit" });
      NumberTexts.Add(12, new[] { "divpadsmit" });
      NumberTexts.Add(13, new[] { "trīspadsmit" });
      NumberTexts.Add(14, new[] { "četrpadsmit" });
      NumberTexts.Add(15, new[] { "piecpadsmit" });
      NumberTexts.Add(16, new[] { "sešpadsmit" });
      NumberTexts.Add(17, new[] { "septiņpadsmit" });
      NumberTexts.Add(18, new[] { "astoņpadsmit" });
      NumberTexts.Add(19, new[] { "deviņpadsmit" });
      NumberTexts.Add(20, new[] { "divdesmit" });
      NumberTexts.Add(30, new[] { "trīsdesmit" });
      NumberTexts.Add(40, new[] { "četrdesmit" });
      NumberTexts.Add(50, new[] { "piecdesmit" });
      NumberTexts.Add(60, new[] { "sešdesmit" });
      NumberTexts.Add(70, new[] { "septiņdesmit" });
      NumberTexts.Add(80, new[] { "astoņdesmit" });
      NumberTexts.Add(90, new[] { "deviņdesmit" });
      NumberTexts.Add(100, new[] { "simts", "simti" });

      ScaleTexts.Add(1000000000, new[] { "miljards", "miljardi" });
      ScaleTexts.Add(1000000, new[] { "miljons", "miljoni" });
      ScaleTexts.Add(1000, new[] { "tūkstotis", "tūkstoši" });
    }

    protected override CurrencyModel GetCurrencyModel(string currency)
    {
      switch (currency)
      {
        case Currency.EUR:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "eiro", "eiro", "eiro" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel
            {
              Gender = GenderGroup.Masculine,
              Names = new[] { "cents", "centi", "centu" }
            }
          };
        case Currency.GBP:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "sterliņu mārciņa", "sterliņu mārciņas", "sterliņu mārciņu" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel
            {
              Gender = GenderGroup.Masculine,
              Names = new[] { "penss", "pensi", "pensu" }
            }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "ASV dolārs", "ASV dolāri", "ASV dolāru" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel
            {
              Gender = GenderGroup.Masculine,
              Names = new[] { "cents", "centi", "centu" }
            }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "Krievijas rublis", "Krievijas rubļi", "Krievijas rubļu" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel
            {
              Gender = GenderGroup.Feminine,
              Names = new[] { "kapeika", "kapeikas", "kapeiku" }
            }
          };
        case Currency.PLN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "Polijas zlots", "Polijas zloti", "Polijas zlotu" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel
            {
              Gender = GenderGroup.Masculine,
              Names = new[] { "grasis", "graši", "grašu" }
            }
          };
      }
      return null;
    }
  }
}
