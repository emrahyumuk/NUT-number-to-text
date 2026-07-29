namespace Nut.Tests
{
    /// <summary>
    /// Latvian, from <see href="https://github.com/emrahyumuk/NUT-number-to-text/pull/24">#24</see>.
    /// Numerals agree in gender with what they count, and the unit name takes three forms:
    /// singular after a count ending in one, genitive plural after zero, plural otherwise.
    /// </summary>
    [TestFixture]
    public class LatvianNumerals
    {
        [TestCase(0, "nulle")]
        [TestCase(1, "viens")]
        [TestCase(2, "divi")]
        [TestCase(11, "vienpadsmit")]
        [TestCase(21, "divdesmit viens")]
        [TestCase(100, "simts")]
        [TestCase(200, "divi simti")]
        [TestCase(1000, "viens tūkstotis")]
        [TestCase(2000, "divi tūkstoši")]
        [TestCase(5000, "pieci tūkstoši")]
        [TestCase(1234, "viens tūkstotis divi simti trīsdesmit četri")]
        [TestCase(1000000, "viens miljons")]
        public void Numbers(long number, string expected)
        {
            Assert.That(number.ToText(Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>Zero takes the genitive plural: "nulle centu", not "nulle centi".</summary>
        [TestCase(1, "viens eiro un nulle centu")]
        [TestCase(2, "divi eiro un nulle centu")]
        [TestCase(21, "divdesmit viens eiro un nulle centu")]
        [TestCase(1000, "viens tūkstotis eiro un nulle centu")]
        [TestCase(1234.56, "viens tūkstotis divi simti trīsdesmit četri eiro un piecdesmit seši centi")]
        public void Euro(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>mārciņa is feminine, so the numeral agrees.</summary>
        [TestCase(1, "viena sterliņu mārciņa un nulle pensu")]
        [TestCase(2, "divas sterliņu mārciņas un nulle pensu")]
        [TestCase(21, "divdesmit viena sterliņu mārciņa un nulle pensu")]
        public void PoundIsFeminine(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.GBP, Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>Singular after a count ending in one — including 21 — but not after
        /// eleven, where the -teen blocks it.</summary>
        [TestCase(1, "cents")]
        [TestCase(21, "cents")]
        [TestCase(11, "centi")]
        [TestCase(2, "centi")]
        public void SubUnitInflection(int cents, string expectedWord)
        {
            var amount = 1m + cents / 100m;
            Assert.That(amount.ToText(Currency.EUR, Language.Latvian), Does.EndWith(expectedWord));
        }

        [Test]
        public void GeneralBehaviourApplies()
        {
            Assert.That((-41L).ToText(Language.Latvian), Is.EqualTo("mīnus četrdesmit viens"));
            Assert.That(101.ToText(Culture.Latvian), Is.EqualTo("simts viens"));
        }
    }
}
