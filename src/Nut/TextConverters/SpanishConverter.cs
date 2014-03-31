using System;
using System.Text;

namespace Nut.TextConverters
{
    internal sealed class SpanishConverter : BaseConverter
    {

        private static readonly Lazy<SpanishConverter> Lazy = new Lazy<SpanishConverter>(() => new SpanishConverter());
        public static SpanishConverter Instance { get { return Lazy.Value; } }
        public SpanishConverter()
        {
            Initialize();
        }

        protected override long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;
                if (scale > 999 && baseScale == 1)
                {
                    AppendUnitsForAdditional(baseScale, builder);
                }
                else
                {
                    AppendLessThanOneThousand(baseScale, builder);
                }

                if (scale == 1000000 && num > 1)
                {
                    builder.AppendFormat("{0} ", AdditionalStrings[scale]);
                }
                else
                {
                    builder.AppendFormat("{0} ", Scales[scale]);
                }

                num = num - (baseScale * scale);
            }
            return num;
        }

        protected override long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20)
            {
                var tens = ((int)(num / 10)) * 10;

                builder.AppendFormat(num == tens ? "{0}" : "{0} y ", TextStrings[tens]);

                num = num - tens;
            }
            return num;
        }

        protected override long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100 * 100;
                builder.AppendFormat("{0} ", TextStrings[hundreds]);
                num = num - hundreds;
            }
            return num;
        }

        private void Initialize()
        {
            TextStrings.Add(0, "cero");
            TextStrings.Add(1, "uno");
            TextStrings.Add(2, "dos");
            TextStrings.Add(3, "tres");
            TextStrings.Add(4, "cuatro");
            TextStrings.Add(5, "cinco");
            TextStrings.Add(6, "seis");
            TextStrings.Add(7, "siete");
            TextStrings.Add(8, "ocho");
            TextStrings.Add(9, "nueve");
            TextStrings.Add(10, "diez");
            TextStrings.Add(11, "once");
            TextStrings.Add(12, "doce");
            TextStrings.Add(13, "trece");
            TextStrings.Add(14, "catorce");
            TextStrings.Add(15, "quince");
            TextStrings.Add(16, "dieciséis");
            TextStrings.Add(17, "diecisiete");
            TextStrings.Add(18, "dieciocho");
            TextStrings.Add(19, "diecinueve");
            TextStrings.Add(20, "veinte");
            TextStrings.Add(30, "treinta");
            TextStrings.Add(40, "cuarenta");
            TextStrings.Add(50, "cincuenta");
            TextStrings.Add(60, "sesenta");
            TextStrings.Add(70, "setenta");
            TextStrings.Add(80, "ochenta");
            TextStrings.Add(90, "noventa");
            TextStrings.Add(100, "cien");
            TextStrings.Add(200, "doscientos");
            TextStrings.Add(300, "trescientos");
            TextStrings.Add(400, "cuatrocientos");
            TextStrings.Add(500, "quinientos");
            TextStrings.Add(600, "seiscientos");
            TextStrings.Add(700, "setecientos");
            TextStrings.Add(800, "ochocientos");
            TextStrings.Add(900, "novecientos");

            AdditionalStrings.Add(1, "un");
            AdditionalStrings.Add(1000000, "milones");

            Scales.Add(1000000000, "mil millones");
            Scales.Add(1000000, "millón");
            Scales.Add(1000, "mil");
        }
    }
}
