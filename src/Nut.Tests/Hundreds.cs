namespace Nut.Tests
{
    public class Hundreds : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 100, "sto złotych zero groszy")]
        [TestCase("pln", "pl-PL", 123, "sto dwadzieścia trzy złote zero groszy")]
        [TestCase("pln", "pl-PL", 223.65, "dwieście dwadzieścia trzy złote sześćdziesiąt pięć groszy")]

        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
