namespace Nut.Tests
{
    /// <summary>
    /// Some currency names are neuter, and the Slavic languages mark that on the numeral.
    /// GenderGroup only had None, Feminine and Masculine, so anything neuter fell through
    /// to the masculine form: "един евро", "один песо".
    /// </summary>
    [TestFixture]
    public class NeuterGender
    {
        /// <summary>евро and пени are neuter; лев is masculine and стотинка feminine, so
        /// all three genders appear in Bulgarian output.</summary>
        [TestCase(1, Currency.EUR, "едно евро и нула евроцента")]
        [TestCase(2, Currency.EUR, "две евро и нула евроцента")]
        [TestCase(1.01, Currency.GBP, "една британска лира и едно пени")]
        [TestCase(1, Currency.BGN, "един лев и нула стотинки")]
        [TestCase(1.01, Currency.BGN, "един лев и една стотинка")]
        public void Bulgarian(decimal amount, string currency, string expected)
        {
            Assert.That(amount.ToText(currency, Language.Bulgarian), Is.EqualTo(expected));
        }

        /// <summary>песо is neuter in the East Slavic languages too.</summary>
        [TestCase(Language.Russian, "одно аргентинское песо ноль сентаво")]
        [TestCase(Language.Ukrainian, "одне аргентинське песо нуль сентаво")]
        [TestCase(Language.Belarusian, "адно аргентынскае песа нуль сентава")]
        public void PesoIsNeuter(string lang, string expected)
        {
            Assert.That(1m.ToText(Currency.ARS, lang), Is.EqualTo(expected));
        }

        /// <summary>The other two genders keep working.</summary>
        [Test]
        public void MasculineAndFeminineAreUnaffected()
        {
            Assert.That(1m.ToText(Currency.RUB, Language.Russian), Is.EqualTo("один рубль ноль копеек"));
            Assert.That(1m.ToText(Currency.UAH, Language.Russian), Is.EqualTo("одна гривня ноль копеек"));
        }

        /// <summary>Appended to the enum, so the values that existed keep their numbers —
        /// anything persisting a GenderGroup as an int is unaffected.</summary>
        [Test]
        public void ExistingEnumValuesKeepTheirNumbers()
        {
            Assert.That((int)GenderGroup.None, Is.EqualTo(0));
            Assert.That((int)GenderGroup.Feminine, Is.EqualTo(1));
            Assert.That((int)GenderGroup.Masculine, Is.EqualTo(2));
            Assert.That((int)GenderGroup.Neuter, Is.EqualTo(3));
        }
    }
}
