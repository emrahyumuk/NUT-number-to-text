namespace Nut.Tests
{
    /// <summary>
    /// German writes a number below a million as one closed-up word and only separates the
    /// parts from a million upwards (Duden). The converter emitted "ein hundert" and
    /// "zwei tausend" with spaces, while inconsistently closing up "eintausend".
    ///
    /// It also used "ein" everywhere; standing alone the numeral is "eins".
    /// </summary>
    [TestFixture]
    public class GermanSpelling
    {
        [TestCase(1, "eins")]
        [TestCase(21, "einundzwanzig")]
        [TestCase(100, "einhundert")]
        [TestCase(101, "einhunderteins")]
        [TestCase(120, "einhundertzwanzig")]
        [TestCase(121, "einhunderteinundzwanzig")]
        [TestCase(200, "zweihundert")]
        [TestCase(999, "neunhundertneunundneunzig")]
        [TestCase(1000, "eintausend")]
        [TestCase(1001, "eintausendeins")]
        [TestCase(2000, "zweitausend")]
        [TestCase(21000, "einundzwanzigtausend")]
        [TestCase(41000, "einundvierzigtausend")]
        [TestCase(100000, "einhunderttausend")]
        [TestCase(120000, "einhundertzwanzigtausend")]
        public void BelowAMillionIsOneWord(long number, string expected)
        {
            Assert.That(number.ToText(Language.German), Is.EqualTo(expected));
        }

        [TestCase(1000000, "eine Million")]
        [TestCase(2000000, "zwei Millionen")]
        [TestCase(1000000000, "eine Milliarde")]
        public void FromAMillionUpwardsThePartsSeparate(long number, string expected)
        {
            Assert.That(number.ToText(Language.German), Is.EqualTo(expected));
        }

        /// <summary>The example Duden gives for the rule.</summary>
        [Test]
        public void DudensOwnExample()
        {
            Assert.That(2120419L.ToText(Language.German),
                Is.EqualTo("zwei Millionen einhundertzwanzigtausendvierhundertneunzehn"));
        }

        [Test]
        public void LargeNumberReadsEndToEnd()
        {
            Assert.That(999999999L.ToText(Language.German), Is.EqualTo(
                "neunhundertneunundneunzig Millionen neunhundertneunundneunzigtausendneunhundertneunundneunzig"));
        }

        /// <summary>A currency name is a noun, and German takes "ein" before a noun.</summary>
        [TestCase(1, "ein Euro null Cent")]
        [TestCase(2, "zwei Euro null Cent")]
        [TestCase(41000, "einundvierzigtausend Euro null Cent")]
        [TestCase(1000000, "eine Million Euro null Cent")]
        public void AmountsUseEinBeforeTheCurrency(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.German), Is.EqualTo(expected));
        }

        [Test]
        public void SubUnitTakesEinToo()
        {
            Assert.That(1.01m.ToText(Currency.EUR, Language.German),
                Is.EqualTo("ein Euro ein Cent"));
        }

        /// <summary>The sign is handled by BaseConverter; German had its own dead copy of
        /// that logic inside Append, which never ran because negatives are stripped first.</summary>
        [Test]
        public void NegativesStillWork()
        {
            Assert.That((-41L).ToText(Language.German), Is.EqualTo("minus einundvierzig"));
        }
    }
}
