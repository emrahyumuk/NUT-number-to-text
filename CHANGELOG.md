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
