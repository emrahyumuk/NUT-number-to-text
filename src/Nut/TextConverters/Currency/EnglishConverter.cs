using Nut.Constants;
using Nut.Models;

namespace Nut.TextConverters 
{
    public sealed partial class EnglishConverter 
    {
        protected override CurrencyModel GetCurrencyModel(string currency) 
        {
            switch (currency) 
            {
                case Currency.EUR:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "euro", "euros" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "eurocent", "eurocents" } }
                    };
                case Currency.USD:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "dollar", "dollars" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "cent", "cents" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "ruble", "rubles" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "turkish lira", "turkish lira" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kurus", "kurus" } }
                    };
            }
            return null;
        }
    }
}
