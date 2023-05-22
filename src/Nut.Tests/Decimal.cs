namespace Nut.Tests
{
    [TestFixture]
    public class Decimal : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 1.01, "jeden złoty jeden grosz")]
        [TestCase("pln", "pl-PL", 1.02, "jeden złoty dwa grosze")]
        [TestCase("pln", "pl-PL", 1.20, "jeden złoty dwadzieścia groszy")]
        [TestCase("pln", "pl-PL", 4.40, "cztery złote czterdzieści groszy")]
        [TestCase("pln", "pl-PL", 123.45, "sto dwadzieścia trzy złote czterdzieści pięć groszy")]
        [TestCase("pln", "pl-PL", 0.45, "zero złotych czterdzieści pięć groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
