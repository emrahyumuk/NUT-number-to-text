using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  public sealed class SpanishConverter : BaseConverter
  {
    private static readonly Lazy<SpanishConverter> Lazy = new Lazy<SpanishConverter>(() => new SpanishConverter());
    public static SpanishConverter Instance => Lazy.Value;

    public override string CultureName => Culture.Spanish;

    protected override string NegativeSign => "menos";

    public SpanishConverter()
    {
      Initialize();
    }

    // Set only on the short-lived per-conversion instance used for money.
    private readonly bool _beforeNoun;
    private readonly bool _feminine;

    private SpanishConverter(SpanishConverter template, bool beforeNoun, bool feminine) : base(template)
    {
      _beforeNoun = beforeNoun;
      _feminine = feminine;
    }

    /// <summary>
    /// An amount is followed by the currency name, and a numeral in front of a noun takes
    /// its apocopated form: "un euro" and "veintiún euros", not "uno euro".
    /// </summary>
    protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
    {
      var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
      return new SpanishConverter(this, true, gender == GenderGroup.Feminine).ToText(num);
    }

    /// <summary>
    /// Entry [1] is the apocopated form and [2] the feminine one, so "un euro" but
    /// "una libra esterlina".
    /// </summary>
    protected override void AppendUnits(long num, StringBuilder builder)
    {
      if (_beforeNoun && (num == 1 || num == 21))
        builder.AppendFormat("{0} ", NumberTexts[num][_feminine ? 2 : 1]);
      else
        base.AppendUnits(num, builder);
    }

    protected override long Append(long num, long scale, StringBuilder builder)
    {
      if (num > scale - 1)
      {
        var baseScale = num / scale;

        // "mil" and "mil millones" are said without a leading one — "mil euros", never
        // "un mil euros" — while "un millón" keeps it. Everything in front of a scale word
        // is apocopated: "cuarenta y un mil", not "cuarenta y uno mil".
        var omitsLeadingOne = scale == 1000 || scale == 1000000000;
        if (!(baseScale == 1 && omitsLeadingOne))
        {
          // "mil" is an adjective and does not interrupt the agreement with what is being
          // counted — "doscientas mil libras". "millón" and "mil millones" are masculine
          // nouns and take their own: "doscientos millones de libras".
          AppendLessThanOneThousandApocopated(baseScale, builder, _feminine && scale == 1000);
        }

        if (scale == 1000000 && num / scale > 1)
        {
          builder.AppendFormat("{0} ", ScaleTexts[scale][2]);
        }
        else
        {
          builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
        }

        num = num - (baseScale * scale);
      }
      return num;
    }

    /// <summary>
    /// Entry [1] of one and twenty-one is the apocopated form, which is what a numeral
    /// takes in front of a noun — and a scale word is a noun.
    /// </summary>
    private void AppendLessThanOneThousandApocopated(long num, StringBuilder builder, bool feminine)
    {
      num = AppendHundreds(num, builder, feminine);
      num = AppendTens(num, builder);
      if (num == 1 || num == 21) builder.AppendFormat("{0} ", NumberTexts[num][1]);
      else AppendUnits(num, builder);
    }

    protected override long AppendTens(long num, StringBuilder builder)
    {
      if (num > 30)
      {
        var tens = ((int)(num / 10)) * 10;

        builder.AppendFormat(num == tens ? "{0}" : "{0} y ", NumberTexts[tens][0]);

        num = num - tens;
      }
      return num;
    }

    protected override long AppendHundreds(long num, StringBuilder builder)
    {
      return AppendHundreds(num, builder, _feminine);
    }

    /// <summary>
    /// The hundreds agree in gender with what they count — "doscientas libras esterlinas" —
    /// and "cien" does not inflect for it at all, it only becomes "ciento" when something
    /// follows. Entry [1] means those two different things depending on the hundred, which
    /// is why the cases are split rather than sharing an index.
    /// </summary>
    private long AppendHundreds(long num, StringBuilder builder, bool feminine)
    {
      if (num > 99)
      {
        var hundreds = num / 100 * 100;

        builder.AppendFormat("{0} ", hundreds == 100
          ? NumberTexts[100][num % 100 > 0 ? 1 : 0]
          : NumberTexts[hundreds][feminine ? 1 : 0]);

        num = num - hundreds;
      }
      return num;
    }

    /// <summary>
    /// RAE: millón and millardo are nouns, and what they count is linked to them with
    /// "de" — "un millón de euros", never "un millón euros". mil is an adjective and takes
    /// none, so "mil euros" stays as it is. The preposition only appears when the amount
    /// ends on the scale noun; "un millón uno" is followed directly by the currency.
    /// </summary>
    protected override string GetCurrencyText(long num, CurrencyModel currency)
    {
      var name = base.GetCurrencyText(num, currency);
      return num >= 1000000 && num % 1000000 == 0 ? "de " + name : name;
    }

    protected override string GetUnitSeparator(CurrencyModel currency)
    {
      return " con ";
    }

    private void Initialize()
    {
      NumberTexts.Add(0, new[] { "cero" });
      NumberTexts.Add(1, new[] { "uno", "un", "una" });
      NumberTexts.Add(2, new[] { "dos" });
      NumberTexts.Add(3, new[] { "tres" });
      NumberTexts.Add(4, new[] { "cuatro" });
      NumberTexts.Add(5, new[] { "cinco" });
      NumberTexts.Add(6, new[] { "seis" });
      NumberTexts.Add(7, new[] { "siete" });
      NumberTexts.Add(8, new[] { "ocho" });
      NumberTexts.Add(9, new[] { "nueve" });
      NumberTexts.Add(10, new[] { "diez" });
      NumberTexts.Add(11, new[] { "once" });
      NumberTexts.Add(12, new[] { "doce" });
      NumberTexts.Add(13, new[] { "trece" });
      NumberTexts.Add(14, new[] { "catorce" });
      NumberTexts.Add(15, new[] { "quince" });
      NumberTexts.Add(16, new[] { "dieciséis", "dieciseis" });
      NumberTexts.Add(17, new[] { "diecisiete" });
      NumberTexts.Add(18, new[] { "dieciocho" });
      NumberTexts.Add(19, new[] { "diecinueve" });
      NumberTexts.Add(20, new[] { "veinte" });
      // [0] full form, [1] apocopated before a noun, [2] feminine — same shape as one.
      NumberTexts.Add(21, new[] { "veintiuno", "veintiún", "veintiuna" });
      NumberTexts.Add(22, new[] { "veintidós", "veintidos" });
      NumberTexts.Add(23, new[] { "veintitrés", "veintitres" });
      NumberTexts.Add(24, new[] { "veinticuatro" });
      NumberTexts.Add(25, new[] { "veinticinco" });
      NumberTexts.Add(26, new[] { "veintiséis", "veintiseis" });
      NumberTexts.Add(27, new[] { "veintisiete" });
      NumberTexts.Add(28, new[] { "veintiocho" });
      NumberTexts.Add(29, new[] { "veintinueve" });
      NumberTexts.Add(30, new[] { "treinta" });
      NumberTexts.Add(40, new[] { "cuarenta" });
      NumberTexts.Add(50, new[] { "cincuenta" });
      NumberTexts.Add(60, new[] { "sesenta" });
      NumberTexts.Add(70, new[] { "setenta" });
      NumberTexts.Add(80, new[] { "ochenta" });
      NumberTexts.Add(90, new[] { "noventa" });
      NumberTexts.Add(100, new[] { "cien", "ciento" });
      NumberTexts.Add(200, new[] { "doscientos", "doscientas" });
      NumberTexts.Add(300, new[] { "trescientos", "trescientas" });
      NumberTexts.Add(400, new[] { "cuatrocientos", "cuatrocientas" });
      NumberTexts.Add(500, new[] { "quinientos", "quinientas" });
      NumberTexts.Add(600, new[] { "seiscientos", "seiscientas" });
      NumberTexts.Add(700, new[] { "setecientos", "setecientas" });
      NumberTexts.Add(800, new[] { "ochocientos", "ochocientas" });
      NumberTexts.Add(900, new[] { "novecientos", "novecientas" });

      // RAE: "billón" is 10^12 in Spanish, not the English billion. 10^9 is "mil millones"
      // (or "millardo"), so the previous entry was out by a factor of a thousand.
      // One form only, like "mil": the plural entry is read for millón alone, and
      // "mil millones" is already plural.
      ScaleTexts.Add(1000000000, new[] { "mil millones" });
      ScaleTexts.Add(1000000, new[] { "millón", "millon", "millones" });
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
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "céntimo de euro", "céntimos de euro" } }
          };
        case Currency.GBP:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "libra esterlina", "libras esterlinas" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "penique", "peniques" } }
          };
        case Currency.BGN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "lev búlgaro", "levs búlgaros" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "stotinka", "stotinki" } }
          };
        case Currency.BRL:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "real brasileño", "reales brasileños" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
        case Currency.UZS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "som uzbeko", "soms uzbekos" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "tiyin", "tiyin" } }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "dólar", "dólares" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "rublo", "rublos" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopek", "kopeks" } }
          };
        case Currency.TRY:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "lira turco", "liras turco" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kuruş", "kuruş" } }
          };
        case Currency.UAH:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "grivna ucraniana", "grivnas ucraniana" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopek", "kopeks" } }
          };
        case Currency.ETB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "birr", "birr" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
        case Currency.PLN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "zloty", "zloty" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "groszy", "groszy" } }
          };
        case Currency.BYN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "rublo bielorruso", "rublos bielorrusos" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "kopek", "kopeks" } }
          };
        case Currency.ARS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "peso", "pesos" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "centavo", "centavos" } }
          };
      }
      return null;
    }
  }
}
