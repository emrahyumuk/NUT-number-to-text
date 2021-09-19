namespace Nut.Models 
{
    public class CurrencyModel : BaseCurrencyModel
    {
        public string Currency { get; set; }
        public BaseCurrencyModel SubUnitCurrency { get; set; }
    }
}
