using Nut.Models;

namespace Nut.TextConverters 
{
    internal sealed partial class RussianConverter 
    {

        public override string GetCurrencyText(long num, CurrencyModel currency) 
        {
            var textType = GetTextType(num);
            return currency.Names[textType-1];
        }

        public override string GetChildCurrencyText(long num, CurrencyModel currency) 
        {
            var textType = GetTextType(num);
            return currency.ChildCurrency.Names[textType-1];
        }

        public override CurrencyModel GetCurrencyModel(Currency currency) 
        {
            switch (currency) 
            {
                case Currency.eur:
                    return new CurrencyModel 
                    {
                        Currency = Currency.eur,
                        Names = new[] { "евро", "евро", "евро" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "евроцент", "евроцента", "евроцентов" } }
                    };
                case Currency.usd:
                    return new CurrencyModel 
                    {
                        Currency = Currency.usd,
                        Names = new[] { "доллар", "доллара", "долларов" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "цент", "цента", "центов" } }
                    };
                case Currency.rub:
                    return new CurrencyModel 
                    {
                        Currency = Currency.rub,
                        Names = new[] { "рубль", "рубля", "рублей" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
                    };
                case Currency.tl:
                    return new CurrencyModel 
                    {
                        Currency = Currency.tl,
                        Names = new[] { "турецкая лира", "турецких лир", "турецких лир" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруша", "курушей" } }
                    };
            }
            return null;
        }
    }
}
