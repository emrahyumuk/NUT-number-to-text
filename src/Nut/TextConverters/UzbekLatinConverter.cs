using System;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{ 
  public class UzbekLatinConverter: BaseConverter
    {
        private static readonly Lazy<UzbekLatinConverter> Lazy = new Lazy<UzbekLatinConverter>(() => new UzbekLatinConverter());
        public static UzbekLatinConverter Instance => Lazy.Value;

        public override string CultureName => Culture.Russian;

        public UzbekLatinConverter()
        {
          Initialize();
        }

      protected override string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
      {
        switch (currencyModel.Currency)
        {
           
          case Currency.RUB:
            NumberTexts[1][0] = "Bir";
            NumberTexts[2][0] = "Ikki";
            break;
          default:
            NumberTexts[1][0] = "Bir";
            NumberTexts[2][0] = "Ikki";
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
          if (scale == 1000 && textType < 3 && (baseScale == 1 || baseScale == 2))
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

          num -= (baseScale * scale);
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
        NumberTexts.Add(0, new[] { "No'l" });
        NumberTexts.Add(1, new[] { "Bir"});
        NumberTexts.Add(2, new[] { "Ikki"});
        NumberTexts.Add(3, new[] { "Uch" });
        NumberTexts.Add(4, new[] { "To'rt" });
        NumberTexts.Add(5, new[] { "Besh" });
        NumberTexts.Add(6, new[] { "Olti" });
        NumberTexts.Add(7, new[] { "Yetti" });
        NumberTexts.Add(8, new[] { "Sakkiz" });
        NumberTexts.Add(9, new[] { "To'qqiz" });
        NumberTexts.Add(10, new[] { "O'n" });
        NumberTexts.Add(11, new[] { "O'n Bir" });
        NumberTexts.Add(12, new[] { "O'n Ikki" });
        NumberTexts.Add(13, new[] { "o'n Uch" });
        NumberTexts.Add(14, new[] { "O'n To'rt" });
        NumberTexts.Add(15, new[] { "O'n Besh" });
        NumberTexts.Add(16, new[] { "O'n Olti" });
        NumberTexts.Add(17, new[] { "O'n Yetti" });
        NumberTexts.Add(18, new[] { "O'n Sakkiz" });
        NumberTexts.Add(19, new[] { "O'n To'qqiz" });
        NumberTexts.Add(20, new[] { "Yigirma" });
        NumberTexts.Add(30, new[] { "O'ttiz" });
        NumberTexts.Add(40, new[] { "Qiriq" });
        NumberTexts.Add(50, new[] { "Ellik" });
        NumberTexts.Add(60, new[] { "Oltmush" });
        NumberTexts.Add(70, new[] { "Yetmush" });
        NumberTexts.Add(80, new[] { "Sakson" });
        NumberTexts.Add(90, new[] { "To'qson" });
        NumberTexts.Add(100, new[] { "Bir Yuz" , "Yuz" });
        NumberTexts.Add(200, new[] { "Ikki Yuz" });
        NumberTexts.Add(300, new[] { "Uch Yuz" });
        NumberTexts.Add(400, new[] { "To'rt Yuz" });
        NumberTexts.Add(500, new[] { "Besh Yuz" });
        NumberTexts.Add(600, new[] { "Olti Yuz" });
        NumberTexts.Add(700, new[] { "Yetti Yuz" });
        NumberTexts.Add(800, new[] { " Sakkiz Yuz" });
        NumberTexts.Add(900, new[] { "To'qqiz Yuz" });

        ScaleTexts.Add(1000000000, new[] { "Milliard", "Milliard", "Milliard" });
        ScaleTexts.Add(1000000, new[] { "Million", "Million", "Million" });
        ScaleTexts.Add(1000, new[] { "Ming", "Ming", "Ming" });
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
          case Currency.USD:
            return new CurrencyModel
            {
              Currency = currency,
              Names = new[] { "Do'llar", "Do'llar", "Do'llar" },
              SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Sent", "Sent", "Sent" } }
            };
          case Currency.RUB:
            return new CurrencyModel
            {
              Currency = currency,
              Names = new[] { "Rubl", "Rubl", "Rubl" },
              SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Kopeyka", "Kopeyka", "Kopeyka" } }
            };
          case Currency.UZB:
            return new CurrencyModel
            {
              Currency = currency,
              Names = new[] { "So'm", "So'm", "So'm" },
              SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "Tiyin", "Tiyin", "Tiyin" } }
            };
        }
        return null;
      }

      #endregion
    }
}