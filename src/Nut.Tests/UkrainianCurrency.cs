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
        [TestCase(Currency.UAH, 1, "одна гривня нуль копійок")]
        [TestCase(Currency.UAH, 2, "дві гривні нуль копійок")]
        [TestCase(Currency.UAH, 5, "п'ять гривень нуль копійок")]
        [TestCase(Currency.USD, 1, "один долар нуль центів")]
        [TestCase(Currency.USD, 2, "два долари нуль центів")]
        [TestCase(Currency.USD, 5, "п'ять доларів нуль центів")]
        [TestCase(Currency.EUR, 1, "один євро нуль євроцентів")]
        [TestCase(Currency.EUR, 5, "п'ять євро нуль євроцентів")]
        [TestCase(Currency.RUB, 1, "один рубль нуль копійок")]
        [TestCase(Currency.RUB, 5, "п'ять рублів нуль копійок")]
        [TestCase(Currency.TRY, 1, "одна турецька ліра нуль курушів")]
        [TestCase(Currency.TRY, 2, "дві турецькі ліри нуль курушів")]
        [TestCase(Currency.TRY, 5, "п'ять турецьких лір нуль курушів")]
        [TestCase(Currency.PLN, 1, "один злотий нуль грошів")]
        [TestCase(Currency.PLN, 2, "два злоті нуль грошів")]
        [TestCase(Currency.PLN, 5, "п'ять злотих нуль грошів")]
        [TestCase(Currency.ETB, 1, "один бир нуль центів")]
        [TestCase(Currency.ETB, 5, "п'ять бирів нуль центів")]
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
