namespace Nut.Tests
{
    /// <summary>
    /// Uzbek in the Latin script. The number system builds from twenty-two basic words and
    /// behaves like Turkish: no "bir" before yuz or ming, and the teens are two words.
    /// </summary>
    [TestFixture]
    public class UzbekNumerals
    {
        [TestCase(0, "nol")]
        [TestCase(1, "bir")]
        [TestCase(10, "o'n")]
        [TestCase(11, "o'n bir")]
        [TestCase(15, "o'n besh")]
        [TestCase(20, "yigirma")]
        [TestCase(21, "yigirma bir")]
        [TestCase(42, "qirq ikki")]
        [TestCase(60, "oltmish")]
        [TestCase(70, "yetmish")]
        [TestCase(99, "to'qson to'qqiz")]
        public void BasicNumbers(long number, string expected)
        {
            Assert.That(number.ToText(Language.Uzbek), Is.EqualTo(expected));
        }

        /// <summary>Like Turkish, the leading "bir" is dropped before yuz and ming.</summary>
        [TestCase(100, "yuz")]
        [TestCase(101, "yuz bir")]
        [TestCase(200, "ikki yuz")]
        [TestCase(1000, "ming")]
        [TestCase(2000, "ikki ming")]
        [TestCase(41000, "qirq bir ming")]
        [TestCase(100000, "yuz ming")]
        public void NoLeadingOneBeforeYuzAndMing(long number, string expected)
        {
            Assert.That(number.ToText(Language.Uzbek), Is.EqualTo(expected));
        }

        /// <summary>But million and milliard do take it.</summary>
        [TestCase(1000000, "bir million")]
        [TestCase(2000000, "ikki million")]
        [TestCase(1000000000, "bir milliard")]
        public void MillionAndMilliardKeepIt(long number, string expected)
        {
            Assert.That(number.ToText(Language.Uzbek), Is.EqualTo(expected));
        }

        [TestCase(1, "bir so'm nol tiyin")]
        [TestCase(123.45, "yuz yigirma uch so'm qirq besh tiyin")]
        [TestCase(41000, "qirq bir ming so'm nol tiyin")]
        public void Som(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.UZS, Language.Uzbek), Is.EqualTo(expected));
        }

        [Test]
        public void GeneralBehaviourApplies()
        {
            Assert.That((-41L).ToText(Language.Uzbek), Is.EqualTo("minus qirq bir"));
            Assert.That(101.ToText(Culture.Uzbek), Is.EqualTo("yuz bir"));
            Assert.That(101.ToText("UZ"), Is.EqualTo("yuz bir"));
        }

        [Test]
        public void LargeNumberReadsEndToEnd()
        {
            Assert.That(999999999L.ToText(Language.Uzbek), Is.EqualTo(
                "to'qqiz yuz to'qson to'qqiz million to'qqiz yuz to'qson to'qqiz ming to'qqiz yuz to'qson to'qqiz"));
        }
    }
}
