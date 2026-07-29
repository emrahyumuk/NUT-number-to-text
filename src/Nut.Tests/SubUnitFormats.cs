namespace Nut.Tests
{
    /// <summary>
    /// How the fractional part is written. The default spells it out, which is what the
    /// language calls for. Cheques commonly use a fraction over a hundred instead — Chase
    /// and Capital One both document "and 50/100" — so that is available as an opt-in.
    /// It is a document convention rather than a rule of the language, which is why it is
    /// not the default.
    /// </summary>
    [TestFixture]
    public class SubUnitFormats
    {
        private static Options Fraction => new Options { SubUnitFormat = SubUnitFormat.Fraction };

        [TestCase(2575.50, "two thousand five hundred seventy-five dollars and 50/100")]
        [TestCase(105.05, "one hundred five dollars and 05/100")]
        [TestCase(1.99, "one dollar and 99/100")]
        public void FractionForm(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.English, Fraction), Is.EqualTo(expected));
        }

        /// <summary>Zero is written out rather than dropped: the form exists so nothing can
        /// be appended to the amount afterwards.</summary>
        [Test]
        public void ZeroIsStillWritten()
        {
            Assert.That(2575m.ToText(Currency.USD, Language.English, Fraction),
                Is.EqualTo("two thousand five hundred seventy-five dollars and 00/100"));
        }

        [Test]
        public void DefaultIsUnchanged()
        {
            Assert.That(2575.50m.ToText(Currency.USD, Language.English),
                Is.EqualTo("two thousand five hundred seventy-five dollars fifty cents"));
        }

        /// <summary>The enum's Digits and the older bool mean the same thing.</summary>
        [Test]
        public void DigitsMatchesTheOlderFlag()
        {
            var viaEnum = 2575.50m.ToText(Currency.USD, Language.English,
                new Options { SubUnitFormat = SubUnitFormat.Digits });
            var viaBool = 2575.50m.ToText(Currency.USD, Language.English,
                new Options { SubUnitNotConvertedToText = true });

            Assert.That(viaEnum, Is.EqualTo("two thousand five hundred seventy-five dollars 50 cents"));
            Assert.That(viaBool, Is.EqualTo(viaEnum));
        }

        /// <summary>Languages that already join the two parts with a word keep that word
        /// rather than borrowing the English "and".</summary>
        [TestCase(Language.Spanish, Currency.EUR, "ciento cinco euros con 50/100")]
        [TestCase(Language.Portuguese, Currency.BRL, "cento e cinco reais e 50/100")]
        [TestCase(Language.Bulgarian, Currency.BGN, "сто и пет лева и 50/100")]
        [TestCase(Language.Latvian, Currency.EUR, "simts pieci eiro un 50/100")]
        public void SeparatorFollowsTheLanguage(string lang, string currency, string expected)
        {
            Assert.That(105.50m.ToText(currency, lang, Fraction), Is.EqualTo(expected));
        }

        [Test]
        public void WorksWithTheOtherOptions()
        {
            var options = new Options { SubUnitFormat = SubUnitFormat.Fraction, MainUnitFirstCharUpper = true };
            Assert.That((-2575.50m).ToText(Currency.USD, Language.English, options),
                Is.EqualTo("Minus two thousand five hundred seventy-five dollars and 50/100"));
        }
    }
}
