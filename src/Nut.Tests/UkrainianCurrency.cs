using System;

namespace Nut.Tests
{
    /// <summary>
    /// Ukrainian currency wording. Most of this table had been copied from Russian, so it
    /// read as Russian words spelled in Ukrainian context ("доллар", "Нуль центов"), and
    /// the Polish sub unit said "грубий", which means "coarse".
    ///
    /// Ukrainian agreement takes three forms: nominative singular after 1, nominative
    /// plural after 2-4, genitive plural after 5 and above.
    /// </summary>
    [TestFixture]
    public class UkrainianCurrency
    {
        [TestCase(Currency.UAH, 1, "Одна гривня Нуль копійок")]
        [TestCase(Currency.UAH, 2, "Дві гривні Нуль копійок")]
        [TestCase(Currency.UAH, 5, "П'ять гривень Нуль копійок")]
        [TestCase(Currency.USD, 1, "Один долар Нуль центів")]
        [TestCase(Currency.USD, 2, "Два долари Нуль центів")]
        [TestCase(Currency.USD, 5, "П'ять доларів Нуль центів")]
        [TestCase(Currency.EUR, 1, "Один євро Нуль євроцентів")]
        [TestCase(Currency.EUR, 5, "П'ять євро Нуль євроцентів")]
        [TestCase(Currency.RUB, 1, "Один рубль Нуль копійок")]
        [TestCase(Currency.RUB, 5, "П'ять рублів Нуль копійок")]
        [TestCase(Currency.TRY, 1, "Одна турецька ліра Нуль курушів")]
        [TestCase(Currency.TRY, 2, "Дві турецькі ліри Нуль курушів")]
        [TestCase(Currency.TRY, 5, "П'ять турецьких лір Нуль курушів")]
        [TestCase(Currency.PLN, 1, "Один злотий Нуль грошів")]
        [TestCase(Currency.PLN, 2, "Два злоті Нуль грошів")]
        [TestCase(Currency.PLN, 5, "П'ять злотих Нуль грошів")]
        [TestCase(Currency.ETB, 1, "Один бир Нуль центів")]
        [TestCase(Currency.ETB, 5, "П'ять бирів Нуль центів")]
        public void Wording(string currency, decimal amount, string expected)
        {
            Assert.That(amount.ToText(currency, Language.Ukrainian), Is.EqualTo(expected));
        }

        /// <summary>
        /// The birr had only two name forms while the converter indexes three, so any count
        /// of five or more threw instead of returning text.
        /// </summary>
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(100)]
        public void BirrDoesNotThrowForLargerCounts(decimal amount)
        {
            Assert.That(() => amount.ToText(Currency.ETB, Language.Ukrainian), Throws.Nothing);
        }

        /// <summary>Every currency the converter claims to support must survive all three
        /// agreement forms; a short Names array would throw on the third.</summary>
        [Test]
        public void EveryCurrencyHasAllThreeForms()
        {
            var currencies = new[] { Currency.EUR, Currency.USD, Currency.RUB, Currency.TRY,
                Currency.UAH, Currency.ETB, Currency.PLN };

            Assert.Multiple(() =>
            {
                foreach (var currency in currencies)
                    foreach (var amount in new decimal[] { 1, 2, 5, 11, 21, 25 })
                        Assert.That(() => amount.ToText(currency, Language.Ukrainian),
                            Throws.Nothing, $"{currency} at {amount}");
            });
        }
    }
}
