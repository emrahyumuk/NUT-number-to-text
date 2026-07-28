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

        /// <summary>Ukrainian follows the same rule; its table was feminine-first, which made
        /// bare numerals and millions feminine while thousands happened to come out right.</summary>
        [TestCase(1, "Один")]
        [TestCase(2, "Два")]
        [TestCase(1000, "Одна тисяча")] // тисяча feminine
        [TestCase(2000, "Дві тисячі")]
        [TestCase(41000, "Сорок Одна тисяча")]
        [TestCase(1001, "Одна тисяча Один")] // feminine, then masculine
        [TestCase(1000000, "Один мільйон")] // мільйон masculine
        [TestCase(2000000, "Два мільйони")]
        [TestCase(1000000000, "Один мільярд")]
        public void UkrainianPlainNumbers(long number, string expected)
        {
            Assert.That(number.ToText(Language.Ukrainian), Is.EqualTo(expected));
        }

        [TestCase(41000, "Сорок Одна тисяча гривень Нуль копійок")]
        [TestCase(1000000, "Один мільйон гривень Нуль копійок")]
        public void UkrainianHryvnia(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.UAH, Language.Ukrainian), Is.EqualTo(expected));
        }

        /// <summary>Bulgarian drops the leading one before хиляда but keeps it before
        /// милион, and милион is masculine where хиляда is feminine.</summary>
        [TestCase(1, "един")]
        [TestCase(1000, "хиляда")]
        [TestCase(2000, "две хиляди")] // хиляда feminine
        [TestCase(5000, "пет хиляди")]
        [TestCase(21000, "двадесет и една хиляди")]
        [TestCase(100000, "сто хиляди")]
        [TestCase(1000000, "един милион")] // милион masculine, and counted explicitly
        [TestCase(2000000, "два милиона")]
        [TestCase(1000000000, "един милиард")]
        public void BulgarianPlainNumbers(long number, string expected)
        {
            Assert.That(number.ToText(Language.Bulgarian), Is.EqualTo(expected));
        }

        [TestCase(1, "един лев и нула стотинки")]
        [TestCase(2, "два лева и нула стотинки")]
        [TestCase(1000000, "един милион лева и нула стотинки")]
        public void BulgarianLev(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.BGN, Language.Bulgarian), Is.EqualTo(expected));
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
