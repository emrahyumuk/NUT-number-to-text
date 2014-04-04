using System;
using System.Linq;
using System.Text;

namespace Nut.TextConverters
{
    public sealed partial class FrenchConverter : BaseConverter
    {

        private static readonly Lazy<FrenchConverter> Lazy = new Lazy<FrenchConverter>(() => new FrenchConverter());
        public static FrenchConverter Instance { get { return Lazy.Value; } }
        public FrenchConverter()
        {
            Initialize();
        }

        protected override long Append(long num, long scale, StringBuilder builder) 
        {
            if (num > scale - 1) 
            {
                var baseScale = num / scale;

                if (scale != 1000 || baseScale != 1) 
                {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                builder.AppendFormat("{0} ", Scales[scale]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder) {
            if (num > 20) 
            {

                if (num == 80) 
                {
                    builder.AppendFormat("{0}", AdditionalStrings[num]);
                    return 0;
                }

                var tens = ((int)(num / 10)) * 10;

                if (tens == 70 || tens == 90) 
                {
                    tens = tens - 10;

                    if (num - tens == 11) 
                    {
                        builder.AppendFormat("{0} ", TextStrings[tens]);
                        builder.AppendFormat("{0} ", AdditionalStrings[num-tens]);
                        return 0;
                    }

                    builder.AppendFormat("{0}-", TextStrings[tens]);

                }
                else 
                {
                    builder.AppendFormat("{0} ", TextStrings[tens]);

                    var etUnList = new long[] { 21, 31, 41, 51, 61 };
                    if (etUnList.Contains(num)) {
                        builder.AppendFormat("{0} ", AdditionalStrings[num - tens]);
                        return 0;
                    }
                }
                
                num = num - tens;
            }
            return num;
        }
        private void Initialize()
        {
            TextStrings.Add(0, "zéro");
            TextStrings.Add(1, "un");
            TextStrings.Add(2, "deux");
            TextStrings.Add(3, "trois");
            TextStrings.Add(4, "quatre");
            TextStrings.Add(5, "cinq");
            TextStrings.Add(6, "six");
            TextStrings.Add(7, "sept");
            TextStrings.Add(8, "huit");
            TextStrings.Add(9, "neuf");
            TextStrings.Add(10, "dix");
            TextStrings.Add(11, "onze");
            TextStrings.Add(12, "douze");
            TextStrings.Add(13, "treize");
            TextStrings.Add(14, "quatorze");
            TextStrings.Add(15, "quinze");
            TextStrings.Add(16, "seize");
            TextStrings.Add(17, "dix-sept");
            TextStrings.Add(18, "dix-huit");
            TextStrings.Add(19, "dix-neuf");
            TextStrings.Add(20, "vingt");
            TextStrings.Add(30, "trente");
            TextStrings.Add(40, "quarante");
            TextStrings.Add(50, "cinquante ");
            TextStrings.Add(60, "soixante");
            TextStrings.Add(80, "quatre-vingt");
            TextStrings.Add(100, "cent");

            AdditionalStrings.Add(1, "et un");
            AdditionalStrings.Add(11, "et onze");
            AdditionalStrings.Add(80, "quatre-vingts");

            Scales.Add(1000000000, "milliard");
            Scales.Add(1000000, "million");
            Scales.Add(1000, "mille");
        }
    }
}