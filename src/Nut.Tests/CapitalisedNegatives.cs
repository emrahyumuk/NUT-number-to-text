namespace Nut.Tests
{
    /// <summary>
    /// The amount-in-words field on a cheque or invoice is usually capitalised, and it is
    /// the field the bank pays against. Capitalising the main unit put the capital on the
    /// wrong word once a sign was in front of it: "minus Forty-one dollars".
    /// </summary>
    [TestFixture]
    public class CapitalisedNegatives
    {
        private static Options Caps => new Options
        {
            MainUnitFirstCharUpper = true,
            CurrencyFirstCharUpper = true,
            SubUnitFirstCharUpper = true,
        };

        [TestCase(Language.English, Currency.USD, "Minus forty-one Dollars Fifty Cents")]
        [TestCase(Language.Turkish, Currency.TRY, "Eksi kırk bir Türk lirası Elli Kuruş")]
        [TestCase(Language.Russian, Currency.RUB, "Минус сорок один Рубль Пятьдесят Копеек")]
        public void TheSignTakesTheCapital(string lang, string currency, string expected)
        {
            Assert.That((-41.5m).ToText(currency, lang, Caps), Is.EqualTo(expected));
        }

        [Test]
        public void PositiveAmountsAreUnchanged()
        {
            Assert.That(41.5m.ToText(Currency.USD, Language.English, Caps),
                Is.EqualTo("Forty-one Dollars Fifty Cents"));
        }

        /// <summary>Without the option, nothing is capitalised — including the sign.</summary>
        [Test]
        public void NoCapitalWhenNoneIsAskedFor()
        {
            Assert.That((-41.5m).ToText(Currency.USD, Language.English),
                Is.EqualTo("minus forty-one dollars fifty cents"));
        }

        /// <summary>Exactly one capital at the start, never two.</summary>
        [TestCase(Language.English, Currency.USD)]
        [TestCase(Language.Uzbek, Currency.UZS)]
        [TestCase(Language.Russian, Currency.RUB)]
        public void OnlyTheFirstWordIsCapitalised(string lang, string currency)
        {
            var text = (-41.5m).ToText(currency, lang, new Options { MainUnitFirstCharUpper = true });
            var words = text.Split(' ');
            Assert.That(char.IsUpper(words[0][0]), Is.True, "first word should be capitalised");
            Assert.That(words[1], Is.EqualTo(words[1].ToLowerInvariant()),
                "the word after the sign should not be capitalised");
        }
    }
}
