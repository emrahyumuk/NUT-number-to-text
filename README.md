# NUT Extended - Number To Text
Package based on the Nut package - https://github.com/emrahyumuk/NUT-number-to-text
---

Number To Text Converter

Money To Text Converter

**Supported Languages:** English, French, Russian, Spanish, Turkish, Ukrainian, Bulgarian, Amharic, Polish, Belarussian, Portuguese.

**Supported Currencies:** EUR, USD, RUB, TRY, UAH, BGN, ETB, PLN, BYN, ARS, BRL, GBP.

**Number Limit:** 1 trillion

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
        CurrencyFirstCharUpper = true
    }
    var moneyText = number.ToText(Nut.Currency.USD, Nut.Language.English, options);
```

---

**NUGET**

<https://www.nuget.org/packages/NutExtended/>

---

**LICENCE**

NUT-number-to-text is [MIT licensed.](LICENSE)

---
