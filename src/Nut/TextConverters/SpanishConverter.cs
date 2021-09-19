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

    public SpanishConverter()
    {
      Initialize();
    }

    public override string ToText(decimal num, string currency, Options options, GenderGroup genderGroup = GenderGroup.None)
    {
      return base.ToText(num, currency, options, genderGroup);
    }

    protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit, GenderGroup genderGroup = GenderGroup.None)
    {
      return base.ToText(num, currencyModel, isMainUnit, genderGroup);
    }

    protected override long Append(long num, long scale, StringBuilder builder)
    {
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
      if (num > 99)
      {
        var hundreds = num / 100 * 100;
         if (num % 100 > 0)
         {
           builder.AppendFormat("{0} ", NumberTexts[hundreds][1]);
         }
         else
         {
           builder.AppendFormat("{0} ", NumberTexts[hundreds][0]);
         }
                    
        num = num - hundreds;
      }
      return num;
    }

    protected override string GetUnitSeparator(CurrencyModel currency, bool addAndAsUnitSeparator)
    {
      return addAndAsUnitSeparator ? " con " : " ";
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
      NumberTexts.Add(21, new[] { "veintiún", "veintiun", "veintiuno", "veintiuna" });
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

      ScaleTexts.Add(1000000000, new[] { "billón", "billon", "billones" });
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
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "céntimo de euro", "céntimos de euro" } }
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
      }
      return null;
    }
  }
}
