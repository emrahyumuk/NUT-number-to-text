namespace Nut
{
    /// <summary>How the fractional part of an amount is rendered.</summary>
    public enum SubUnitFormat
    {
        /// <summary>Spelled out, with the sub-unit name: "fifty cents".</summary>
        Words = 0,

        /// <summary>Digits, with the sub-unit name: "50 cents".</summary>
        Digits,

        /// <summary>
        /// The fraction-over-hundred form used on cheques: "and 50/100", with no sub-unit
        /// name. Zero is written out rather than dropped — "and 00/100" — because the point
        /// of the form is to leave no room to add digits afterwards.
        /// </summary>
        Fraction,
    }
}
