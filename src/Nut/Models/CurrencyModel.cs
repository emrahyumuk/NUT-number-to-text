namespace Nut.Models 
{
    public class CurrencyModel : BaseCurrencyModel 
    {
        public string Currency { get; set; }
        public BaseCurrencyModel ChildCurrency { get; set; }
    }
}
