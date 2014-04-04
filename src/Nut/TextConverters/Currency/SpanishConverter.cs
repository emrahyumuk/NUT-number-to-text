using Nut.Constants;
using Nut.Models;

namespace Nut.TextConverters 
{
    public sealed partial class SpanishConverter 
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
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "céntimo de euro", "céntimos de euro" } }
                    };
                case Currency.USD:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "dólar", "dólares" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "rublo", "rublos" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "lira turco", "liras turco" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
            }
            return null;
        }
    }
}
