using System;

namespace Nut.Tests
{
    /// <summary>
    /// The library supports numbers below one trillion in magnitude. Going past that used
    /// to throw a bare <see cref="Exception"/>, which a caller cannot catch selectively
    /// without swallowing everything else.
    /// </summary>
    [TestFixture]
    public class NumberLimit
    {
        [TestCase(1000000000000L)]
        [TestCase(-1000000000000L)]
        [TestCase(long.MaxValue)]
        [TestCase(long.MinValue)]
        public void PastTheLimitThrowsArgumentOutOfRange(long number)
        {
            Assert.That(() => number.ToText(Language.English),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(999999999999L)]
        [TestCase(-999999999999L)]
        public void TheLimitItselfIsInclusive(long number)
        {
            Assert.That(() => number.ToText(Language.English), Throws.Nothing);
        }

        /// <summary>The message should say what the range is, not just that something failed.</summary>
        [Test]
        public void TheMessageStatesTheRange()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => 1000000000000L.ToText(Language.English));
            Assert.That(ex.Message, Does.Contain("999999999999"));
        }
    }
}
