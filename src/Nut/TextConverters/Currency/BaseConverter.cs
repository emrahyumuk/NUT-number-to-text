using System;
using System.Linq;
using System.Text;
using Nut.Models;

namespace Nut.TextConverters 
{
    public abstract partial class BaseConverter 
    {
        public virtual string ToText(decimal num, Currency currency) 
        {

            var builder = new StringBuilder();
            var currencyModel = GetCurrencyModel(currency);
            var decimalSeperator = num.ToString().Contains(",") ? ',' : '.';
            var nums = num.ToString().Split(decimalSeperator);

            var mainNum = Convert.ToInt64(nums[0]);
            builder.Append(ToText(mainNum));
            builder.Append(" ");
            builder.Append(GetCurrencyText(mainNum, currencyModel));

            if (nums.Count() > 1 && !string.IsNullOrEmpty(nums[1])) 
            {
                var childNum = Convert.ToInt64(nums[1].Substring(0, 2));
                if (childNum != 0) 
                {
                    builder.Append(" ");
                    builder.Append(ToText(childNum));
                    builder.Append(" ");
                    builder.Append(GetChildCurrencyText(childNum, currencyModel));
                }

            }

            return builder.ToString().Trim();
        }

        protected virtual string GetCurrencyText(long num, CurrencyModel currency) 
        {
            return num > 1 ? currency.Names[1] : currency.Names[0];
        }

        protected virtual string GetChildCurrencyText(long num, CurrencyModel currency) 
        {
            return num > 1 ? currency.ChildCurrency.Names[1] : currency.ChildCurrency.Names[0];
        }
        protected virtual CurrencyModel GetCurrencyModel(Currency currency) 
        {
            return null;
        }
    }
}
