# Changelog

All notable changes to this project are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this
project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Entries before 3.5.0 were reconstructed from git history and the
[NuGet release dates](https://www.nuget.org/packages/Nut/#versions-body-tab); the original
releases shipped without release notes, so those entries record the main change of each
version rather than a complete list.

## [Unreleased]

Every change below alters produced text, so this is shaping up as **4.0.0** rather than a
minor release. Of 4536 checked conversions, **1181 change** relative to the published
3.4.1; Turkish, Polish and Amharic are untouched.

Upgrading from 3.4.1 or 3.5.0 will change the wording your application prints. If you
assert on that text, review the tables below before taking this release.

### Fixed — affecting every language

- **Converters no longer share mutable state.** See 3.5.0; that fix shipped there and
  changed no output.

- **Negative amounts are converted instead of vanishing**
  ([#22](https://github.com/emrahyumuk/NUT-number-to-text/pull/22)). Every `Append` helper
  is guarded by `num > x`, so a negative number matched none of them:

  | Input | Before | After |
  | --- | --- | --- |
  | `(-41).ToText("en")` | `""` | `minus forty-one` |
  | `(-41.5m)` USD | `dollar fifty cents` | `minus forty-one dollars fifty cents` |

  The money case was the dangerous one: the integer part disappeared while the fraction
  survived, producing a plausible-looking but wrong amount. The sign word is per language —
  minus, moins, menos, eksi, минус, мінус, ሲቀነስ — taken from the list in #22.

  The one-trillion limit now applies on both sides; previously an amount below
  -1 000 000 000 000 returned nonsense where its positive twin threw.

- **Amounts carrying more decimals than the currency has are rounded**, rather than read as
  whole sub-units: `123.456` USD gave "four hundred fifty six cents" and now gives "forty-six
  cents". Rounding is half away from zero and carries into the main unit, matching what
  `decimal.ToString("C")` renders for the same value. This also fixes `1.100` and `1.10`
  disagreeing, since `decimal` preserves the scale it was written with.

- **All thirteen `Culture` constants were dead on the `int` overloads.** Those lowercased the
  argument before comparing it against constants like `"en-US"`, so no case ever matched and
  the caller silently got `""` — while the same string worked on the `long` and `decimal`
  overloads.

- **Language and currency matching now ignores case.** `"EN"`, `"en-US"`, `"USD"` and `"TL"`
  resolve like their lower-case forms; an upper-case currency code used to return `""`.

### Fixed — Russian, Ukrainian, Belarusian, Bulgarian

- **The count now agrees with the word it counts.** `тысяча` is feminine and `миллион`
  masculine, and both can occur in one amount
  ([#25](https://github.com/emrahyumuk/NUT-number-to-text/issues/25),
  [#28](https://github.com/emrahyumuk/NUT-number-to-text/pull/28)):

  | Amount | Before | After |
  | --- | --- | --- |
  | `41000 RUB` | сорок **один** тысяча рублей | сорок **одна** тысяча рублей |
  | `1000000 UAH` in Russian | **одна** миллион гривень | **один** миллион гривень |
  | `41000 BYN` | сорак **адзін** тысяча | сорак **адна** тысяча |
  | `1 USD` in Ukrainian | **Одна** доллар | **Один** долар |
  | `2.02 BGN` | два лева и **два** стотинки | два лева и **две** стотинки |

  Gender moved onto the currency model, so the main unit and the sub unit can differ —
  рубль is masculine while копейка is feminine.

- **Ukrainian**: bare numerals and millions were feminine; its word table was
  feminine-first and millions took the path thousands take elsewhere.

- **Bulgarian**: `1` rendered as an empty string, millions lost their count entirely
  (`милиона` rather than `един милион`), and `милион` was treated as feminine.

- **Ukrainian currency wording.** Most of the table had been copied from Russian, so it
  produced Russian words: `доллар`, `Нуль центов`, `турецкая лира`. The Polish sub unit
  read `грубий`, which means "coarse". Corrected throughout, with the three agreement forms
  Ukrainian requires.

- **`5 ETB` and above threw `IndexOutOfRangeException` in Ukrainian.** The birr had two
  name forms where the converter indexes three.

### Fixed — Spanish

- **10^9 was rendered as "billón", which means 10^12** — out by a factor of a thousand. Per
  RAE it is now `mil millones`. The library produced this correctly until a 2016 refactor.
- **"uno" is apocopated in front of a noun or scale word**: `uno mil` → `mil`,
  `uno millón` → `un millón`, `uno euro` → `un euro`. Standing alone it keeps its full form.
- **Hundreds above 100 no longer switch to their feminine form**, so `999` reads as
  `novecientos noventa y nueve`.
- **Scale nouns link to the currency with "de"**: `un millón de euros`. `mil` is an
  adjective and takes none.

### Fixed — other languages

- **German** writes numbers below a million as one closed-up word, as Duden requires, and
  separates them only from a million upwards: `ein hundert` → `einhundert`,
  `einundvierzig tausend` → `einundvierzigtausend`. Standing alone the numeral is `eins`;
  `ein` is kept before a noun.
- **British English now says "and"**: `en-GB` was resolving to the same converter as
  `en-US`, so it produced American wording. `101` reads as `one hundred and one` in
  `en-GB` and `one hundred one` in `en-US`. On a cheque the written amount is the one the
  bank honours, so the two conventions are kept apart.
- **English** hyphenates compounds: `twenty one` → `twenty-one`. Only the tens-and-units
  pair, so `121` is `one hundred twenty-one`.
- **French** hyphenates compounds below a hundred (`quarante-deux`), agrees multiplied
  `cent` when it ends the number (`deux cents`, but `deux cent mille`), and pluralises
  `million`/`milliard`. Also fixes a stray trailing space that produced `cinquante -deux`.
- **Portuguese** joins the two parts of an amount with `e` rather than `com`
  ([#27](https://github.com/emrahyumuk/NUT-number-to-text/pull/27)).

- **Capitalisation no longer lands on the wrong word for a negative amount.**
  `MainUnitFirstCharUpper` capitalised the main unit, which stopped being the first word
  once the sign was added: `minus Forty-one dollars`. The sign takes the capital now —
  `Minus forty-one dollars`. This matters on a cheque, where the amount in words is the
  field the bank pays against.

### Changed

- **Numbers past the supported range throw `ArgumentOutOfRangeException`** instead of a bare
  `Exception`, so callers can catch it selectively. The message states the range.

- **Neuter gender.** `GenderGroup` had only `None`, `Feminine` and `Masculine`, so a
  neuter currency name fell through to the masculine numeral: `един евро`, `один песо`.
  Bulgarian marks all three (`един` / `една` / `едно`), and евро and пени are neuter;
  песо is neuter in Russian, Ukrainian and Belarusian.

  The Bulgarian name for GBP is corrected to `британска лира`, which is what the Bulgarian
  National Bank uses.

### Added

- `GenderGroup.Neuter`, appended so existing values keep their numbers.

- **GBP**, in every language except Amharic
  ([#24](https://github.com/emrahyumuk/NUT-number-to-text/pull/24)). The pound is feminine
  in French, Spanish and Portuguese, and none of those three had any gender handling, so
  it is added here — in French the distinction carries meaning, since *le livre* is a book
  and *la livre* the currency.

- **Uzbek (Latin script)** and the Uzbek som (`UZS`), from
  [#23](https://github.com/emrahyumuk/NUT-number-to-text/pull/23). The number system builds
  from twenty-two basic words and behaves like Turkish: no "bir" before *yuz* or *ming*.

- `Options.SubUnitTruncated`, for callers who need extra decimals dropped rather than
  carried: `1.999` reads as "one dollar ninety-nine cents". Rounding remains the default.
- `Currency.RUR` as an alias for `Currency.RUB`, mirroring how `tl` maps to `try`.

## [3.5.0] - 2026-07-28

Output is byte-for-byte identical to 3.4.1 in all 12 languages. The fix below is about
correctness under concurrency and repeated calls, not wording.

### Fixed

- **Converters no longer share mutable state.** The Russian, Ukrainian, Belarusian and
  Bulgarian converters wrote gendered word forms into their singleton's shared word table
  before every conversion and never restored it. Two consequences, both fixed:
  - `ToText(long)` returned whatever the previous conversion had left behind, so the same
    input could produce different output depending on call order.
  - Concurrent conversions overwrote each other. Measured 17 938 of 50 000 parallel
    conversions producing wrong text (for example `одна рубль`, a feminine numeral with a
    masculine noun) before the fix, and none after.

  Bulgarian additionally kept a mutable `textType` field with the same race.

### Added

- Continuous integration: every push and pull request builds and runs the tests.
- Test suite grown from 26 to 237 tests, covering plain-number output in all 12 languages,
  the defects that currently reproduce, and converter isolation under parallel load.
- Releases are now published from version tags, so the package version comes from the tag
  rather than being edited by hand in three places.

### Removed

- `Nut.Demo`, a WinForms project targeting the out-of-support .NET Core 3.1. It was not
  part of the package; usage examples live in the README.

## [3.4.1] - 2023-06-02

### Added

- German language and currency support, contributed by
  [Maryam1986](https://github.com/Maryam1986).

## [3.3.0] - 2022-05-24

### Added

- Portuguese language and BRL currency, contributed by
  [Marciel032](https://github.com/Marciel032).

## [3.2.4] - 2021-09-27

### Fixed

- French translation of "one hundred".
- A merge defect in `BaseConverter`.

## [3.2.3] - 2021-09-19

### Added

- ARS (Argentine peso).

### Fixed

- Spanish and Amharic converters.

### Removed

- The `AddAndBetweenMainUnitAndSubUnits` and `UseShortenedUnits` options.

## [3.1.0] - 2021-01-27

### Added

- Belarusian language and BYN currency, contributed by
  [DeNcHiK3713](https://github.com/DeNcHiK3713).
- Amharic language, contributed by [ashGHub](https://github.com/ashGHub).
- Polish language and PLN currency, contributed by
  [kashiash](https://github.com/kashiash).

### Fixed

- Russian converter endings.

## [3.0.0] - 2020-07-03

- **Capitalisation no longer lands on the wrong word for a negative amount.**
  `MainUnitFirstCharUpper` capitalised the main unit, which stopped being the first word
  once the sign was added: `minus Forty-one dollars`. The sign takes the capital now —
  `Minus forty-one dollars`. This matters on a cheque, where the amount in words is the
  field the bank pays against.

### Changed

- Retargeted to .NET Standard 2.0, contributed by
  [ashGHub](https://github.com/ashGHub).

### Added

- ETB (Ethiopian birr).

## [2.4.2] - 2016-05-27

### Fixed

- French language and sub-unit errors.

## [2.4.1] - 2016-01-15

### Fixed

- Single-digit sub-unit and single-digit child-number conversions.

## [2.3.3] - 2015-04-14

### Fixed

- Sub-unit handling when the number is zero.

## [2.3.0] - 2015-04-06

### Added

- Ukrainian language and UAH currency, contributed by
  [Latif Turk](https://github.com/Latif07).

### Fixed

- Russian `два` / `две` gender agreement across all supported currencies.

## [2.0.0] - 2014-04-01

### Added

- Money-to-text conversion alongside number-to-text.

## [1.0.0] - 2014-03-31

Initial release, with English, Spanish and Turkish.

<!-- Bulgarian (2017) shipped between 2.4.2 and 3.0.0; the exact version is not
     recoverable from the available history. -->

[Unreleased]: https://github.com/emrahyumuk/NUT-number-to-text/compare/v3.5.0...HEAD
[3.5.0]: https://github.com/emrahyumuk/NUT-number-to-text/releases/tag/v3.5.0
[3.4.1]: https://www.nuget.org/packages/Nut/3.4.1
[3.3.0]: https://www.nuget.org/packages/Nut/3.3.0
[3.2.4]: https://www.nuget.org/packages/Nut/3.2.4
[3.2.3]: https://www.nuget.org/packages/Nut/3.2.3
[3.1.0]: https://www.nuget.org/packages/Nut/3.1.0
[3.0.0]: https://www.nuget.org/packages/Nut/3.0.0
[2.4.2]: https://www.nuget.org/packages/Nut/2.4.2
[2.4.1]: https://www.nuget.org/packages/Nut/2.4.1
[2.3.3]: https://www.nuget.org/packages/Nut/2.3.3
[2.3.0]: https://www.nuget.org/packages/Nut/2.3.0
[2.0.0]: https://www.nuget.org/packages/Nut/2.0.0
[1.0.0]: https://www.nuget.org/packages/Nut/1.0.0
