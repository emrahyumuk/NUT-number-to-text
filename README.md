# NUT - Number To Text

[![NuGet](https://img.shields.io/nuget/v/Nut.svg)](https://www.nuget.org/packages/Nut/)
[![Downloads](https://img.shields.io/nuget/dt/Nut.svg)](https://www.nuget.org/packages/Nut/)
[![CI](https://github.com/emrahyumuk/NUT-number-to-text/actions/workflows/ci.yml/badge.svg)](https://github.com/emrahyumuk/NUT-number-to-text/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENCE)

---

Number To Text Converter

Money To Text Converter

**Supported Languages:** English, French, German, Russian, Spanish, Turkish, Ukrainian, Bulgarian, Amharic, Polish, Belarussian, Portuguese, Uzbek.

**Supported Currencies:** EUR, USD, GBP, RUB, TRY, UAH, BGN, ETB, PLN, BYN, ARS, BRL, UZS.

**Number Limit:** 1 trillion

**Rounding:** amounts with more decimals than the currency has are rounded to the sub-unit,
half away from zero — the same result `decimal.ToString("C")` gives. Set
`Options.SubUnitTruncated` to cut the extra digits instead.

**Target Framework:** .NET Standard 2.0 — runs on .NET Framework 4.6.1+, .NET Core 2.0+ and .NET 5 and later.

---

**INSTALL**

```
dotnet add package Nut
```

---

**USAGE - Number To Text**

```csharp

    var number = 123456
    var text = number.ToText("en");

    var number = 123456;
    var text = number.ToText(Language.English);
```

---

**USAGE - Money To Text**

```csharp

    var number = 123456.78
    var moneyText = number.ToText("usd", "en");

    var number = 123456.78;
    var moneyText = number.ToText(Nut.Currency.USD, Nut.Language.English);

    var number = 123456.78;
    var options = new Nut.Options {
        MainUnitNotConvertedToText = true,
        SubUnitNotConvertedToText = true,
        SubUnitZeroNotDisplayed = true,
        MainUnitFirstCharUpper = true,
        SubUnitFirstCharUpper = true,
        CurrencyFirstCharUpper = true,
        SubUnitTruncated = true
    }
    var moneyText = number.ToText(Nut.Currency.USD, Nut.Language.English, options);
```

---

**NUGET**

<https://www.nuget.org/packages/Nut/>

---

**CHANGELOG**

See [CHANGELOG.md](CHANGELOG.md) for what changed in each release.

---

**CONTRIBUTING**

Pull requests are welcome. Two things make them much easier to accept:

- **Add tests for what you change.** `dotnet test src/Nut.sln` runs the suite, and CI runs
  it on every pull request.
- **A new language or currency needs tests written by someone who speaks it.** Reviewers
  cannot verify wording in a language they do not read, so a table of expected outputs is
  the only thing that makes such a contribution reviewable. Cover at least `0, 1, 2, 11,
  21, 100, 1000, 2000, 41000, 1000000` plus one decimal amount.

---

**THANKS**

- [Latif Turk](https://github.com/Latif07) - Ukrainian Language and Currency
- [SecreT2k8](https://github.com/SecreT2k8) - Bulgarian Language and Currency
- [ashGHub](https://github.com/ashGHub) - .Net Standart Migration, Ethiopian Language and Currency
- [kashiash](https://github.com/kashiash) - Polish Language and Currency
- [DeNcHiK3713](https://github.com/DeNcHiK3713) - Belarussian Language and Currency
- [Marciel032](https://github.com/Marciel032) - Portuguese Language and Currency
- [ArkadiuszMakosa](https://github.com/ArkadiuszMakosa) - Polish Language Unit Tests
- [Maryam1986](https://github.com/Maryam1986) - German Language and Currency
- [Furqat-Abduvosiqov](https://github.com/Furqat-Abduvosiqov) - Uzbek Language and Currency
- [IlyashenkoA](https://github.com/IlyashenkoA) - GBP Currency

---

**LICENCE**

NUT-number-to-text is [MIT licensed.](LICENCE)

---
