using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nut.Constants;

namespace Nut.Demo {
    public partial class Form2 : Form 
    {
        public Form2() 
        {
            InitializeComponent();
            cmbLang.DataSource = new List<string>() { "en", "es","fr", "ru", "tr" };
            cmbCurrency.DataSource = new List<string>() { "usd", "eur", "rub", "try" };
        }

        private void btnMoneyToText_Click(object sender, EventArgs e) 
        {
            var lang = cmbLang.SelectedValue.ToString();
            var currency = cmbCurrency.SelectedValue.ToString();
            var text = Convert.ToDecimal(txtNumber.Text).ToText(currency,lang);
            txtResultText.Text = text;
        }
    }
}
