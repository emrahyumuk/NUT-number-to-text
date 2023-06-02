namespace Nut.Tests
{
    internal class Billions : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 1_000_000_000, "jeden miliard złotych zero groszy")]
        [TestCase("pln", "pl-PL", 2_000_000_000, "dwa miliardy złotych zero groszy")]
        [TestCase("pln", "pl-PL", 6_000_000_000.01, "sześć miliardów złotych jeden grosz")]
        [TestCase("pln", "pl-PL", 6_543_210_987.65, "sześć miliardów pięćset czterdzieści trzy miliony dwieście dziesięć tysięcy dziewięćset osiemdziesiąt siedem złotych sześćdziesiąt pięć groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
