namespace Nut.Models {
    public class BaseCurrencyModel {
        public string[] Names { get; set; }

        /// <summary>
        /// Grammatical gender of the unit name, for languages where the numeral in front of
        /// it has to agree: "один рубль" but "одна гривня". Left as
        /// <see cref="GenderGroup.None"/> by converters whose numerals do not inflect, and
        /// treated as masculine by those that do.
        /// </summary>
        public GenderGroup Gender { get; set; }
    }
}
