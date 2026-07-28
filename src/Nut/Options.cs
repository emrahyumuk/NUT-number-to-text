namespace Nut
{
    public struct Options
    {
        public bool MainUnitNotConvertedToText { get; set; }
        public bool SubUnitNotConvertedToText { get; set; }
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
