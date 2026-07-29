namespace Nut.Tests
{
    /// <summary>
    /// French agreement and hyphenation, per the Banque de dépannage linguistique and the
    /// Académie's 1990 rectifications. Three rules were missing: compounds below a hundred
    /// were spaced rather than hyphenated, multiplied "cent" never took its -s, and
    /// "million"/"milliard" — which are nouns — were never pluralised.
    /// </summary>
    [TestFixture]
    public class FrenchNumerals
    {
        [TestCase(22, "vingt-deux")]
        [TestCase(42, "quarante-deux")]
        [TestCase(52, "cinquante-deux")]
        [TestCase(81, "quatre-vingt-un")]
        [TestCase(99, "quatre-vingt-dix-neuf")]
        public void CompoundsBelowAHundredAreHyphenated(long number, string expected)
        {
            Assert.That(number.ToText(Language.French), Is.EqualTo(expected));
        }

        /// <summary>"et" replaces the hyphen at 21, 31, 41, 51, 61 and 71.</summary>
        [TestCase(21, "vingt et un")]
        [TestCase(31, "trente et un")]
        [TestCase(71, "soixante et onze")]
        public void TheEtFormsKeepTheirSpaces(long number, string expected)
        {
            Assert.That(number.ToText(Language.French), Is.EqualTo(expected));
        }

        /// <summary>Multiplied "cent" takes -s only when nothing follows it.</summary>
        [TestCase(100, "cent")] // not multiplied
        [TestCase(200, "deux cents")]
        [TestCase(300, "trois cents")]
        [TestCase(201, "deux cent un")] // a numeral follows
        [TestCase(200000, "deux cent mille")] // a scale word follows
        [TestCase(201000, "deux cent un mille")]
        public void CentAgreesOnlyWhenItEndsTheNumber(long number, string expected)
        {
            Assert.That(number.ToText(Language.French), Is.EqualTo(expected));
        }

        /// <summary>Same rule for "quatre-vingt".</summary>
        [TestCase(80, "quatre-vingts")]
        [TestCase(80000, "quatre-vingt mille")]
        public void QuatreVingtAgreesOnlyWhenItEndsTheNumber(long number, string expected)
        {
            Assert.That(number.ToText(Language.French), Is.EqualTo(expected));
        }

        /// <summary>million and milliard are nouns; mille is invariable.</summary>
        [TestCase(1000000, "un million")]
        [TestCase(2000000, "deux millions")]
        [TestCase(1000000000, "un milliard")]
        [TestCase(2000000000, "deux milliards")]
        [TestCase(1000, "mille")]
        [TestCase(2000, "deux mille")]
        public void ScaleNounsTakePlural(long number, string expected)
        {
            Assert.That(number.ToText(Language.French), Is.EqualTo(expected));
        }

        [Test]
        public void LargeNumberReadsEndToEnd()
        {
            Assert.That(999999999L.ToText(Language.French), Is.EqualTo(
                "neuf cent quatre-vingt-dix-neuf millions neuf cent quatre-vingt-dix-neuf mille neuf cent quatre-vingt-dix-neuf"));
        }
    }
}
