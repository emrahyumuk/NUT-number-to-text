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

        public abstract string CultureName { get; }

        public virtual string ToText(long num, GenderGroup genderGroup = GenderGroup.None)
        {
            NumberLimitControl(num);

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
            if (num >= Parameters.NumberLimit)
            {
                throw new Exception(string.Format("{0} and larger than {0} numbers are not supported", Parameters.NumberLimit));
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
            if (currency == Currency.TL) currency = Currency.TRY;
            var currencyModel = GetCurrencyModel(currency);
            if (currencyModel == null) return string.Empty;
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
                mainUnitText = options.MainUnitFirstCharUpper ? mainUnitText.ToFirstLetterUpper(CultureName) : mainUnitText;
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

            return builder.ToString().Trim();
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

        protected virtual string GetNegativeSign()
        {
            return "";
        }
        public virtual long AddNegativeSign(long num, StringBuilder builder)
        {
            if (num < 0)
            {
                builder.AppendFormat(this.GetNegativeSign());
                builder.AppendFormat(" ");
                num = -num;
            }

            return num;
        }
    }
}
