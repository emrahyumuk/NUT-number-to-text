namespace Nut.Tests
{
    public class Millions : ToTextTestBase
    {
        [TestCase("pln", "pl-PL", 1_000_000, "jeden milion złotych zero groszy")]
        [TestCase("pln", "pl-PL", 2_000_000, "dwa miliony złotych zero groszy")]
        [TestCase("pln", "pl-PL", 5_000_000, "pięć milionów złotych zero groszy")]
        [TestCase("pln", "pl-PL", 1_234_567.89, "jeden milion dwieście trzydzieści cztery tysiące pięćset sześćdziesiąt siedem złotych osiemdziesiąt dziewięć groszy")]
        public void ToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            base.BaseToText_ForDecimalNumber_ExpectCorrectText(currency, lang, number, expectedText);
        }
    }
}
