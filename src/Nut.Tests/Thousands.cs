namespace Nut.Tests
{
    public class Thousands : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 1_000, "jeden tysiąc złotych zero groszy")]
        [TestCase("pln", "pl-PL", 3_000, "trzy tysiące złotych zero groszy")]
        [TestCase("pln", "pl-PL", 5_000, "pięć tysięcy złotych zero groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
