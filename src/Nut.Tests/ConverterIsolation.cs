using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Nut.Tests
{
    /// <summary>
    /// The four Slavic converters pick gendered forms per conversion. They used to do that
    /// by writing into the singleton's shared word table, which leaked into the next call
    /// and corrupted concurrent ones — 17938 of 50000 parallel conversions came back wrong.
    /// These tests fail if that shared state ever comes back.
    /// </summary>
    [TestFixture]
    public class ConverterIsolation
    {
        [TestCase(Language.Russian, Currency.RUB, Currency.UAH)]
        [TestCase(Language.Ukrainian, Currency.UAH, Currency.RUB)]
        [TestCase(Language.Belarusian, Currency.BYN, Currency.UAH)]
        [TestCase(Language.Bulgarian, Currency.BGN, Currency.RUB)]
        public void PlainNumbersDoNotChangeAfterAMoneyConversion(string lang, string a, string b)
        {
            var before = 1L.ToText(lang) + "|" + 2L.ToText(lang);

            1m.ToText(a, lang);
            2m.ToText(b, lang);
            1m.ToText(b, lang);

            Assert.That(1L.ToText(lang) + "|" + 2L.ToText(lang), Is.EqualTo(before));
        }

        [TestCase(Language.Russian, Currency.RUB, Currency.UAH)]
        [TestCase(Language.Belarusian, Currency.BYN, Currency.UAH)]
        public void MoneyConversionsDoNotAffectEachOther(string lang, string a, string b)
        {
            var first = 1m.ToText(a, lang);
            var second = 1m.ToText(b, lang);

            Assert.That(1m.ToText(a, lang), Is.EqualTo(first));
            Assert.That(1m.ToText(b, lang), Is.EqualTo(second));
            Assert.That(first, Is.Not.EqualTo(second), "these two currencies should differ");
        }

        [Test]
        public void ConcurrentConversionsAgreeWithSequentialOnes()
        {
            var ruble = 1m.ToText(Currency.RUB, Language.Russian);
            var hryvnia = 1m.ToText(Currency.UAH, Language.Russian);

            var wrong = new ConcurrentBag<string>();
            Parallel.For(0, 20000, i =>
            {
                var useRuble = i % 2 == 0;
                var actual = useRuble
                    ? 1m.ToText(Currency.RUB, Language.Russian)
                    : 1m.ToText(Currency.UAH, Language.Russian);
                var expected = useRuble ? ruble : hryvnia;
                if (actual != expected) wrong.Add(actual);
            });

            Assert.That(wrong, Is.Empty);
        }
    }
}
