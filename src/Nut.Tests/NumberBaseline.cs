namespace Nut.Tests
{
    /// <summary>
    /// Pins the current plain-number output of every language so later refactors have to
    /// declare what they change. Values here are what the library produces today, which is
    /// not always what the language actually requires — cases known to be wrong carry a
    /// BUG comment and are expected to change when that defect is fixed.
    ///
    /// Numbers containing 1 or 2 are missing for Russian, Ukrainian, Belarusian and
    /// Bulgarian on purpose: those converters rewrite their shared word table at runtime,
    /// so the result depends on what ran before. See <see cref="KnownDefects"/>.
    /// </summary>
    [TestFixture]
    public class NumberBaseline
    {
        private static void Check(long number, string lang, string expected)
        {
            Assert.That(number.ToText(lang), Is.EqualTo(expected));
        }

        [TestCase(0, "zero")]
        [TestCase(1, "one")]
        [TestCase(2, "two")]
        [TestCase(11, "eleven")]
        [TestCase(15, "fifteen")]
        [TestCase(20, "twenty")]
        [TestCase(21, "twenty one")] // BUG: compounds 21-99 are always hyphenated -> "twenty-one" (Merriam-Webster)
        [TestCase(42, "forty two")] // BUG: -> "forty-two"
        [TestCase(100, "one hundred")]
        [TestCase(101, "one hundred one")]
        [TestCase(200, "two hundred")]
        [TestCase(999, "nine hundred ninety nine")]
        [TestCase(1000, "one thousand")]
        [TestCase(41000, "forty one thousand")]
        [TestCase(1000000, "one million")]
        [TestCase(1000000000, "one billion")]
        public void English(long number, string expected) => Check(number, Language.English, expected);

        [TestCase(0, "zéro")]
        [TestCase(1, "un")]
        [TestCase(11, "onze")]
        [TestCase(20, "vingt")]
        [TestCase(21, "vingt et un")]
        [TestCase(42, "quarante deux")] // BUG: hyphenated both traditionally and post-1990 -> "quarante-deux" (BDL/Académie)
        [TestCase(100, "cent")]
        [TestCase(101, "cent un")]
        [TestCase(200, "deux cent")] // BUG: multiplied "cent" with nothing after it takes -s -> "deux cents" (BDL)
        [TestCase(999, "neuf cent quatre-vingt-dix-neuf")] // hyphenated here but not at 42 — inconsistent
        [TestCase(1000, "mille")]
        [TestCase(41000, "quarante et un mille")]
        [TestCase(1000000, "un million")]
        [TestCase(2000000, "deux million")] // BUG: million is a noun and takes -s -> "deux millions"
        [TestCase(1000000000, "un milliard")]
        public void French(long number, string expected) => Check(number, Language.French, expected);

        [TestCase(0, "null")]
        [TestCase(1, "ein")] // BUG: standalone one is "eins"; "ein" is only attributive (Duden)
        [TestCase(11, "elf")]
        [TestCase(20, "zwanzig")]
        [TestCase(21, "einundzwanzig")]
        [TestCase(42, "zweiundvierzig")]
        // Duden: numbers below a million are written as one closed-up word, above a million separated.
        [TestCase(100, "ein hundert")] // BUG: -> "einhundert"
        [TestCase(101, "ein hundert ein")] // BUG: -> "einhunderteins"
        [TestCase(999, "neun hundert neunundneunzig")] // BUG: -> "neunhundertneunundneunzig"
        [TestCase(1000, "eintausend")] // closed up here, but not at 2000 — inconsistent
        [TestCase(2000, "zwei tausend")] // BUG: -> "zweitausend"
        [TestCase(41000, "einundvierzig tausend")] // BUG: -> "einundvierzigtausend"
        [TestCase(1000000, "eine Million")]
        [TestCase(2000000, "zwei Millionen")]
        [TestCase(1000000000, "eine Milliarde")]
        public void German(long number, string expected) => Check(number, Language.German, expected);

        [TestCase(0, "cero")]
        [TestCase(1, "uno")]
        [TestCase(11, "once")]
        [TestCase(20, "veinte")]
        [TestCase(21, "veintiuno")] // full form standing alone; "veintiún" only before a noun
        [TestCase(42, "cuarenta y dos")]
        [TestCase(100, "cien")]
        [TestCase(101, "ciento uno")]
        [TestCase(200, "doscientos")]
        [TestCase(999, "novecientos noventa y nueve")]
        [TestCase(1000, "mil")]
        [TestCase(2000, "dos mil")]
        [TestCase(41000, "cuarenta y un mil")]
        [TestCase(1000000, "un millón")]
        [TestCase(2000000, "dos millones")]
        [TestCase(1000000000, "mil millones")]
        public void Spanish(long number, string expected) => Check(number, Language.Spanish, expected);

        [TestCase(0, "zero")]
        [TestCase(1, "um")]
        [TestCase(11, "onze")]
        [TestCase(20, "vinte")]
        [TestCase(21, "vinte e um")]
        [TestCase(42, "quarenta e dois")]
        [TestCase(100, "cem")]
        [TestCase(101, "cento e um")]
        [TestCase(200, "duzentos")]
        [TestCase(999, "novecentos e noventa e nove")]
        [TestCase(1000, "um mil")] // "mil" is the usual reading, but "um mil" is accepted and common on cheques — not a defect
        [TestCase(41000, "quarenta e um mil")]
        [TestCase(1000000, "um milhão")]
        [TestCase(2000000, "dois milhões")]
        [TestCase(1000000000, "um bilhão")]
        public void Portuguese(long number, string expected) => Check(number, Language.Portuguese, expected);

        [TestCase(0, "sıfır")]
        [TestCase(1, "bir")]
        [TestCase(11, "on bir")]
        [TestCase(20, "yirmi")]
        [TestCase(21, "yirmi bir")]
        [TestCase(42, "kırk iki")]
        [TestCase(100, "yüz")]
        [TestCase(101, "yüz bir")]
        [TestCase(200, "iki yüz")]
        [TestCase(999, "dokuz yüz doksan dokuz")]
        [TestCase(1000, "bin")]
        [TestCase(2000, "iki bin")]
        [TestCase(41000, "kırk bir bin")]
        [TestCase(1000000, "bir milyon")]
        [TestCase(1000000000, "bir milyar")]
        public void Turkish(long number, string expected) => Check(number, Language.Turkish, expected);

        [TestCase(0, "ноль")]
        [TestCase(1, "один")]
        [TestCase(2, "два")]
        [TestCase(3, "три")]
        [TestCase(11, "одиннадцать")]
        [TestCase(15, "пятнадцать")]
        [TestCase(20, "двадцать")]
        [TestCase(21, "двадцать один")]
        [TestCase(42, "сорок два")]
        [TestCase(100, "сто")]
        [TestCase(101, "сто один")]
        [TestCase(200, "двести")]
        [TestCase(999, "девятьсот девяносто девять")]
        [TestCase(1000, "одна тысяча")]
        [TestCase(2000, "две тысячи")]
        [TestCase(5000, "пять тысяч")]
        [TestCase(41000, "сорок одна тысяча")]
        [TestCase(1000000, "один миллион")] // correct: миллион is masculine
        [TestCase(2000000, "два миллиона")]
        [TestCase(1000000000, "один миллиард")]
        public void Russian(long number, string expected) => Check(number, Language.Russian, expected);

        // Capitalisation of units is still inconsistent with scales ("Одна тисяча"); that
        // is a separate defect from gender.
        [TestCase(0, "Нуль")]
        [TestCase(1, "Один")]
        [TestCase(2, "Два")]
        [TestCase(3, "Три")]
        [TestCase(11, "Одинадцять")]
        [TestCase(20, "Двадцять")]
        [TestCase(21, "Двадцять Один")]
        [TestCase(100, "Сто")]
        [TestCase(999, "Дев'ятсот Дев'яносто Дев'ять")]
        [TestCase(1000, "Одна тисяча")] // тисяча is feminine
        [TestCase(2000, "Дві тисячі")]
        [TestCase(5000, "П'ять тисяч")]
        [TestCase(41000, "Сорок Одна тисяча")]
        [TestCase(1000000, "Один мільйон")] // мільйон is masculine
        [TestCase(1000000000, "Один мільярд")]
        public void Ukrainian(long number, string expected) => Check(number, Language.Ukrainian, expected);

        [TestCase(0, "нуль")]
        [TestCase(1, "адзін")]
        [TestCase(2, "два")]
        [TestCase(3, "тры")]
        [TestCase(11, "адзінаццаць")]
        [TestCase(20, "дваццаць")]
        [TestCase(21, "дваццаць адзін")]
        [TestCase(100, "сто")]
        [TestCase(200, "дзвесце")]
        [TestCase(999, "дзевяцьсот дзевяноста дзевяць")]
        [TestCase(1000, "адна тысяча")]
        [TestCase(2000, "дзве тысячы")]
        [TestCase(5000, "пяць тысяч")]
        [TestCase(41000, "сорак адна тысяча")]
        [TestCase(1000000, "адзін мільён")] // correct: мільён is masculine
        public void Belarusian(long number, string expected) => Check(number, Language.Belarusian, expected);

        [TestCase(0, "нула")]
        [TestCase(1, "един")]
        [TestCase(2, "два")]
        [TestCase(3, "три")]
        [TestCase(11, "единадесет")]
        [TestCase(20, "двадесет")]
        [TestCase(21, "двадесет и един")]
        [TestCase(100, "сто")]
        [TestCase(101, "сто и един")]
        [TestCase(999, "деветстотин деветдесет и девет")]
        [TestCase(1000, "хиляда")] // Bulgarian drops the leading one before хиляда
        [TestCase(2000, "две хиляди")]
        [TestCase(5000, "пет хиляди")]
        [TestCase(41000, "четиридесет и една хиляди")]
        [TestCase(100000, "сто хиляди")]
        [TestCase(1000000, "един милион")] // but keeps it before милион
        [TestCase(2000000, "два милиона")]
        public void Bulgarian(long number, string expected) => Check(number, Language.Bulgarian, expected);

        [TestCase(0, "Zero")]
        [TestCase(1, "Jeden")]
        [TestCase(11, "Jedenaście")]
        [TestCase(20, "Dwadzieścia")]
        [TestCase(21, "Dwadzieścia Jeden")]
        [TestCase(42, "Czterdzieści Dwa")]
        [TestCase(100, "Sto")]
        [TestCase(200, "Dwieście")]
        [TestCase(999, "Dziewięćset Dziewięćdziesiąt Dziewięć")]
        [TestCase(1000, "Jeden tysiąc")] // capitalisation is inconsistent: units are capitalised, scales are not
        [TestCase(5000, "Pięć tysięcy")]
        [TestCase(41000, "Czterdzieści Jeden tysięcy")]
        [TestCase(1000000, "Jeden milion")]
        public void Polish(long number, string expected) => Check(number, Language.Polish, expected);

        [TestCase(0, "ዜሮ")]
        [TestCase(1, "አንድ")]
        [TestCase(11, "አስራ አንድ")]
        [TestCase(20, "ሀያ")]
        [TestCase(100, "አንድ መቶ")]
        [TestCase(999, "ዘጠኝ መቶ ዘጠና ዘጠኝ")]
        [TestCase(1000, "አንድ ሺህ")]
        [TestCase(1000000, "አንድ ሚሊዮን")]
        public void Amharic(long number, string expected) => Check(number, Language.Amharic, expected);
    }
}
