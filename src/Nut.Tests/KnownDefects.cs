using System;

namespace Nut.Tests
{
    /// <summary>
    /// Defects that are reproducible today. Each test asserts the WRONG behaviour on
    /// purpose, so the suite stays green until someone fixes the defect — at which point
    /// the test fails and has to be rewritten as the correct expectation. Every case here
    /// was reproduced against the library before being written down.
    /// </summary>
    [TestFixture]
    public class KnownDefects
    {
        /// <summary>
        /// Extentions.cs lowercases the language argument on the int overloads, but the
        /// Culture constants are mixed case ("en-US"), so no case ever matches and the
        /// caller silently gets "". The long and decimal overloads do not lowercase and
        /// work fine, so the same culture string succeeds or fails depending on the
        /// numeric type it is called on.
        /// </summary>
        [TestCase(Culture.EnglishUS)]
        [TestCase(Culture.EnglishGB)]
        [TestCase(Culture.French)]
        [TestCase(Culture.Russian)]
        [TestCase(Culture.Spanish)]
        [TestCase(Culture.Turkish)]
        [TestCase(Culture.Ukrainian)]
        [TestCase(Culture.Bulgarian)]
        [TestCase(Culture.EthiopianAM)]
        [TestCase(Culture.Polish)]
        [TestCase(Culture.Belarusian)]
        [TestCase(Culture.PortugueseBR)]
        [TestCase(Culture.GermanDE)]
        public void CultureCodesReturnEmptyOnTheIntOverload(string culture)
        {
            Assert.That(101.ToText(culture), Is.Empty);
        }

        [Test]
        public void SameCultureWorksOnTheLongOverload()
        {
            Assert.That(101L.ToText(Culture.EnglishUS), Is.EqualTo("one hundred one"));
        }

        [Test]
        public void UppercaseCurrencyCodeReturnsEmpty()
        {
            Assert.That(41m.ToText("USD", Language.English), Is.Empty);
            Assert.That(41m.ToText("usd", Language.English), Is.Not.Empty);
        }

        /// <summary>Every Append* helper guards with "num > x", so a negative number matches
        /// nothing and falls through to an empty string. On the money overload the integer
        /// part vanishes while the fraction survives, producing a plausible-looking but
        /// completely wrong amount.</summary>
        [Test]
        public void NegativeNumbersProduceEmptyOrWrongText()
        {
            Assert.That((-5L).ToText(Language.English), Is.Empty);
            Assert.That((-41.5m).ToText(Currency.USD, Language.English),
                Is.EqualTo("dollar fifty cents")); // the 41 and the minus are both gone
        }

        /// <summary>BaseConverter pads the fraction to two digits but never trims it, so a
        /// third decimal is read as if it were part of the sub-unit count.</summary>
        [Test]
        public void MoreThanTwoDecimalsAreReadAsWholeSubUnits()
        {
            Assert.That(123.456m.ToText(Currency.USD, Language.English),
                Is.EqualTo("one hundred twenty three dollars four hundred fifty six cents"));
        }

        /// <summary>Callers cannot catch this selectively.</summary>
        [Test]
        public void OverTheLimitThrowsBareException()
        {
            var ex = Assert.Throws<Exception>(() => 1000000000000L.ToText(Language.English));
            Assert.That(ex.GetType(), Is.EqualTo(typeof(Exception)));
        }
    }
}
