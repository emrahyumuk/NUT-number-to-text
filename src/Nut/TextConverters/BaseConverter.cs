using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nut.TextConverters {
    public abstract partial class BaseConverter
    {

        protected Dictionary<long, string> TextStrings;
        protected Dictionary<long, string> AdditionalStrings;
        protected Dictionary<long, string> Scales;

        protected BaseConverter() 
        {
            TextStrings = new Dictionary<long, string>();
            AdditionalStrings = new Dictionary<long, string>();
            Scales = new Dictionary<long, string>();
        }

        public virtual string ToText(long num) 
        {
            NumberLimitControl(num);

            var builder = new StringBuilder();

            if (num == 0)
            {
                builder.Append(TextStrings[num]);
                return builder.ToString();
            }

            num = Scales.Aggregate(num, (current, scale) => Append(current, scale.Key, builder));
            AppendLessThanOneThousand(num, builder);

            return builder.ToString().Trim();
        }

        protected virtual long Append(long num, long scale, StringBuilder builder)
        {
            if (num > scale - 1)
            {
                var baseScale = num / scale;
                AppendLessThanOneThousand(baseScale, builder);
                builder.AppendFormat("{0} ", Scales[scale]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        protected virtual void AppendLessThanOneThousand(long num, StringBuilder builder)
        {
            num = AppendHundreds(num, builder);
            num = AppendTens(num, builder);
            AppendUnits(num, builder);
        }

        protected virtual void AppendUnits(long num, StringBuilder builder)
        {
            if (num > 0)
            {
                builder.AppendFormat("{0} ", TextStrings[num]);
            }
        }

        protected virtual void AppendUnitsForAdditional(long num, StringBuilder builder)
        {
            if (num > 0)
            {
                builder.AppendFormat("{0} ", AdditionalStrings[num]);
            }
        }

        protected virtual long AppendTens(long num, StringBuilder builder)
        {
            if (num > 20)
            {
                var tens = ((int)(num / 10)) * 10;
                builder.AppendFormat("{0} ", TextStrings[tens]);
                num = num - tens;
            }
            return num;
        }

        protected virtual long AppendHundreds(long num, StringBuilder builder)
        {
            if (num > 99)
            {
                var hundreds = num / 100;
                builder.AppendFormat("{0} {1} ", TextStrings[hundreds], TextStrings[100]);
                num = num - (hundreds * 100);
            }
            return num;
        }

        private static void NumberLimitControl(long num) {
            if (num >= Constants.Parameters.NumberLimit) {
                throw new Exception(string.Format("{0} and larger than {0} numbers are not supported", Constants.Parameters.NumberLimit));
            }
        }
    }
}
