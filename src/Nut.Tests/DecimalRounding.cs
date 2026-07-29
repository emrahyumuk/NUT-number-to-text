namespace Nut.Tests
{
    /// <summary>
    /// The fraction was padded to two digits but never trimmed, so a third decimal was read
    /// as whole sub-units: 123.456 USD produced "four hundred fifty six cents". And because
    /// decimal preserves trailing zeros, 1.100 and 1.10 — the same amount — disagreed.
    ///
    /// Amounts are now rounded to the sub-unit first, away from zero, which is the
    /// convention for money.
    /// </summary>
    [TestFixture]
    public class DecimalRounding
    {
        [TestCase(1.5, "one dollar fifty cents")]
        [TestCase(1.05, "one dollar five cents")]
        [TestCase(1.994, "one dollar ninety-nine cents")]
        [TestCase(123.456, "one hundred twenty-three dollars forty-six cents")]
        [TestCase(2.345, "two dollars thirty-five cents")] // away from zero, not to even
        [TestCase(1.005, "one dollar one cent")]
        [TestCase(0.001, "zero dollar zero cent")] // too small to register
        public void ExtraDecimalsAreRounded(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
        }

        /// <summary>Rounding has to carry into the main unit, not just the fraction.</summary>
        [TestCase(1.999, "two dollars zero cent")]
        [TestCase(1.995, "two dollars zero cent")]
        [TestCase(0.999, "one dollar zero cent")]
        public void RoundingCarriesIntoTheMainUnit(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
        }

        /// <summary>1.10 and 1.100 are the same amount and must read the same. They did not:
        /// decimal keeps the scale it was written with, and the old code used it directly.</summary>
        [Test]
        public void TrailingZerosDoNotChangeTheReading()
        {
            var expected = "one dollar ten cents";
            Assert.That(1.1m.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
            Assert.That(1.10m.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
            Assert.That(1.100m.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
        }

        [Test]
        public void RoundingAppliesUnderTheSignToo()
        {
            Assert.That((-1.999m).ToText(Currency.USD, Language.English),
                Is.EqualTo("minus two dollars zero cent"));
        }

        /// <summary>An amount that rounds to nothing is not negative.</summary>
        [Test]
        public void MinusZeroIsJustZero()
        {
            Assert.That((-0.001m).ToText(Currency.USD, Language.English),
                Is.EqualTo("zero dollar zero cent"));
        }

        /// <summary>Callers who need the extra digits dropped rather than carried can ask
        /// for that; rounding stays the default.</summary>
        [TestCase(1.999, "one dollar ninety-nine cents")]
        [TestCase(123.456, "one hundred twenty-three dollars forty-five cents")]
        [TestCase(1.005, "one dollar zero cent")]
        [TestCase(0.999, "zero dollar ninety-nine cents")]
        [TestCase(1.5, "one dollar fifty cents")] // unaffected either way
        public void SubUnitTruncatedCutsInsteadOfRounding(decimal amount, string expected)
        {
            var options = new Options { SubUnitTruncated = true };
            Assert.That(amount.ToText(Currency.USD, Language.English, options), Is.EqualTo(expected));
        }

        [Test]
        public void TruncatingStillDropsTheSignWhenNothingIsLeft()
        {
            var options = new Options { SubUnitTruncated = true };
            Assert.That((-0.001m).ToText(Currency.USD, Language.English, options),
                Is.EqualTo("zero dollar zero cent"));
        }
    }
}
