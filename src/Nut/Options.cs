namespace Nut
{
    public struct Options
    {
        public bool MainUnitNotConvertedToText { get; set; }
        /// <summary>
        /// Equivalent to <see cref="SubUnitFormat.Digits"/>. Kept because it predates
        /// <see cref="SubUnitFormat"/>; setting either has the same effect.
        /// </summary>
        public bool SubUnitNotConvertedToText { get; set; }

        private SubUnitFormat? _subUnitFormat;

        /// <summary>
        /// How the fractional part is written. Defaults to <see cref="SubUnitFormat.Words"/>.
        /// <see cref="SubUnitFormat.Fraction"/> gives the cheque form, "and 50/100".
        /// <para>
        /// Backed by a nullable field so that "left alone" and "deliberately set to Words"
        /// are different states. Words is the enum's zero value, so without this the older
        /// bool silently overruled a caller who asked for words in as many words.
        /// </para>
        /// </summary>
        public SubUnitFormat SubUnitFormat
        {
            get { return _subUnitFormat ?? (SubUnitNotConvertedToText ? SubUnitFormat.Digits : SubUnitFormat.Words); }
            set { _subUnitFormat = value; }
        }
        public bool SubUnitZeroNotDisplayed { get; set; }
        public bool MainUnitFirstCharUpper { get; set; }
        public bool SubUnitFirstCharUpper { get; set; }
        public bool CurrencyFirstCharUpper { get; set; }

        /// <summary>
        /// Cut extra decimals off instead of rounding them. An amount carrying more digits
        /// than the currency has, such as 1.999, reads as "one dollar ninety nine cents"
        /// rather than "two dollars zero cent".
        /// <para>
        /// Default is to round half away from zero, the usual convention for money.
        /// </para>
        /// </summary>
        public bool SubUnitTruncated { get; set; }
    }
}
