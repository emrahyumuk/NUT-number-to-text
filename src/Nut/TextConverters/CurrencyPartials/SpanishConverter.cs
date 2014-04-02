using Nut.Models;

namespace Nut.TextConverters 
{
    internal sealed partial class SpanishConverter 
    {
        public override CurrencyModel GetCurrencyModel(Currency currency) 
        {
            switch (currency) 
            {
                case Currency.eur:
                    return new CurrencyModel 
                    {
                        Currency = Currency.eur,
                        Names = new[] { "euro", "euros" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "céntimo de euro", "céntimos de euro" } }
                    };
                case Currency.usd:
                    return new CurrencyModel 
                    {
                        Currency = Currency.usd,
                        Names = new[] { "dólar", "dólares" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "centavo", "centavos" } }
                    };
                case Currency.rub:
                    return new CurrencyModel 
                    {
                        Currency = Currency.rub,
                        Names = new[] { "rublo", "rublos" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopeks" } }
                    };
                case Currency.tl:
                    return new CurrencyModel 
                    {
                        Currency = Currency.tl,
                        Names = new[] { "lira turco", "liras turco" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
            }
            return null;
        }
    }
}
