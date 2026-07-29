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

    protected override string NegativeSign => "moins";

    public FrenchConverter()
    {
      Initialize();
    }
    
    // Set only on the short-lived instance used for the part in front of a scale word.
    private readonly bool _beforeScaleWord;
    private readonly bool _feminine;

    private FrenchConverter(FrenchConverter template, bool beforeScaleWord) : base(template)
    {
      _beforeScaleWord = beforeScaleWord;
    }

    private FrenchConverter(FrenchConverter template, bool beforeScaleWord, bool feminine) : base(template)
    {
      _beforeScaleWord = beforeScaleWord;
      _feminine = feminine;
    }

    /// <summary>"la livre" is the pound; "le livre" is a book. The numeral has to agree.</summary>
    protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
    {
      var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
      return new FrenchConverter(this, false, gender == GenderGroup.Feminine).ToText(num);
    }

    protected override void AppendUnits(long num, StringBuilder builder)
    {
      if (_feminine && num == 1) builder.AppendFormat("{0} ", NumberTexts[1][2]);
      else base.AppendUnits(num, builder);
    }

    protected override long Append(long num, long scale, StringBuilder builder)
    {
      if (num > scale - 1)
      {
        var baseScale = num / scale;

        if (scale != 1000 || baseScale != 1)
        {
          // "cent" and "quatre-vingt" drop their -s when a scale word follows:
          // "deux cents" but "deux cent mille".
          new FrenchConverter(this, true).AppendLessThanOneThousand(baseScale, builder);
        }

        // million and milliard are nouns and take -s; mille is invariable.
        var plural = baseScale > 1 && ScaleTexts[scale].Length > 1;
        builder.AppendFormat("{0} ", ScaleTexts[scale][plural ? 1 : 0]);
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
          // "quatre-vingts" alone, "quatre-vingt mille" before a scale word.
          builder.AppendFormat("{0} ", NumberTexts[num][_beforeScaleWord ? 0 : 1]);
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
        else
        {
          var etUnList = new long[] { 21, 31, 41, 51, 61 };
          if (etUnList.Contains(num))
          {
            builder.AppendFormat("{0} {1} ", NumberTexts[tens][0], NumberTexts[num - tens][_feminine && num - tens == 1 ? 3 : 1]);
            return 0;
          }

          // Compound numbers below a hundred are hyphenated: "quarante-deux".
          builder.Append(NumberTexts[tens][0]);
          builder.Append(num - tens > 0 ? "-" : " ");
        }

        num = num - tens;
      }
      return num;
    }

    protected override long AppendHundreds(long num, StringBuilder builder)
    {
        if (num > 99)
        {
            var hundreds = num / 100;
            var rest = num - (hundreds * 100);
            if (hundreds != 1)
            {
                // Multiplied "cent" takes -s only when nothing follows it — not another
                // numeral, and not a scale word: "deux cents", "deux cent un",
                // "deux cent mille".
                var takesPlural = rest == 0 && !_beforeScaleWord;
                builder.AppendFormat("{0} {1} ", NumberTexts[hundreds][0], NumberTexts[100][takesPlural ? 1 : 0]);
            }
            else
                builder.AppendFormat("{0} ", NumberTexts[100][0]);
            num = rest;
        }
        return num;
    }

    private void Initialize()
    {
      NumberTexts.Add(0, new[] { "zéro" });
      NumberTexts.Add(1, new[] { "un", "et un", "une", "et une" });
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
      NumberTexts.Add(50, new[] { "cinquante" });
      NumberTexts.Add(60, new[] { "soixante" });
      NumberTexts.Add(80, new[] { "quatre-vingt", "quatre-vingts" });
      NumberTexts.Add(100, new[] { "cent", "cents" });

      ScaleTexts.Add(1000000000, new[] { "milliard", "milliards" });
      ScaleTexts.Add(1000000, new[] { "million", "millions" });
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
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centime", "centimes" } }
          };
        case Currency.GBP:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "livre sterling", "livres sterling" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "penny", "pence" } }
          };
        case Currency.ARS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "peso argentin", "pesos argentins" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
        case Currency.BGN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "lev bulgare", "levs bulgares" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "stotinka", "stotinki" } }
          };
        case Currency.BRL:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "réal brésilien", "réals brésiliens" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
        case Currency.UZS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "sum ouzbek", "sums ouzbeks" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "tiyin", "tiyin" } }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "dollar", "dollars" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centime", "centimes" } }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "rouble", "roubles" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopeck", "kopecks" } }
          };
        case Currency.TRY:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "livre turque", "livres turques" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kuruş", "kuruş" } }
          };
        case Currency.UAH:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "hryvnia ukrainienne", "hryvnias ukrainiennes" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopiyka", "kopiyka" } }
          };
        case Currency.ETB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "Birr", "Birr" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centime", "centimes" } }
          };
        case Currency.PLN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "zloty", "zlotys" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "grosz", "groszys" } }
          };
        case Currency.BYN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "rouble biélorusse", "roubles biélorusses" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopeck", "kopecks" } }
          };
      }
      return null;
    }
  }
}