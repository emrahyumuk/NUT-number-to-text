using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters
{
  public abstract class BaseConverter
  {

    protected Dictionary<long, string[]> NumberTexts;
    protected Dictionary<long, string[]> ScaleTexts;


    protected BaseConverter()
    {
      NumberTexts = new Dictionary<long, string[]>();
      ScaleTexts = new Dictionary<long, string[]>();
    }

    /// <summary>
    /// Shares another converter's word tables instead of rebuilding them. Used by the
    /// converters that need per-conversion state (gendered numerals) so they can keep that
    /// state on a short-lived instance without paying to rebuild the tables. The tables are
    /// shared, so a subclass using this must never write into them.
    /// </summary>
    protected BaseConverter(BaseConverter template)
    {
      NumberTexts = template.NumberTexts;
      ScaleTexts = template.ScaleTexts;
    }

    public abstract string CultureName { get; }

    /// <summary>Word placed in front of a negative amount.</summary>
    protected virtual string NegativeSign => "minus";

    public virtual string ToText(long num, GenderGroup genderGroup = GenderGroup.None)
    {
      NumberLimitControl(num);

      // Every Append* helper is guarded by "num > x", so a negative number matches none of
      // them and falls through to an empty string. Handle the sign once, here, and let the
      // rest of the pipeline only ever see a positive number.
      if (num < 0) return NegativeSign + " " + ToText(-num, genderGroup);

      var builder = new StringBuilder();

      if (num == 0)
      {
        builder.Append(NumberTexts[num][0]);
        return builder.ToString();
      }

      foreach (var scale in ScaleTexts)
        num = Append(num, scale.Key, builder);
      AppendLessThanOneThousand(num, builder);

      return builder.ToString().Trim();
    }

    protected virtual string ToText(long num)
    {
      return ToText(num, GenderGroup.None);
    }

    protected virtual string ToText(long num, CurrencyModel currencyModel, bool isMainUnit)
    {
      return ToText(num);
    }

    protected virtual string ToText(long num, CurrencyModel currencyModel, bool isMainUnit, GenderGroup genderGroup = GenderGroup.None)
    {
      return ToText(num, genderGroup);
    }

    protected virtual long Append(long num, long scale, StringBuilder builder)
    {
      if (num > scale - 1)
      {
        var baseScale = num / scale;
        AppendLessThanOneThousand(baseScale, builder);
        builder.AppendFormat("{0} ", ScaleTexts[scale][0]);
        num = num - (baseScale * scale);
      }
      return num;
    }

    protected virtual void AppendLessThanOneThousand(long num, StringBuilder builder)
    {
      num = AppendHundreds(num, builder);
      num = AppendTens(num, builder);
      AppendUnits(num, builder);
    }

    protected virtual void AppendUnits(long num, StringBuilder builder)
    {
      if (num > 0)
      {
        builder.AppendFormat("{0} ", NumberTexts[num][0]);
      }
    }

    protected virtual void AppendUnitsForAdditional(long num, StringBuilder builder)
    {
      if (num > 0)
      {
        builder.AppendFormat("{0} ", NumberTexts[num][0]);
      }
    }

    protected virtual long AppendTens(long num, StringBuilder builder)
    {
      if (num > 20)
      {
        var tens = ((int)(num / 10)) * 10;
        builder.AppendFormat("{0} ", NumberTexts[tens][0]);
        num = num - tens;
      }
      return num;
    }

    protected virtual long AppendHundreds(long num, StringBuilder builder)
    {
      if (num > 99)
      {
        var hundreds = num / 100;
        builder.AppendFormat("{0} {1} ", NumberTexts[hundreds][0], NumberTexts[100][0]);
        num = num - (hundreds * 100);
      }
      return num;
    }

    private static void NumberLimitControl(long num)
    {
      // Checked on both sides so that negating below cannot overflow at long.MinValue.
      if (num >= Parameters.NumberLimit || num <= -Parameters.NumberLimit)
      {
        throw new ArgumentOutOfRangeException(nameof(num), num,
          string.Format("Only numbers between {0} and {1} are supported.",
            -Parameters.NumberLimit + 1, Parameters.NumberLimit - 1));
      }
    }

    #region Currency

    public virtual string ToText(decimal num, string currency, Options options)
    {
      return ToText(num, currency, options, GenderGroup.None);
    }

    public virtual string ToText(decimal num, string currency, Options options, GenderGroup genderGroup = GenderGroup.None)
    {
      var builder = new StringBuilder();

      // Currency codes are compared against lower-case constants, so "USD" used to miss
      // every case and return "" while "usd" worked.
      currency = currency == null ? null : currency.ToLowerInvariant();
      if (currency == Currency.TL) currency = Currency.TRY;
      if (currency == Currency.RUR) currency = Currency.RUB;
      var currencyModel = GetCurrencyModel(currency);
      if (currencyModel == null) return string.Empty;

      // Take the sign off before splitting. Otherwise "-41.5" splits into "-41" and "5",
      // and the minus is lost somewhere in the integer part while the fraction survives,
      // which produced a plausible-looking but completely wrong amount.
      var isNegative = num < 0;
      if (isNegative) num = -num;

      // Reduce to the sub-unit before splitting the number into text. Without this a third
      // decimal was read as whole sub-units (123.456 became "four hundred fifty six
      // cents"), and because decimal preserves trailing zeros, 1.100 and 1.10 disagreed.
      // Rounding away from zero is the convention for money and carries into the main unit
      // where it should: 1.999 is two. Truncating is available for callers who need the
      // digits dropped rather than carried.
      num = options.SubUnitTruncated
        ? Math.Truncate(num * 100) / 100
        : Math.Round(num, 2, MidpointRounding.AwayFromZero);

      // An amount too small to register, like -0.001, rounds to zero; "minus zero" is not
      // an amount anyone writes.
      if (num == 0) isNegative = false;

      var decimalSeperator = num.ToString(CultureInfo.InvariantCulture).Contains(",") ? ',' : '.';
      var nums = num.ToString(CultureInfo.InvariantCulture).Split(decimalSeperator).ToList();
      if (nums.Count == 1) nums.Add("00");

      var mainUnitNum = Convert.ToInt64(nums[0]);

      if (options.MainUnitNotConvertedToText)
      {
        builder.Append(nums[0]);
      }
      else
      {
        var mainUnitText = ToText(mainUnitNum, currencyModel, true);
        // Only capitalise here when this really is the first word. On a negative amount
        // the sign comes first, so capitalising the main unit would put the capital in the
        // middle: "minus Forty-one dollars" instead of "Minus forty-one dollars".
        mainUnitText = options.MainUnitFirstCharUpper && !isNegative
          ? mainUnitText.ToFirstLetterUpper(CultureName)
          : mainUnitText;
        builder.Append(mainUnitText);
      }

      builder.Append(" ");

      var currencyText = GetCurrencyText(mainUnitNum, currencyModel);
      currencyText = options.CurrencyFirstCharUpper ? currencyText.ToFirstLetterUpper(CultureName) : currencyText;
      builder.Append(currencyText);


      if (nums.Count > 1 && !string.IsNullOrEmpty(nums[1]))
      {
        var subUnitText = nums[1].PadRight(2, '0');
        var subUnitNum = Convert.ToInt64(subUnitText);
        if (!options.SubUnitZeroNotDisplayed || subUnitNum != 0)
        {
          builder.Append(GetUnitSeparator(currencyModel));

          if (options.SubUnitNotConvertedToText)
          {
            builder.Append(subUnitText);
          }
          else
          {
            subUnitText = ToText(subUnitNum, currencyModel, false);
            subUnitText = options.SubUnitFirstCharUpper ? subUnitText.ToFirstLetterUpper(CultureName) : subUnitText;
            builder.Append(subUnitText);
          }

          builder.Append(" ");

          var subUnitCurrencyText = GetSubUnitCurrencyText(subUnitNum, currencyModel);
          subUnitCurrencyText = options.CurrencyFirstCharUpper ? subUnitCurrencyText.ToFirstLetterUpper(CultureName) : subUnitCurrencyText;
          builder.Append(subUnitCurrencyText);
        }

      }

      var text = builder.ToString().Trim();
      if (!isNegative) return text;

      var sign = options.MainUnitFirstCharUpper ? NegativeSign.ToFirstLetterUpper(CultureName) : NegativeSign;
      return sign + " " + text;
    }

    protected virtual string GetCurrencyText(long num, CurrencyModel currency)
    {
      return num > 1 ? currency.Names[1] : currency.Names[0];
    }

    protected virtual string GetSubUnitCurrencyText(long num, CurrencyModel currency)
    {
      return num > 1 ? currency.SubUnitCurrency.Names[1] : currency.SubUnitCurrency.Names[0];
    }
    protected virtual CurrencyModel GetCurrencyModel(string currency)
    {
      return null;
    }

    protected virtual string GetUnitSeparator(CurrencyModel currency)
    {
      return " ";
    }
    #endregion
  }
}
