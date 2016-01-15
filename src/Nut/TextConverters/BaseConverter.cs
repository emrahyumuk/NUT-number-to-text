using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters {
    public abstract class BaseConverter {

        protected Dictionary<long, string> TextStrings;
        protected Dictionary<long, string> AdditionalStrings;
        protected Dictionary<long, string> Scales;
        protected Dictionary<long, string> AdditionalScales;

        protected BaseConverter() {
            TextStrings = new Dictionary<long, string>();
            AdditionalStrings = new Dictionary<long, string>();
            Scales = new Dictionary<long, string>();
            AdditionalScales = new Dictionary<long, string>();
        }

        public abstract string CultureName { get; }

        public virtual string ToText(long num) {
            NumberLimitControl(num);

            var builder = new StringBuilder();

            if (num == 0) {
                builder.Append(TextStrings[num]);
                return builder.ToString();
            }

            foreach (var scale in Scales)
                num = Append(num, scale.Key, builder);
            AppendLessThanOneThousand(num, builder);

            return builder.ToString().Trim();
        }

        protected virtual string ToText(long num, CurrencyModel currencyModel, bool isMainUnit) {
            return ToText(num);
        }

        protected virtual long Append(long num, long scale, StringBuilder builder) {
            if (num > scale - 1) {
                var baseScale = num / scale;
                AppendLessThanOneThousand(baseScale, builder);
                builder.AppendFormat("{0} ", Scales[scale]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        protected virtual void AppendLessThanOneThousand(long num, StringBuilder builder) {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnits(num, builder);
        }

        protected virtual void AppendUnits(long num, StringBuilder builder) {
            if (num > 0) {
                builder.AppendFormat("{0} ", TextStrings[num]);
            }
        }

        protected virtual void AppendUnitsForAdditional(long num, StringBuilder builder) {
            if (num > 0) {
                builder.AppendFormat("{0} ", AdditionalStrings[num]);
            }
        }

        protected virtual long AppendTens(long num, StringBuilder builder) {
            if (num > 20) {
                var tens = ((int)(num / 10)) * 10;
                builder.AppendFormat("{0} ", TextStrings[tens]);
                num = num - tens;
            }
            return num;
        }

        protected virtual long AppendHundreds(long num, StringBuilder builder) {
            if (num > 99) {
                var hundreds = num / 100;
                builder.AppendFormat("{0} {1} ", TextStrings[hundreds], TextStrings[100]);
                num = num - (hundreds * 100);
            }
            return num;
        }

        private static void NumberLimitControl(long num) {
            if (num >= Parameters.NumberLimit) {
                throw new Exception(string.Format("{0} and larger than {0} numbers are not supported", Parameters.NumberLimit));
            }
        }

        #region Currency

        public virtual string ToText(decimal num, string currency, Options options) {
            var builder = new StringBuilder();
            if (currency == Currency.TL) currency = Currency.TRY;
            var currencyModel = GetCurrencyModel(currency);
            if (currencyModel == null) return string.Empty;
            var decimalSeperator = num.ToString(CultureInfo.InvariantCulture).Contains(",") ? ',' : '.';
            var nums = num.ToString(CultureInfo.InvariantCulture).Split(decimalSeperator).ToList();
            if (nums.Count == 1) nums.Add("00");

            var mainUnitNum = Convert.ToInt64(nums[0]);

            if (options.MainUnitNotConvertedToText) {
                builder.Append(nums[0]);
            }
            else {
                var mainUnitText = ToText(mainUnitNum, currencyModel, true);
                mainUnitText = options.MainUnitFirstCharUpper ? mainUnitText.ToFirstLetterUpper(CultureName) : mainUnitText;
                builder.Append(mainUnitText);
            }

            builder.Append(" ");

            var currencyText = GetCurrencyText(mainUnitNum, currencyModel);
            currencyText = options.CurrencyFirstCharUpper ? currencyText.ToFirstLetterUpper(CultureName) : currencyText;
            builder.Append(currencyText);


            if (nums.Count > 1 && !string.IsNullOrEmpty(nums[1])) {
                var subUnitText = nums[1].Length == 1 ? nums[1] + "0" : nums[1];

                var subUnitNum = Convert.ToInt64(nums[1].Substring(0, nums[1].Length));
                if (!options.SubUnitZeroNotDisplayed || subUnitNum != 0) {
                    builder.Append(" ");

                    if (options.SubUnitNotConvertedToText) {
                        builder.Append(subUnitText);
                    }
                    else {
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

        protected virtual string GetCurrencyText(long num, CurrencyModel currency) {
            return num > 1 ? currency.Names[1] : currency.Names[0];
        }

        protected virtual string GetSubUnitCurrencyText(long num, CurrencyModel currency) {
            return num > 1 ? currency.SubUnitCurrency.Names[1] : currency.SubUnitCurrency.Names[0];
        }
        protected virtual CurrencyModel GetCurrencyModel(string currency) {
            return null;
        }
        #endregion
    }
}
