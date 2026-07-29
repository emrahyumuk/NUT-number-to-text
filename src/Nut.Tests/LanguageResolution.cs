using System;
namespace Nut.Tests
{
    /// <summary>
    /// Which language string resolves to which converter. The int overloads used to
    /// lowercase the argument before comparing it against the mixed-case Culture
    /// constants, so all thirteen culture codes silently produced "" there while working
    /// on the long and decimal overloads — the same string succeeded or failed depending
    /// on the numeric type it was called on.
    /// </summary>
    [TestFixture]
    public class LanguageResolution
    {
        [TestCase(Culture.EnglishUS, "one hundred one")]
        [TestCase(Culture.EnglishGB, "one hundred and one")] // British "and"
        [TestCase(Culture.French, "cent un")]
        [TestCase(Culture.GermanDE, "einhunderteins")]
        [TestCase(Culture.Spanish, "ciento uno")]
        [TestCase(Culture.PortugueseBR, "cento e um")]
        [TestCase(Culture.Turkish, "yüz bir")]
        [TestCase(Culture.Russian, "сто один")]
        [TestCase(Culture.Ukrainian, "Сто Один")]
        [TestCase(Culture.Belarusian, "сто адзін")]
        [TestCase(Culture.Bulgarian, "сто и един")]
        [TestCase(Culture.Polish, "Sto Jeden")]
        [TestCase(Culture.EthiopianAM, "አንድ መቶ አንድ")]
        public void CultureCodesWorkOnTheIntOverload(string culture, string expected)
        {
            Assert.That(101.ToText(culture), Is.EqualTo(expected));
        }

        /// <summary>The same string must give the same answer whichever overload it reaches.</summary>
        [TestCase(Culture.EnglishUS)]
        [TestCase(Culture.Turkish)]
        [TestCase(Culture.Russian)]
        [TestCase(Language.English)]
        [TestCase(Language.Turkish)]
        public void IntAndLongOverloadsAgree(string lang)
        {
            Assert.That(101.ToText(lang), Is.EqualTo(101L.ToText(lang)));
            Assert.That(101.ToText(lang), Is.Not.Empty);
        }

        [TestCase("EN")]
        [TestCase("En")]
        [TestCase("en-us")]
        [TestCase("EN-US")]
        [TestCase("en-US")]
        public void LanguageMatchingIgnoresCase(string lang)
        {
            Assert.That(101.ToText(lang), Is.EqualTo("one hundred one"));
        }

        [TestCase("USD")]
        [TestCase("usd")]
        [TestCase("Usd")]
        public void CurrencyMatchingIgnoresCase(string currency)
        {
            Assert.That(41m.ToText(currency, Language.English),
                Is.EqualTo("forty-one dollars zero cent"));
        }

        /// <summary>Aliases resolve regardless of case too.</summary>
        [TestCase("TL", "forty-one turkish lira zero kurus")]
        [TestCase("RUR", "forty-one rubles zero kopek")]
        public void CurrencyAliasesIgnoreCase(string currency, string expected)
        {
            Assert.That(41m.ToText(currency, Language.English), Is.EqualTo(expected));
        }

        /// <summary>An unsupported language fails loudly rather than returning "".</summary>
        [TestCase("xx")]
        [TestCase("")]
        [TestCase("zz-ZZ")]
        [TestCase(null)]
        public void UnknownLanguageThrows(string lang)
        {
            Assert.That(() => 101.ToText(lang), Throws.InstanceOf<NotSupportedException>());
            Assert.That(() => 41m.ToText(Currency.USD, lang), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void UnknownCurrencyThrows()
        {
            Assert.That(() => 41m.ToText("zzz", Language.English), Throws.InstanceOf<NotSupportedException>());
            Assert.That(() => 41m.ToText(null, Language.English), Throws.InstanceOf<NotSupportedException>());
        }

        /// <summary>The message has to say what went wrong and what would work.</summary>
        [Test]
        public void TheMessageNamesTheProblemAndTheAlternatives()
        {
            var lang = Assert.Throws<NotSupportedException>(() => 101.ToText("xx"));
            Assert.That(lang.Message, Does.Contain("xx").And.Contain("en").And.Contain("tr"));

            var currency = Assert.Throws<NotSupportedException>(
                () => 1m.ToText(Currency.ARS, Language.Amharic));
            Assert.That(currency.Message, Does.Contain("ars").And.Contain("am-ET"));
        }

        [Test]
        public void SupportedLanguagesIsDiscoverable()
        {
            Assert.That(Extensions.SupportedLanguages, Contains.Item("en").And.Contains("en-GB").And.Contains("lv"));
        }
    }
}
