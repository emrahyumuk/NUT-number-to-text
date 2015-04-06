using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Nut.Models;

namespace Nut.Demo {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
            cmbLang.DataSource = new List<string>() { "en", "es", "fr", "ru", "tr" };
            cmbCurrency.DataSource = new List<string>() { "usd", "eur", "rub", "try" };
        }

        private void btnMoneyToText_Click(object sender, EventArgs e) {
            var lang = cmbLang.SelectedValue.ToString();
            var currency = cmbCurrency.SelectedValue.ToString();
            var configuration = new Configuration {
                FractionZeroNotIncluded = cbFractionZeroNotIncluded.Checked,
                FractionNotConvertToText = cbFractionNotConvertToText.Checked,
                FirstCharUpper = cbFirstCharUpper.Checked,
                CurrencyFirstCharUpper = cbCurrencyFirstCharUpper.Checked
            };
            var text = Convert.ToDecimal(txtNumber.Text).ToText(currency, lang, configuration);
            txtResultText.Text = text;
        }
    }
}
