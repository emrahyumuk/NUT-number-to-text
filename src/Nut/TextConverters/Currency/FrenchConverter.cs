using Nut.Constants;
using Nut.Models;

namespace Nut.TextConverters 
{
    public sealed partial class FrenchConverter 
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
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "centime", "centimes" } }
                    };
                case Currency.USD:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "dollar", "dollars" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "centime", "centimes" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "rouble", "roubles" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopeck ", "kopecks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "livre turques", "livres turques" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
            }
            return null;
        }
    }
}
