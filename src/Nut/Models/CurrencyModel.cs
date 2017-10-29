namespace Nut.Models 
{
    public class CurrencyModel : BaseCurrencyModel 
    {
        public string Currency { get; set; }
        public BaseCurrencyModel SubUnitCurrency { get; set; }
        public string ShortUnitCurrency { get; set; }
        public string ShortSubUnitCurrency { get; set; }
    }
}
