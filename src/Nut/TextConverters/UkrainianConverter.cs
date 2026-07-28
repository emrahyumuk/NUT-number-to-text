using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  public sealed class UkrainianConverter : BaseConverter
  {

    private static readonly Lazy<UkrainianConverter> Lazy = new Lazy<UkrainianConverter>(() => new UkrainianConverter());
    public static UkrainianConverter Instance { get { return Lazy.Value; } }

    public override string CultureName
    {
      get { return "uk-UA"; }
    }

    public UkrainianConverter()
    {
      Initialize();
    }

    // Set only on the short-lived per-conversion instance; null on the shared singleton.
    private string _one;
    private string _two;

    private UkrainianConverter(UkrainianConverter template, CurrencyModel currencyModel, bool isMainUnit)
      : base(template)
    {
      switch (currencyModel.Currency)
      {
        case Currency.BYN:
        case Currency.RUB:
        case Currency.EUR:
          _one = isMainUnit ? "Один" : "Одна";
          _two = isMainUnit ? "Два" : "Дві";
          break;
        default:
          // Feminine, which suits гривня but not долар — currency gender is not modelled
          // yet, so USD still comes out as "Одна доллар". Tracked separately; note this
          // is deliberately not the masculine entry [0] of the word table.
          _one = "Одна";
          _two = "Дві";
          break;
      }
    }

    protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
    {
      // Gendered forms are per-conversion; see RussianConverter for why this is not
      // written into the shared singleton.
      return new UkrainianConverter(this, currencyModel, isMainUnit).ToText(num);
    }

    protected override void AppendUnits(long num, StringBuilder builder)
    {
      if (!AppendGendered(num, builder)) base.AppendUnits(num, builder);
    }

    // Append only routes here for scale 1000, and the count before тисяча agrees with
    // тисяча, which is feminine whatever currency is being counted. Entry [1] is feminine.
    protected override void AppendUnitsForAdditional(long num, StringBuilder builder)
    {
      if (num == 1 || num == 2) builder.AppendFormat("{0} ", NumberTexts[num][1]);
      else base.AppendUnitsForAdditional(num, builder);
    }

    private bool AppendGendered(long num, StringBuilder builder)
    {
      var word = num == 1 ? _one : num == 2 ? _two : null;
      if (word == null) return false;
      builder.AppendFormat("{0} ", word);
      return true;
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
          AppendLessThanOneThousandForScale(baseScale, builder);
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

    // мільйон and мільярд are masculine, so their count must not pick up the currency's
    // gender the way the trailing unit does: "один мільйон гривень", not "одна мільйон".
    private void AppendLessThanOneThousandForScale(long num, StringBuilder builder)
    {
      num = AppendHundreds(num, builder);
      num = AppendTens(num, builder);
      base.AppendUnits(num, builder);
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
          return 2;
      }
      return 3;
    }

    private void Initialize()
    {
      NumberTexts.Add(0, new[] { "Нуль" });
      // Masculine first, like the other Slavic converters: entry [0] is the bare form,
      // entry [1] the feminine one used before тисяча.
      NumberTexts.Add(1, new[] { "Один", "Одна" });
      NumberTexts.Add(2, new[] { "Два", "Дві" });
      NumberTexts.Add(3, new[] { "Три" });
      NumberTexts.Add(4, new[] { "Чотири" });
      NumberTexts.Add(5, new[] { "П'ять" });
      NumberTexts.Add(6, new[] { "Шість" });
      NumberTexts.Add(7, new[] { "Сім" });
      NumberTexts.Add(8, new[] { "Вісім" });
      NumberTexts.Add(9, new[] { "Дев'ять" });
      NumberTexts.Add(10, new[] { "Десять" });
      NumberTexts.Add(11, new[] { "Одинадцять" });
      NumberTexts.Add(12, new[] { "Дванадцять" });
      NumberTexts.Add(13, new[] { "Тринадцять" });
      NumberTexts.Add(14, new[] { "Чотирнадцять" });
      NumberTexts.Add(15, new[] { "П'ятнадцять" });
      NumberTexts.Add(16, new[] { "Шістнадцять" });
      NumberTexts.Add(17, new[] { "Сімнадцять" });
      NumberTexts.Add(18, new[] { "Вісімнадцять" });
      NumberTexts.Add(19, new[] { "Дев'ятнадцять" });
      NumberTexts.Add(20, new[] { "Двадцять" });
      NumberTexts.Add(30, new[] { "Тридцять" });
      NumberTexts.Add(40, new[] { "Сорок" });
      NumberTexts.Add(50, new[] { "П'ятдесят" });
      NumberTexts.Add(60, new[] { "Шістдесят" });
      NumberTexts.Add(70, new[] { "Сімдесят" });
      NumberTexts.Add(80, new[] { "Вісімдесят" });
      NumberTexts.Add(90, new[] { "Дев'яносто" });
      NumberTexts.Add(100, new[] { "Сто" });
      NumberTexts.Add(200, new[] { "Двісті" });
      NumberTexts.Add(300, new[] { "Триста" });
      NumberTexts.Add(400, new[] { "Чотириста" });
      NumberTexts.Add(500, new[] { "П'ятсот" });
      NumberTexts.Add(600, new[] { "Шістсот" });
      NumberTexts.Add(700, new[] { "Сімсот" });
      NumberTexts.Add(800, new[] { "Вісімсот" });
      NumberTexts.Add(900, new[] { "Дев'ятсот" });

      ScaleTexts.Add(1000000000, new[] { "мільярдів", "мільярд", "мільярди" });
      ScaleTexts.Add(1000000, new[] { "мільйонів", "мільйон", "мільйони" });
      ScaleTexts.Add(1000, new[] { "тисяч", "тисяча", "тисячі" });

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
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копійка", "копійки", "копійок" } }
          };
        case Currency.ETB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "берр", "берр" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "копійка", "копійки", "копійок" } }
          };
        case Currency.PLN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "злотий", "злотий", "злотих" },
            SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "грубий", "грубий", "груба" } }
          };
      }
      return null;
    }

    #endregion
  }
}
