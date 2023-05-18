namespace Nut.Tests
{
    [TestFixture]
    public class Unity
    {
        [TestCase("pln", "pl-PL", 1, "jeden złoty zero groszy")]
        [TestCase("pln", "pl-PL", 2, "dwa złote zero groszy")]
        [TestCase("pln", "pl-PL", 5, "pięć złotych zero groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            var moneyText = number.ToText(currency, lang).ToLower();

            Assert.AreEqual(expectedText, moneyText);
        }
    }
}
