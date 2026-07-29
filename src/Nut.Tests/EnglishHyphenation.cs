namespace Nut.Tests
{
    /// <summary>
    /// Compound numbers from twenty-one through ninety-nine are hyphenated
    /// (Merriam-Webster). Only that pair takes a hyphen — the rest of the number stays
    /// spaced, so 121 is "one hundred twenty-one", not "one-hundred-twenty-one".
    /// </summary>
    [TestFixture]
    public class EnglishHyphenation
    {
        [TestCase(21, "twenty-one")]
        [TestCase(42, "forty-two")]
        [TestCase(99, "ninety-nine")]
        [TestCase(121, "one hundred twenty-one")]
        [TestCase(199, "one hundred ninety-nine")]
        [TestCase(999, "nine hundred ninety-nine")]
        [TestCase(1021, "one thousand twenty-one")]
        [TestCase(41000, "forty-one thousand")]
        [TestCase(1000021, "one million twenty-one")]
        public void CompoundsAreHyphenated(long number, string expected)
        {
            Assert.That(number.ToText(Language.English), Is.EqualTo(expected));
        }

        /// <summary>A round ten has nothing to join, and hundreds keep their space.</summary>
        [TestCase(20, "twenty")]
        [TestCase(30, "thirty")]
        [TestCase(100, "one hundred")]
        [TestCase(101, "one hundred one")]
        [TestCase(110, "one hundred ten")]
        [TestCase(120, "one hundred twenty")]
        public void NothingElseIsHyphenated(long number, string expected)
        {
            Assert.That(number.ToText(Language.English), Is.EqualTo(expected));
        }

        [TestCase(21, "twenty-one dollars zero cent")]
        [TestCase(21.42, "twenty-one dollars forty-two cents")]
        [TestCase(121.99, "one hundred twenty-one dollars ninety-nine cents")]
        public void AmountsAreHyphenatedToo(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.English), Is.EqualTo(expected));
        }

        [Test]
        public void LargeNumberReadsEndToEnd()
        {
            Assert.That(999999999L.ToText(Language.English), Is.EqualTo(
                "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine"));
        }
    }
}
