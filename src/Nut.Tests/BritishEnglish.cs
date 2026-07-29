namespace Nut.Tests
{
    /// <summary>
    /// British usage puts "and" before the final group when it is under a hundred;
    /// American usage drops it. Both cultures were resolving to the same converter, so
    /// asking for en-GB produced American wording.
    ///
    /// This matters more than style here: on a cheque the written amount is the one the
    /// bank honours, so the two conventions are kept apart rather than merged.
    /// </summary>
    [TestFixture]
    public class BritishEnglish
    {
        [TestCase(101, "one hundred and one")]
        [TestCase(110, "one hundred and ten")]
        [TestCase(121, "one hundred and twenty-one")]
        [TestCase(999, "nine hundred and ninety-nine")]
        [TestCase(1001, "one thousand and one")]
        [TestCase(5026, "five thousand and twenty-six")]
        [TestCase(1101, "one thousand one hundred and one")]
        [TestCase(1000001, "one million and one")]
        public void AndBeforeTheFinalGroup(long number, string expected)
        {
            Assert.That(number.ToText(Culture.EnglishGB), Is.EqualTo(expected));
        }

        /// <summary>No "and" when the number ends on a round hundred or higher.</summary>
        [TestCase(100, "one hundred")]
        [TestCase(1000, "one thousand")]
        [TestCase(1100, "one thousand one hundred")]
        [TestCase(100000, "one hundred thousand")]
        [TestCase(21, "twenty-one")]
        public void NoAndWhereItDoesNotBelong(long number, string expected)
        {
            Assert.That(number.ToText(Culture.EnglishGB), Is.EqualTo(expected));
        }

        /// <summary>American wording is unchanged.</summary>
        [TestCase(101, "one hundred one")]
        [TestCase(5026, "five thousand twenty-six")]
        public void AmericanStaysAsItWas(long number, string expected)
        {
            Assert.That(number.ToText(Culture.EnglishUS), Is.EqualTo(expected));
            Assert.That(number.ToText(Language.English), Is.EqualTo(expected));
        }

        [Test]
        public void AmountsFollowTheSameRule()
        {
            Assert.That(101.25m.ToText(Currency.GBP, Culture.EnglishGB),
                Is.EqualTo("one hundred and one pounds sterling twenty-five pence"));
            Assert.That(101.25m.ToText(Currency.USD, Culture.EnglishUS),
                Is.EqualTo("one hundred one dollars twenty-five cents"));
        }

        [Test]
        public void CaseStillDoesNotMatter()
        {
            Assert.That(101.ToText("EN-GB"), Is.EqualTo("one hundred and one"));
            Assert.That(101.ToText("en-gb"), Is.EqualTo("one hundred and one"));
        }
    }
}
