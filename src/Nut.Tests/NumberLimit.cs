using System;
using System.Globalization;

namespace Nut.Tests
{
    /// <summary>
    /// The library supports numbers below one trillion in magnitude. Going past that used
    /// to throw a bare <see cref="Exception"/>, which a caller cannot catch selectively
    /// without swallowing everything else.
    /// </summary>
    [TestFixture]
    public class NumberLimit
    {
        [TestCase(1000000000000L)]
        [TestCase(-1000000000000L)]
        [TestCase(long.MaxValue)]
        [TestCase(long.MinValue)]
        public void PastTheLimitThrowsArgumentOutOfRange(long number)
        {
            Assert.That(() => number.ToText(Language.English),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(999999999999L)]
        [TestCase(-999999999999L)]
        public void TheLimitItselfIsInclusive(long number)
        {
            Assert.That(() => number.ToText(Language.English), Throws.Nothing);
        }

        /// <summary>
        /// The money overload is the primary API, and it used to reach the check only after
        /// Convert.ToInt64 had already raised OverflowException — so a caller who narrowed
        /// their catch the way the library tells them to still crashed above 2^63.
        /// </summary>
        [TestCase("1000000000000")]
        [TestCase("-1000000000000")]
        [TestCase("9300000000000000000")]
        [TestCase("79228162514264337593543950335")] // decimal.MaxValue
        public void PastTheLimitThrowsArgumentOutOfRangeOnMoneyToo(string number)
        {
            var amount = decimal.Parse(number, CultureInfo.InvariantCulture);

            Assert.That(() => amount.ToText(Currency.USD, Language.English),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        /// <summary>Leaving the whole part as digits skipped the conversion, and with it the check.</summary>
        [Test]
        public void TheLimitHoldsWithTheWholePartLeftAsDigits()
        {
            Assert.That(() => 1000000000000m.ToText(Currency.USD, Language.English,
                    new Options { MainUnitNotConvertedToText = true }),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        /// <summary>
        /// Rounding can carry an in-range amount over the limit, and the answer must not
        /// depend on which output format is asked for. Checking only on the way in let
        /// 999999999999.995 render as "1000000000000 dollars" with the whole part left as
        /// digits, while the same input threw on the default path.
        /// </summary>
        [Test]
        public void RoundingCannotCarryPastTheLimit()
        {
            const decimal justUnder = 999999999999.995m;

            Assert.Multiple(() =>
            {
                Assert.That(() => justUnder.ToText(Currency.USD, Language.English),
                    Throws.InstanceOf<ArgumentOutOfRangeException>());
                Assert.That(() => justUnder.ToText(Currency.USD, Language.English,
                        new Options { MainUnitNotConvertedToText = true }),
                    Throws.InstanceOf<ArgumentOutOfRangeException>());
                Assert.That(() => justUnder.ToText(Currency.USD, Language.English,
                        new Options { SubUnitFormat = SubUnitFormat.Fraction }),
                    Throws.InstanceOf<ArgumentOutOfRangeException>());

                // Truncating drops the digits instead of carrying them, so it stays in range.
                Assert.That(() => justUnder.ToText(Currency.USD, Language.English,
                        new Options { SubUnitTruncated = true }),
                    Throws.Nothing);
                Assert.That(() => 999999999999.994m.ToText(Currency.USD, Language.English),
                    Throws.Nothing);
            });
        }

        /// <summary>The message should say what the range is, not just that something failed.</summary>
        [Test]
        public void TheMessageStatesTheRange()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => 1000000000000L.ToText(Language.English));
            Assert.That(ex.Message, Does.Contain("999999999999"));
        }
    }
}
