# Changelog

All notable changes to this project are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this
project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Entries before 3.5.0 were reconstructed from git history and the
[NuGet release dates](https://www.nuget.org/packages/Nut/#versions-body-tab); the original
releases shipped without release notes, so those entries record the main change of each
version rather than a complete list.

## [Unreleased]

Work in progress on the linguistic defects recorded in `Nut.Tests/KnownDefects.cs`. These
change the produced text, so they are being collected for 4.0.0 rather than a minor release.

### Fixed

- **Russian and Belarusian: the count now agrees with the scale word rather than the
  currency** ([#25](https://github.com/emrahyumuk/NUT-number-to-text/issues/25)).
  `тысяча` is feminine and `миллион` is masculine, and both can occur in one amount.

  | Amount | Before | After |
  | --- | --- | --- |
  | `41000 RUB` | сорок **один** тысяча рублей | сорок **одна** тысяча рублей |
  | `42000 RUB` | сорок **два** тысячи рублей | сорок **две** тысячи рублей |
  | `1000000 UAH` in Russian | **одна** миллион гривень | **один** миллион гривень |
  | `41000 BYN` | сорак **адзін** тысяча | сорак **адна** тысяча |

  353 of 4536 checked conversions change across the four languages; the other eight are
  untouched.

- **Numerals now agree with the gender of the unit being counted**, in Russian,
  Ukrainian, Belarusian and Bulgarian. Gender moved onto the currency model, so the main
  unit and the sub unit can differ:

  | Amount | Before | After |
  | --- | --- | --- |
  | `1 TRY` in Russian | **один** турецкая лира | **одна** турецкая лира |
  | `1 USD` in Ukrainian | **Одна** доллар | **Один** доллар |
  | `0.01 EUR` in Russian | **одна** евроцент | **один** евроцент |
  | `2.02 BGN` | два лева и **два** стотинки | два лева и **две** стотинки |

- **Ukrainian**: bare numerals and millions were feminine (`Одна`, `Одна мільйон`); the
  word table was feminine-first and millions took the path thousands take elsewhere.
- **Bulgarian**: `1` rendered as an empty string, millions lost their count entirely
  (`милиона` rather than `един милион`), and `милион` was treated as feminine.

- **Ukrainian currency wording.** Most of the table had been copied from Russian, so it
  produced Russian words in Ukrainian output: `доллар`, `Нуль центов`, `турецкая лира`.
  The Polish sub unit read `грубий`, which means "coarse". Corrected throughout, with the
  three agreement forms Ukrainian requires (1 / 2-4 / 5+).

- **`5 ETB` and above threw `IndexOutOfRangeException` in Ukrainian.** The birr had two
  name forms where the converter indexes three. It now has all three, and its sub unit is
  the cent rather than the kopiyka.

- **Negative amounts are converted instead of vanishing**
  ([#22](https://github.com/emrahyumuk/NUT-number-to-text/pull/22)). Every `Append` helper
  is guarded by `num > x`, so a negative number matched none of them:

  | Input | Before | After |
  | --- | --- | --- |
  | `(-41).ToText("en")` | `""` | `minus forty one` |
  | `(-41.5m)` USD | `dollar fifty cents` | `minus forty one dollars fifty cents` |

  The money case was the dangerous one: the integer part disappeared while the fraction
  survived, producing a plausible-looking but wrong amount rather than an obvious failure.

  The sign word is per language — minus, moins, menos, eksi, минус, мінус, ሲቀነስ — taken
  from the list in #22.

  The one-trillion limit now applies on both sides. Previously an amount below
  -1 000 000 000 000 returned nonsense (`dollar zero cent`) where the positive equivalent
  threw; both now throw.

- **Amounts carrying more decimals than the currency has are now rounded**, rather than
  read as whole sub-units. `123.456` USD produced "four hundred fifty six cents"; it now
  produces "forty six cents". Rounding is half away from zero and carries into the main
  unit, so `1.999` reads as two dollars — matching what `decimal.ToString("C")` renders
  for the same value, so a document showing both the figure and the words stays
  consistent.

  This also fixes `1.100` and `1.10` disagreeing: `decimal` preserves the scale it was
  written with, and the old code read it directly.

- **All thirteen `Culture` constants were dead on the `int` overloads.** Those overloads
  lowercased the argument before comparing it against constants like `"en-US"`, so no case
  ever matched and the caller silently got `""`. The same string worked on the `long` and
  `decimal` overloads, so whether a culture code was accepted depended on the numeric type
  it was called on.

- **Language and currency matching now ignores case.** `"EN"`, `"en-US"`, `"USD"` and
  `"TL"` resolve like their lower-case forms; previously an upper-case currency code
  returned `""`.

### Added

- `Options.SubUnitTruncated`, for callers who need extra decimals dropped rather than
  carried: `1.999` reads as "one dollar ninety nine cents". Rounding remains the default.

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
