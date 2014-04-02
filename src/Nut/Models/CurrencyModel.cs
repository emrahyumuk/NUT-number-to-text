using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nut.Models 
{
    public class CurrencyModel : BaseCurrencyModel 
    {
        public Currency Currency { get; set; }
        public BaseCurrencyModel ChildCurrency { get; set; }
    }
}
