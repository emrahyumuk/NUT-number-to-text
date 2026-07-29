namespace Nut.Tests
{
    /// <summary>
    /// Portuguese joins the whole and fractional parts of an amount with "e". The converter
    /// used "com", which reads as "with" — not how an amount is said or written on a
    /// cheque. Reported as #27.
    /// </summary>
    [TestFixture]
    public class PortugueseSeparator
    {
        [TestCase(1.01, "um real e um centavo")]
        [TestCase(123.45, "cento e vinte e três reais e quarenta e cinco centavos")]
        [TestCase(2.02, "dois reais e dois centavos")]
        public void PartsAreJoinedWithE(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.BRL, Language.Portuguese), Is.EqualTo(expected));
        }
    }
}
