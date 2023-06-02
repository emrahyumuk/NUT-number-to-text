namespace Nut.Tests
{
    public class ToTextTestBase
    {
        public void BaseToText_ForDecimalNumber_ExpectCorrectText(string currency, string lang, decimal number, string expectedText)
        {
            var moneyText = number.ToText(currency, lang).ToLower();

            Assert.AreEqual(expectedText, moneyText);
        }
    }
}
