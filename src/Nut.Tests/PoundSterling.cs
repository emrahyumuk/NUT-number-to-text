namespace Nut.Tests
{
    /// <summary>
    /// GBP across the languages that name it. Requested in
    /// <see href="https://github.com/emrahyumuk/NUT-number-to-text/pull/24">#24</see>.
    ///
    /// The pound is feminine in French, Spanish and Portuguese — and in French the gender
    /// carries meaning, since "le livre" is a book while "la livre" is the currency. Those
    /// three converters had no gender handling at all before this.
    /// </summary>
    [TestFixture]
    public class PoundSterling
    {
        [TestCase(Language.English, "one pound sterling one penny")]
        [TestCase(Language.Turkish, "bir sterlin bir peni")]
        [TestCase(Language.German, "ein Pfund Sterling ein Penny")]
        [TestCase(Language.Russian, "один фунт стерлингов один пенни")]
        [TestCase(Language.Belarusian, "адзін фунт стэрлінгаў адзін пені")]
        public void MasculineOrUninflected(string lang, string expected)
        {
            Assert.That(1.01m.ToText(Currency.GBP, lang), Is.EqualTo(expected));
        }

        /// <summary>"une livre", not "un livre" — the latter is a book.</summary>
        [Test]
        public void FrenchPoundIsFeminine()
        {
            Assert.That(1m.ToText(Currency.GBP, Language.French),
                Is.EqualTo("une livre sterling zéro penny"));
            Assert.That(21m.ToText(Currency.GBP, Language.French),
                Is.EqualTo("vingt et une livres sterling zéro penny"));
            // and the masculine currencies are unaffected
            Assert.That(1m.ToText(Currency.EUR, Language.French),
                Is.EqualTo("un euro zéro centime"));
        }

        [Test]
        public void SpanishPoundIsFeminine()
        {
            Assert.That(1.01m.ToText(Currency.GBP, Language.Spanish),
                Is.EqualTo("una libra esterlina con un penique"));
            Assert.That(21m.ToText(Currency.GBP, Language.Spanish),
                Is.EqualTo("veintiuna libras esterlinas con cero penique"));
            Assert.That(1m.ToText(Currency.EUR, Language.Spanish),
                Is.EqualTo("un euro con cero céntimo de euro"));
        }

        [Test]
        public void PortuguesePoundIsFeminine()
        {
            Assert.That(1.01m.ToText(Currency.GBP, Language.Portuguese),
                Is.EqualTo("uma libra esterlina e um pêni"));
            Assert.That(2m.ToText(Currency.GBP, Language.Portuguese),
                Is.EqualTo("duas libras esterlinas e zero pêni"));
            Assert.That(2m.ToText(Currency.BRL, Language.Portuguese),
                Is.EqualTo("dois reais e zero centavo"));
        }

        /// <summary>French currency names that were wrong in ways gender work exposed.</summary>
        [TestCase(Currency.TRY, 1, "une livre turque zéro kuruş")]
        [TestCase(Currency.TRY, 2, "deux livres turques zéro kuruş")]
        [TestCase(Currency.UAH, 2, "deux hryvnias ukrainiennes zéro kopiyka")]
        [TestCase(Currency.RUB, 1, "un rouble zéro kopeck")]
        public void FrenchCurrencyNames(string currency, decimal amount, string expected)
        {
            Assert.That(amount.ToText(currency, Language.French), Is.EqualTo(expected));
        }
    }
}
