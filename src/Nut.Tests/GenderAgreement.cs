namespace Nut.Tests
{
    /// <summary>
    /// The count in front of a scale word agrees with that scale word, not with the
    /// currency: тысяча is feminine, миллион is masculine, and both appear in the same
    /// amount. Closes issue #25.
    /// </summary>
    [TestFixture]
    public class GenderAgreement
    {
        [TestCase(1000, "одна тысяча рублей ноль копеек")]
        [TestCase(2000, "две тысячи рублей ноль копеек")]
        [TestCase(21000, "двадцать одна тысяча рублей ноль копеек")]
        [TestCase(22000, "двадцать две тысячи рублей ноль копеек")]
        [TestCase(41000, "сорок одна тысяча рублей ноль копеек")]
        [TestCase(42000, "сорок две тысячи рублей ноль копеек")]
        [TestCase(1000000, "один миллион рублей ноль копеек")] // миллион stays masculine
        [TestCase(2000000, "два миллиона рублей ноль копеек")]
        [TestCase(1001, "одна тысяча один рубль ноль копеек")] // feminine before тысяча, masculine before рубль
        [TestCase(2002, "две тысячи два рубля ноль копеек")]
        public void RussianRuble(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.RUB, Language.Russian), Is.EqualTo(expected));
        }

        /// <summary>The scale word decides, so a feminine currency does not change it.</summary>
        [TestCase(41000, "сорок одна тысяча гривень ноль копеек")]
        [TestCase(1000000, "один миллион гривень ноль копеек")]
        public void RussianHryvnia(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.UAH, Language.Russian), Is.EqualTo(expected));
        }

        [TestCase(1000, "адна тысяча беларускіх рублёў нуль капеек")]
        [TestCase(2000, "дзве тысячы беларускіх рублёў нуль капеек")]
        [TestCase(41000, "сорак адна тысяча беларускіх рублёў нуль капеек")]
        [TestCase(42000, "сорак дзве тысячы беларускіх рублёў нуль капеек")]
        [TestCase(1000000, "адзін мільён беларускіх рублёў нуль капеек")] // мільён stays masculine
        public void BelarusianRuble(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.BYN, Language.Belarusian), Is.EqualTo(expected));
        }

        [TestCase(1000, "одна тысяча")]
        [TestCase(41000, "сорок одна тысяча")]
        [TestCase(1000000, "один миллион")]
        public void RussianPlainNumbersAgreeToo(long number, string expected)
        {
            Assert.That(number.ToText(Language.Russian), Is.EqualTo(expected));
        }

        /// <summary>"rur" is a widely used alias for the Russian ruble, like "tl" for "try".</summary>
        [Test]
        public void RurIsAnAliasForRub()
        {
            Assert.That(41000m.ToText(Currency.RUR, Language.Russian),
                Is.EqualTo(41000m.ToText(Currency.RUB, Language.Russian)));
            Assert.That("rur", Is.EqualTo(Currency.RUR));
        }
    }
}
