using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nut.Models;

namespace Nut.TextConverters {
    internal sealed partial class TurkishConverter {

        public override CurrencyModel GetCurrencyModel(Currency currency) {
            switch (currency) {
                case Currency.eur:
                    return new CurrencyModel {
                        Currency = Currency.eur,
                        Names = new[] { "avro", "avro" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "avro sent", "avro sent" } }
                    };
                case Currency.usd:
                    return new CurrencyModel {
                        Currency = Currency.usd,
                        Names = new[] { "dolar", "dolar" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "sent", "sent" } }
                    };
                case Currency.rub:
                    return new CurrencyModel {
                        Currency = Currency.rub,
                        Names = new[] { "ruble", "ruble" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kopek", "kopek" } }
                    };
                case Currency.tl:
                    return new CurrencyModel {
                        Currency = Currency.tl,
                        Names = new[] { "türk lirası", "türk lirası" },
                        ChildCurrency = new BaseCurrencyModel { Names = new[] { "kuruş", "kuruş" } }
                    };
            }
            return null;
        }

    }
}
