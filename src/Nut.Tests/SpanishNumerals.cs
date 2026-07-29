namespace Nut.Tests
{
    /// <summary>
    /// Spanish numerals, per RAE. Three separate rules were wrong:
    /// <list type="bullet">
    /// <item>10^9 was rendered as "billón", which in Spanish means 10^12 — out by a
    /// factor of a thousand. 10^9 is "mil millones".</item>
    /// <item>"uno" was not apocopated before a noun or a scale word: "uno mil",
    /// "uno millón", "uno euro".</item>
    /// <item>Hundreds above 100 used their feminine form whenever something followed, so
    /// 999 read as "novecientas" while 200 stayed masculine. Correcting that pinned them to
    /// the masculine form instead, which is wrong the other way round in front of a
    /// feminine currency; they agree with what they count.</item>
    /// </list>
    /// </summary>
    [TestFixture]
    public class SpanishNumerals
    {
        /// <summary>Spanish "billón" is 10^12; the English billion is "mil millones".</summary>
        [TestCase(1000000000, "mil millones")]
        [TestCase(2000000000, "dos mil millones")]
        public void TenToTheNinthIsMilMillones(long number, string expected)
        {
            Assert.That(number.ToText(Language.Spanish), Is.EqualTo(expected));
        }

        /// <summary>"mil" carries no leading one; "millón" does.</summary>
        [TestCase(1000, "mil")]
        [TestCase(2000, "dos mil")]
        [TestCase(21000, "veintiún mil")]
        [TestCase(41000, "cuarenta y un mil")]
        [TestCase(100000, "cien mil")]
        [TestCase(1000000, "un millón")]
        [TestCase(2000000, "dos millones")]
        [TestCase(21000000, "veintiún millones")]
        public void OneIsApocopatedBeforeScaleWords(long number, string expected)
        {
            Assert.That(number.ToText(Language.Spanish), Is.EqualTo(expected));
        }

        /// <summary>Standing alone the numeral keeps its full form.</summary>
        [TestCase(1, "uno")]
        [TestCase(21, "veintiuno")]
        [TestCase(41, "cuarenta y uno")]
        [TestCase(121, "ciento veintiuno")]
        public void StandingAloneItIsNotApocopated(long number, string expected)
        {
            Assert.That(number.ToText(Language.Spanish), Is.EqualTo(expected));
        }

        /// <summary>A currency name is a noun, so the amount in front of it apocopates.</summary>
        [TestCase(1, "un euro con cero céntimo de euro")]
        [TestCase(21, "veintiún euros con cero céntimo de euro")]
        [TestCase(2, "dos euros con cero céntimo de euro")]
        public void AmountsApocopateBeforeTheCurrency(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.Spanish), Is.EqualTo(expected));
        }

        [Test]
        public void SubUnitApocopatesToo()
        {
            Assert.That(1.01m.ToText(Currency.EUR, Language.Spanish),
                Is.EqualTo("un euro con un céntimo de euro"));
        }

        /// <summary>"cien" becomes "ciento" when something follows; the other hundreds keep
        /// their masculine form, which is the one the bare numeral takes.</summary>
        [TestCase(100, "cien")]
        [TestCase(101, "ciento uno")]
        [TestCase(200, "doscientos")]
        [TestCase(201, "doscientos uno")]
        [TestCase(900, "novecientos")]
        [TestCase(999, "novecientos noventa y nueve")]
        public void HundredsUseTheMasculineForm(long number, string expected)
        {
            Assert.That(number.ToText(Language.Spanish), Is.EqualTo(expected));
        }

        /// <summary>
        /// Counting something feminine, they take their feminine form. The table has carried
        /// those forms all along and nothing read them, so every amount in the only feminine
        /// currency Spanish has came out masculine.
        /// </summary>
        [TestCase(200, "doscientas libras esterlinas")]
        [TestCase(900, "novecientas libras esterlinas")]
        [TestCase(999, "novecientas noventa y nueve libras esterlinas")]
        // "cien" and "ciento" do not inflect for gender.
        [TestCase(100, "cien libras esterlinas")]
        [TestCase(101, "ciento una libras esterlinas")]
        // mil is an adjective and does not interrupt the agreement; millón is a noun and does.
        [TestCase(200000, "doscientas mil libras esterlinas")]
        [TestCase(200000000, "doscientos millones de libras esterlinas")]
        public void HundredsAgreeWithAFeminineCurrency(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.GBP, Language.Spanish),
                Is.EqualTo(expected + " con cero penique"));
        }

        /// <summary>RAE: millón is a noun and links what it counts with "de"; mil is an
        /// adjective and takes none.</summary>
        [TestCase(1000000, "un millón de euros con cero céntimo de euro")]
        [TestCase(2000000, "dos millones de euros con cero céntimo de euro")]
        [TestCase(1000000000, "mil millones de euros con cero céntimo de euro")]
        public void ScaleNounsLinkWithDe(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.Spanish), Is.EqualTo(expected));
        }

        /// <summary>No "de" when mil ends the amount, or when the scale noun is not final.</summary>
        [TestCase(1000, "mil euros con cero céntimo de euro")]
        [TestCase(2000, "dos mil euros con cero céntimo de euro")]
        [TestCase(1500000, "un millón quinientos mil euros con cero céntimo de euro")]
        public void NoDeWhereItDoesNotBelong(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.Spanish), Is.EqualTo(expected));
        }

        [Test]
        public void LargeNumberReadsEndToEnd()
        {
            Assert.That(999999999L.ToText(Language.Spanish), Is.EqualTo(
                "novecientos noventa y nueve millones novecientos noventa y nueve mil novecientos noventa y nueve"));
        }
    }
}
