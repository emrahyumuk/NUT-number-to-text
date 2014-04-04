using Nut.Constants;
using Nut.Models;

namespace Nut.TextConverters 
{
    public sealed partial class RussianConverter 
    {

        protected override string GetCurrencyText(long num, CurrencyModel currency) 
        {
            var textType = GetTextType(num);
            return currency.Names[textType-1];
        }

        protected override string GetChildCurrencyText(long num, CurrencyModel currency) 
        {
            var textType = GetTextType(num);
            return currency.ChildCurrency.Names[textType-1];
        }

        protected override CurrencyModel GetCurrencyModel(string currency) 
        {
            switch (currency) 
            {
                case Currency.EUR:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "евро", "евро", "евро" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "евроцент", "евроцента", "евроцентов" } }
                    };
                case Currency.USD:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "доллар", "доллара", "долларов" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "цент", "цента", "центов" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "рубль", "рубля", "рублей" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "копейка", "копейки", "копеек" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel 
                    {
                        Currency = currency,
                        Names = new[] { "турецкая лира", "турецких лир", "турецких лир" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "куруш", "куруша", "курушей" } }
                    };
            }
            return null;
        }
    }
}
