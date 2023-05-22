namespace Nut.Tests
{
    [TestFixture]
    public class Tens : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 10, "dziesięć złotych zero groszy")]
        [TestCase("pln", "pl-PL", 11, "jedenaście złotych zero groszy")]
        [TestCase("pln", "pl-PL", 12.13, "dwanaście złotych trzynaście groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
