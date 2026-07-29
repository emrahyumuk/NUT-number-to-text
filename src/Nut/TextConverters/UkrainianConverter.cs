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

    protected override string NegativeSign => "мінус";

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
      var gender = isMainUnit ? currencyModel.Gender : currencyModel.SubUnitCurrency.Gender;
      var feminine = gender == GenderGroup.Feminine;
      _one = feminine ? "Одна" : "Один";
      _two = feminine ? "Дві" : "Два";
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
            Names = new[] { "євро", "євро", "євро" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "євроцент", "євроценти", "євроцентів" } }
          };
        case Currency.GBP:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "фунт стерлінгів", "фунти стерлінгів", "фунтів стерлінгів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "пенні", "пенні", "пенні" } }
          };
        case Currency.ARS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "аргентинське песо", "аргентинських песо", "аргентинських песо" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентаво", "сентаво", "сентаво" } }
          };
        case Currency.BGN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "лев", "леви", "левів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "стотинка", "стотинки", "стотинок" } }
          };
        case Currency.BRL:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "бразильський реал", "бразильські реали", "бразильських реалів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "сентаво", "сентаво", "сентаво" } }
          };
        case Currency.BYN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "білоруський рубль", "білоруські рублі", "білоруських рублів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копійка", "копійки", "копійок" } }
          };
        case Currency.UZS:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "узбецький сум", "узбецькі суми", "узбецьких сумів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "тийин", "тийини", "тийинів" } }
          };
        case Currency.USD:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "долар", "долари", "доларів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "цент", "центи", "центів" } }
          };
        case Currency.RUB:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "рубль", "рублі", "рублів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копійка", "копійки", "копійок" } }
          };
        case Currency.TRY:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "турецька ліра", "турецькі ліри", "турецьких лір" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "куруш", "куруші", "курушів" } }
          };
        case Currency.UAH:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "гривня", "гривні", "гривень" },
            Gender = GenderGroup.Feminine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Feminine, Names = new[] { "копійка", "копійки", "копійок" } }
          };
        case Currency.ETB:
          return new CurrencyModel
          {
            Currency = currency,
            // Only two forms here previously, which threw IndexOutOfRangeException for
            // counts of five and above. The birr's sub unit is the cent, not the kopiyka.
            Names = new[] { "бир", "бири", "бирів" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "цент", "центи", "центів" } }
          };
        case Currency.PLN:
          return new CurrencyModel
          {
            Currency = currency,
            Names = new[] { "злотий", "злоті", "злотих" },
            Gender = GenderGroup.Masculine,
            SubUnitCurrency = new BaseCurrencyModel { Gender = GenderGroup.Masculine, Names = new[] { "гріш", "гроші", "грошів" } }
          };
      }
      return null;
    }

    #endregion
  }
}
