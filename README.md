# NUT — Number To Text

[![NuGet](https://img.shields.io/nuget/v/Nut.svg)](https://www.nuget.org/packages/Nut/)
[![Downloads](https://img.shields.io/nuget/dt/Nut.svg)](https://www.nuget.org/packages/Nut/)
[![CI](https://github.com/emrahyumuk/NUT-number-to-text/actions/workflows/ci.yml/badge.svg)](https://github.com/emrahyumuk/NUT-number-to-text/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENCE)

Writes numbers and money amounts as words, in 14 languages — for the *amount in words*
field on invoices, cheques and contracts.

```csharp
using Nut;

123456.ToText();                                    // one hundred twenty-three thousand four hundred fifty-six
123456.78m.ToText(Currency.USD, Language.English);  // one hundred twenty-three thousand four hundred fifty-six dollars seventy-eight cents
123456.78m.ToText(Currency.TRY, Language.Turkish);  // yüz yirmi üç bin dört yüz elli altı türk lirası yetmiş sekiz kuruş
```

## Install

```
dotnet add package Nut
```

Targets .NET Standard 2.0, so it runs on .NET Framework 4.6.1+, .NET Core 2.0+ and .NET 5
and later. No dependencies.

## Languages

Pass either the short code or the culture code. Matching ignores case.

| Language | Codes | | Language | Codes |
|---|---|---|---|---|
| English (US) | `en`, `en-US` | | Latvian | `lv`, `lv-LV` |
| English (UK) | `en-GB` | | Polish | `pl`, `pl-PL` |
| French | `fr`, `fr-FR` | | Portuguese | `pt`, `pt-BR` |
| German | `de`, `de-DE` | | Russian | `ru`, `ru-RU` |
| Spanish | `es`, `es-ES` | | Turkish | `tr`, `tr-TR` |
| Amharic | `am`, `am-ET` | | Ukrainian | `uk`, `uk-UA` |
| Belarusian | `be`, `be-BY` | | Uzbek | `uz`, `uz-UZ` |
| Bulgarian | `bg`, `bg-BG` | | | |

`en-GB` differs from `en-US`: `101` reads as *one hundred **and** one* rather than *one
hundred one*.

Ukrainian and Belarusian used to be keyed by their ccTLDs, `ua` and `by`. Those still
resolve, but the codes above are the ones `CultureInfo` hands you.

`Extensions.SupportedLanguages` returns the full list at runtime.

## Currencies

`EUR` `USD` `GBP` `RUB` `TRY` `UAH` `BGN` `ETB` `PLN` `BYN` `ARS` `BRL` `UZS`

Also accepted: `TL` for `TRY`, `RUR` for `RUB`.

Every language covers every currency, except Amharic, which is missing `ARS`, `BRL`, `GBP`
and `UZS`.

```csharp
2575.50m.ToText(Currency.EUR, Language.German);   // zweitausendfünfhundertfünfundsiebzig Euro fünfzig Cent
2575.50m.ToText(Currency.EUR, Language.French);   // deux mille cinq cent soixante-quinze euros cinquante centimes
2575.50m.ToText(Currency.EUR, Language.Russian);  // две тысячи пятьсот семьдесят пять евро пятьдесят евроцентов
2575.50m.ToText(Currency.EUR, Language.Latvian);  // divi tūkstoši pieci simti septiņdesmit pieci eiro un piecdesmit centi
```

## Options

```csharp
var options = new Options { MainUnitFirstCharUpper = true, CurrencyFirstCharUpper = true };
2575.50m.ToText(Currency.USD, Language.English, options);
// Two thousand five hundred seventy-five Dollars fifty Cents
```

| Option | Effect |
|---|---|
| `MainUnitFirstCharUpper` | Capitalises the first word. On a negative amount the capital goes on the sign: *Minus two thousand…* |
| `SubUnitFirstCharUpper` | Capitalises the fractional part |
| `CurrencyFirstCharUpper` | Capitalises the currency names |
| `MainUnitNotConvertedToText` | Leaves the whole part as digits |
| `SubUnitZeroNotDisplayed` | Drops the fractional part when it is zero |
| `SubUnitFormat` | `Words` (default), `Digits`, or `Fraction` — see below |
| `SubUnitTruncated` | Cuts extra decimals instead of rounding them |

### Writing the fraction the way cheques do

```csharp
new Options { SubUnitFormat = SubUnitFormat.Fraction }
// two thousand five hundred seventy-five dollars and 50/100
```

Zero is written as `00/100` rather than dropped, so nothing can be added to the amount
afterwards. Languages that join the two parts with a word keep it: *con 50/100* in Spanish,
*un 50/100* in Latvian.

Writing the sub-unit over a hundred is an anglophone cheque convention rather than a rule
of any language, so it is only available where there is a word to join the two parts with:
English, Spanish, Portuguese, Bulgarian, Latvian and Amharic. Asking for it in one of the
other eight raises `NotSupportedException` instead of borrowing the English *and*.

## Behaviour worth knowing

**Rounding.** Amounts carrying more decimals than the currency has are rounded to the
sub-unit, half away from zero — the same result `decimal.ToString("C")` gives, so the
figures and the words on a document agree.

```csharp
123.456m.ToText(Currency.USD, Language.English);  // one hundred twenty-three dollars forty-six cents
```

Set `SubUnitTruncated` to cut the digits instead.

**Negative amounts** use the language's own word — *minus*, *moins*, *menos*, *eksi*,
*минус*, *mīnus*, *ሲቀነስ*.

```csharp
(-2575.50m).ToText(Currency.USD, Language.English);  // minus two thousand five hundred seventy-five dollars fifty cents
```

**Unsupported input throws.** An unknown language, or a currency a language does not cover,
raises `NotSupportedException` naming what was asked for. It does not return an empty
string: a blank amount field on a document is not noticed until the document has gone out.

**Ukrainian paper payment instructions** require the amount in words to start with a
capital letter — NBU Board Resolution 29.07.2022 №163, Annex, field 4. Pass
`MainUnitFirstCharUpper` when printing one. The same resolution says the field is left
empty on electronic instructions.

```csharp
var options = new Options { MainUnitFirstCharUpper = true };
105.50m.ToText(Currency.UAH, Language.Ukrainian, options);
// Сто п'ять гривень п'ятдесят копійок
```

**Range.** Magnitudes below one trillion. Beyond that raises `ArgumentOutOfRangeException`.
Rounding counts: an amount that crosses the limit only after being rounded to the sub-unit
raises it too.

**Thread safety.** Conversions are independent and safe to run in parallel.

## Contributing

Pull requests are welcome. Two things make them much easier to accept:

- **Add tests for what you change.** `dotnet test src/Nut.sln` runs the suite; CI runs it
  on every pull request and fails on warnings.
- **A new language or currency needs expected outputs written by someone who reads it.**
  Nobody here can check wording in a language they do not speak, and this text ends up on
  financial documents. A table of expected results is what makes such a contribution
  reviewable. Cover at least `0, 1, 2, 11, 21, 100, 1000, 2000, 41000, 1000000` plus one
  decimal amount, and say where the forms come from.

`src/Nut.Tests/behaviour-snapshot.tsv` pins every language × currency × amount
combination. If a change is meant to alter output, regenerate it with
`UPDATE_SNAPSHOT=1 dotnet test` — the diff of that file becomes the record of what changed.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

## Thanks

- [Latif Turk](https://github.com/Latif07) — Ukrainian language and currency
- [SecreT2k8](https://github.com/SecreT2k8) — Bulgarian language and currency
- [ashGHub](https://github.com/ashGHub) — .NET Standard migration, Ethiopian language and currency
- [kashiash](https://github.com/kashiash) — Polish language and currency
- [DeNcHiK3713](https://github.com/DeNcHiK3713) — Belarusian language and currency
- [Marciel032](https://github.com/Marciel032) — Portuguese language and currency
- [ArkadiuszMakosa](https://github.com/ArkadiuszMakosa) — Polish unit tests
- [Maryam1986](https://github.com/Maryam1986) — German language and currency, negative numbers
- [Furqat-Abduvosiqov](https://github.com/Furqat-Abduvosiqov) — Uzbek language and currency
- [IlyashenkoA](https://github.com/IlyashenkoA) — GBP currency, Latvian language
- [Stepami](https://github.com/Stepami) — Russian gender fixes, RUR
- [fulviocanducci](https://github.com/fulviocanducci) — Portuguese separator

## Licence

MIT — see [LICENCE](LICENCE).
