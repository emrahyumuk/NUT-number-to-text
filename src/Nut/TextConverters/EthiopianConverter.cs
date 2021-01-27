using Nut.Models;
using System;

namespace Nut.TextConverters
{
    public sealed class EthiopianConverter : BaseConverter
    {
        private static readonly Lazy<EthiopianConverter> Lazy = new Lazy<EthiopianConverter>(() => new EthiopianConverter());
        public static EthiopianConverter Instance => Lazy.Value;

        public override string CultureName => Culture.EthiopianAM;

        public EthiopianConverter()
        {
            Initialize();
        }

        private void Initialize()
        {
            NumberTexts.Add(0, new[] { "ዜሮ" });
            NumberTexts.Add(1, new[] { "አንድ" });
            NumberTexts.Add(2, new[] { "ሁለት"});
            NumberTexts.Add(3, new[] { "ሶስት" });
            NumberTexts.Add(4, new[] { "አራት" });
            NumberTexts.Add(5, new[] { "አምስት" });
            NumberTexts.Add(6, new[] { "ስድስት" });
            NumberTexts.Add(7, new[] { "ሰባት" });
            NumberTexts.Add(8, new[] { "ስምንት" });
            NumberTexts.Add(9, new[] { "ዘጠኝ" });
            NumberTexts.Add(10, new[] { "አስር" });
            NumberTexts.Add(11, new[] { "አስራ አንድ" });
            NumberTexts.Add(12, new[] { "አስራ ሁለት" });
            NumberTexts.Add(13, new[] { "አስራ ሶስት" });
            NumberTexts.Add(14, new[] { "አስራ አራት" });
            NumberTexts.Add(15, new[] { "አስራ አምስት" });
            NumberTexts.Add(16, new[] { "አስራ ስድስት" });
            NumberTexts.Add(17, new[] { "አስራ ሰባት" });
            NumberTexts.Add(18, new[] { "አስራ ስምንት" });
            NumberTexts.Add(19, new[] { "አስራ ዘጠኝ" });
            NumberTexts.Add(20, new[] { "ሀያ" });
            NumberTexts.Add(30, new[] { "ሰላሳ" });
            NumberTexts.Add(40, new[] { "አርባ" });
            NumberTexts.Add(50, new[] { "ሀምሳ" });
            NumberTexts.Add(60, new[] { "ስልሳ" });
            NumberTexts.Add(70, new[] { "ሰባ" });
            NumberTexts.Add(80, new[] { "ሰማንያ" });
            NumberTexts.Add(90, new[] { "ዘጠና" });
            NumberTexts.Add(100, new[] { "መቶ" });

            ScaleTexts.Add(1000000000, new[] { "ቢሊዮን" });
            ScaleTexts.Add(1000000, new[] { "ሚሊዮን" });
            ScaleTexts.Add(1000, new[] { "ሺህ" });
        }

        #region Currency

        protected override string GetUnitSeparator(CurrencyModel currency, bool addAndAsUnitSeparator)
        {
            return " ከ ";
        }

        protected override CurrencyModel GetCurrencyModel(string currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ዩሮ", "ዩሮ" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.USD:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ዶላር", "ዶላር" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.RUB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "የራሺያ ሩብል", "የራሺያ ሩብል" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.TRY:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "የቱርክ ሊራ", "የቱርክ ሊራ" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.BGN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "የቡልጋሪያ ሌቭ", "የቡልጋሪያ ሌቭ" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } },
                    };
                case Currency.UAH:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "የዩክሬን ሀሪይቭኒአ", "የዩክሬን ሀሪይቭኒአ" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.ETB:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "ብር", "ብር" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "ሳንቲም", "ሳንቲም" } }
                    };
                case Currency.PLN:
                    return new CurrencyModel
                    {
                        Currency = currency,
                        Names = new[] { "???", "???" },
                        SubUnitCurrency = new BaseCurrencyModel { Names = new[] { "???", "???" } }
                    };
            }
            return null;
        }

        #endregion Currency
    }
}