using Nut.Models;

namespace Nut.TextConverters 
{
    public sealed partial class EnglishConverter 
    {
        protected override CurrencyModel GetCurrencyModel(Currency currency) 
        {
            switch (currency) 
            {
                case Currency.eur:
                    return new CurrencyModel 
                    {
                        Currency = Currency.eur,
                        Names = new[] { "euro", "euros" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "eurocent", "eurocents" } }
                    };
                case Currency.usd:
                    return new CurrencyModel 
                    {
                        Currency = Currency.usd,
                        Names = new[] { "dollar", "dollars" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "cent", "cents" } }
                    };
                case Currency.rub:
                    return new CurrencyModel 
                    {
                        Currency = Currency.rub,
                        Names = new[] { "ruble", "rubles" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.tl:
                    return new CurrencyModel 
                    {
                        Currency = Currency.tl,
                        Names = new[] { "turkish lira", "turkish lira" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kurus", "kurus" } }
                    };
            }
            return null;
        }
    }
}
