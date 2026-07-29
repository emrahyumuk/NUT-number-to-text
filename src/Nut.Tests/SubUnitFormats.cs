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

        /// <summary>
        /// Words is the enum's zero value, so an explicit request for it used to be
        /// indistinguishable from an unset option and the older bool quietly won — which
        /// is exactly what a caller migrating one call site off the bool would hit.
        /// </summary>
        [Test]
        public void AskingForWordsBeatsTheOlderFlag()
        {
            var text = 2575.50m.ToText(Currency.USD, Language.English,
                new Options { SubUnitNotConvertedToText = true, SubUnitFormat = SubUnitFormat.Words });

            Assert.That(text, Is.EqualTo("two thousand five hundred seventy-five dollars fifty cents"));
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

        /// <summary>
        /// The rest have no separator to borrow, and the form is not theirs to begin with.
        /// Falling back to the English "and" put it into nine of the fourteen — a Turkish
        /// cheque read "yüz beş türk lirası and 50/100" — so ask for a form a language does
        /// not have and you get told, rather than getting English in the amount field.
        /// </summary>
        [TestCase(Language.French, Currency.EUR)]
        [TestCase(Language.German, Currency.EUR)]
        [TestCase(Language.Turkish, Currency.TRY)]
        [TestCase(Language.Russian, Currency.RUB)]
        [TestCase(Language.Ukrainian, Currency.UAH)]
        [TestCase(Language.Belarusian, Currency.BYN)]
        [TestCase(Language.Polish, Currency.PLN)]
        [TestCase(Language.Uzbek, Currency.UZS)]
        public void LanguagesWithoutTheFormSaySo(string lang, string currency)
        {
            Assert.That(() => 105.50m.ToText(currency, lang, Fraction),
                Throws.InstanceOf<NotSupportedException>().With.Message.Contains("Fraction"));
        }

        /// <summary>
        /// Pinned as a set so that adding a language cannot quietly extend the form to it.
        /// A new converter has to say what joins the two parts before this form works, and
        /// until it does, asking for it fails.
        /// </summary>
        [Test]
        public void ExactlyTheseLanguagesHaveTheForm()
        {
            var supported = new List<string>();

            foreach (var lang in AllLanguages)
            {
                try { 105.50m.ToText(Currency.EUR, lang, Fraction); supported.Add(lang); }
                catch (NotSupportedException) { }
            }

            Assert.That(supported, Is.EquivalentTo(new[]
            {
                Language.English, Culture.EnglishUS, Culture.EnglishGB,
                Language.Spanish, Culture.Spanish,
                Language.Portuguese, Culture.PortugueseBR,
                Language.Bulgarian, Culture.Bulgarian,
                Language.Latvian, Culture.Latvian,
                Language.Amharic, Culture.EthiopianAM,
            }));
        }

        private static IEnumerable<string> AllLanguages
        {
            get { return Extensions.SupportedLanguages; }
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
