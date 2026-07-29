namespace Nut.Tests
{
    /// <summary>
    /// Latvian, from <see href="https://github.com/emrahyumuk/NUT-number-to-text/pull/24">#24</see>.
    /// Numerals agree in gender with what they count, and the unit name takes three forms:
    /// singular after a count ending in one, genitive plural after zero, plural otherwise.
    /// </summary>
    [TestFixture]
    public class LatvianNumerals
    {
        [TestCase(0, "nulle")]
        [TestCase(1, "viens")]
        [TestCase(2, "divi")]
        [TestCase(11, "vienpadsmit")]
        [TestCase(21, "divdesmit viens")]
        [TestCase(100, "simts")]
        [TestCase(200, "divi simti")]
        [TestCase(1000, "viens tūkstotis")]
        [TestCase(2000, "divi tūkstoši")]
        [TestCase(5000, "pieci tūkstoši")]
        [TestCase(1234, "viens tūkstotis divi simti trīsdesmit četri")]
        [TestCase(1000000, "viens miljons")]
        public void Numbers(long number, string expected)
        {
            Assert.That(number.ToText(Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>Zero takes the genitive plural: "nulle centu", not "nulle centi".</summary>
        [TestCase(1, "viens eiro un nulle centu")]
        [TestCase(2, "divi eiro un nulle centu")]
        [TestCase(21, "divdesmit viens eiro un nulle centu")]
        [TestCase(1000, "viens tūkstotis eiro un nulle centu")]  // eiro does not decline
        [TestCase(1234.56, "viens tūkstotis divi simti trīsdesmit četri eiro un piecdesmit seši centi")]
        public void Euro(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.EUR, Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>mārciņa is feminine, so the numeral agrees.</summary>
        [TestCase(1, "viena sterliņu mārciņa un nulle pensu")]
        [TestCase(2, "divas sterliņu mārciņas un nulle pensu")]
        [TestCase(21, "divdesmit viena sterliņu mārciņa un nulle pensu")]
        public void PoundIsFeminine(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.GBP, Language.Latvian), Is.EqualTo(expected));
        }

        /// <summary>Singular after a count ending in one — including 21 — but not after
        /// eleven, where the -teen blocks it.</summary>
        [TestCase(1, "cents")]
        [TestCase(21, "cents")]
        [TestCase(11, "centi")]
        [TestCase(2, "centi")]
        public void SubUnitInflection(int cents, string expectedWord)
        {
            var amount = 1m + cents / 100m;
            Assert.That(amount.ToText(Currency.EUR, Language.Latvian), Does.EndWith(expectedWord));
        }

        /// <summary>
        /// tūkstotis, miljons and miljards are masculine nouns, so the count in front of one
        /// agrees with it and not with the currency, and the scale word inflects on the same
        /// rule as everything else here — singular after a count ending in one.
        /// </summary>
        [TestCase(1000, "viens tūkstotis")]
        [TestCase(2000, "divi tūkstoši")]
        [TestCase(21000, "divdesmit viens tūkstotis")]
        [TestCase(41000, "četrdesmit viens tūkstotis")]
        [TestCase(11000, "vienpadsmit tūkstoši")]
        [TestCase(1000000, "viens miljons")]
        [TestCase(21000000, "divdesmit viens miljons")]
        public void ScaleWordsStayMasculineAndInflect(decimal amount, string count)
        {
            Assert.That(amount.ToText(Currency.GBP, Language.Latvian),
                Does.StartWith(count + " sterliņu mārciņu"));
            Assert.That(amount.ToText(Currency.USD, Language.Latvian),
                Does.StartWith(count + " ASV dolāru"));
        }

        /// <summary>
        /// The declinable scale words — simts, tūkstotis, miljons, miljards — govern the
        /// genitive plural of what they count. This test previously pinned the nominative,
        /// which is wrong on exactly the amounts a document is most likely to carry.
        /// <para>
        /// Source: uzdevumi.lv, "Aiz lokāmajām formām desmits, simts, tūkstotis, miljons,
        /// miljards pieņemts lietot lietvārdu daudzskaitļa ģenitīvā"; and
        /// valodaskonsultacijas.lv, "pieci tūkstoši cilvēku", "četri simti iedzīvotāju".
        /// </para>
        /// </summary>
        [TestCase(100, "simts ASV dolāru")]
        [TestCase(200, "divi simti ASV dolāru")]
        [TestCase(1000, "viens tūkstotis ASV dolāru")]
        [TestCase(1100, "viens tūkstotis simts ASV dolāru")]
        [TestCase(1200, "viens tūkstotis divi simti ASV dolāru")]
        [TestCase(2000, "divi tūkstoši ASV dolāru")]
        [TestCase(21000, "divdesmit viens tūkstotis ASV dolāru")]
        [TestCase(100000, "simts tūkstoši ASV dolāru")]
        [TestCase(1000000, "viens miljons ASV dolāru")]
        [TestCase(1000000000, "viens miljards ASV dolāru")]
        public void ScaleWordsGovernTheGenitivePlural(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.Latvian),
                Is.EqualTo(expected + " un nulle centu"));
        }

        /// <summary>
        /// Ten does not decline, so it governs nothing and the noun stays nominative. This
        /// is the boundary the "ends in two zeros" test has to get right.
        /// </summary>
        [TestCase(10, "desmit ASV dolāri")]
        [TestCase(110, "simts desmit ASV dolāri")]
        [TestCase(120, "simts divdesmit ASV dolāri")]
        [TestCase(101, "simts viens ASV dolārs")]
        [TestCase(1234, "viens tūkstotis divi simti trīsdesmit četri ASV dolāri")]
        public void TenDoesNotGovernTheGenitive(decimal amount, string expected)
        {
            Assert.That(amount.ToText(Currency.USD, Language.Latvian),
                Is.EqualTo(expected + " un nulle centu"));
        }

        [Test]
        public void GeneralBehaviourApplies()
        {
            Assert.That((-41L).ToText(Language.Latvian), Is.EqualTo("mīnus četrdesmit viens"));
            Assert.That(101.ToText(Culture.Latvian), Is.EqualTo("simts viens"));
        }
    }
}
