using System;

namespace Nut.Tests
{
    /// <summary>
    /// Negative amounts used to return an empty string, and on the money overload something
    /// worse: the integer part disappeared while the fraction survived, so -41.50 USD read
    /// as "dollar fifty cents" — a plausible-looking amount that was simply wrong.
    ///
    /// The sign is handled once in BaseConverter, so the rest of the pipeline only ever
    /// sees a positive number. Reported as #22.
    /// </summary>
    [TestFixture]
    public class NegativeNumbers
    {
        [TestCase(Language.English, "minus forty one")]
        [TestCase(Language.French, "moins quarante et un")]
        [TestCase(Language.German, "minus einundvierzig")]
        [TestCase(Language.Spanish, "menos cuarenta y uno")]
        [TestCase(Language.Portuguese, "menos quarenta e um")]
        [TestCase(Language.Turkish, "eksi kırk bir")]
        [TestCase(Language.Russian, "минус сорок один")]
        [TestCase(Language.Ukrainian, "мінус Сорок Один")]
        [TestCase(Language.Belarusian, "мінус сорак адзін")]
        [TestCase(Language.Bulgarian, "минус четиридесет и един")]
        [TestCase(Language.Polish, "minus Czterdzieści Jeden")]
        [TestCase(Language.Amharic, "ሲቀነስ አርባ አንድ")]
        public void EveryLanguageHasItsOwnWord(string lang, string expected)
        {
            Assert.That((-41L).ToText(lang), Is.EqualTo(expected));
        }

        /// <summary>The integer part must survive, which is exactly what used to be lost.</summary>
        [Test]
        public void MoneyKeepsBothTheSignAndTheAmount()
        {
            Assert.That((-41.5m).ToText(Currency.USD, Language.English),
                Is.EqualTo("minus forty one dollars fifty cents"));
            Assert.That((-0.5m).ToText(Currency.USD, Language.English),
                Is.EqualTo("minus zero dollar fifty cents"));
        }

        /// <summary>Gender and declension still apply under the sign.</summary>
        [Test]
        public void GrammarStillAppliesToNegativeAmounts()
        {
            Assert.That((-41000m).ToText(Currency.RUB, Language.Russian),
                Is.EqualTo("минус сорок одна тысяча рублей ноль копеек"));
            Assert.That((-2.02m).ToText(Currency.BGN, Language.Bulgarian),
                Is.EqualTo("минус два лева и две стотинки"));
        }

        /// <summary>A negative amount is the mirror of its positive counterpart.</summary>
        [TestCase(1)]
        [TestCase(42)]
        [TestCase(1000)]
        [TestCase(999999999)]
        public void NegativeIsThePositiveWithASignInFront(long number)
        {
            Assert.That((-number).ToText(Language.English),
                Is.EqualTo("minus " + number.ToText(Language.English)));
        }

        [Test]
        public void ZeroIsNeverSigned()
        {
            Assert.That(0L.ToText(Language.English), Is.EqualTo("zero"));
            Assert.That((-0m).ToText(Currency.USD, Language.English),
                Is.EqualTo("zero dollar zero cent"));
        }

        /// <summary>The limit applies on both sides; negating long.MinValue would overflow.</summary>
        [Test]
        public void TheLimitAppliesToNegativesToo()
        {
            Assert.That(() => (-1000000000000L).ToText(Language.English), Throws.Exception);
            Assert.That(() => long.MinValue.ToText(Language.English), Throws.Exception);
            Assert.That(() => (-999999999999L).ToText(Language.English), Throws.Nothing);
        }
    }
}
