using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nut.Demo
{
    public partial class NumberToText : Form
    {
        public NumberToText()
        {
            InitializeComponent();
            txtNumber.Text = "1234567";
            cmbLang.DataSource = new List<string>() { "pl","en", "es", "fr", "ru", "tr", "ua", "bg", "am" };
        }

        private void btnNumberToText_Click(object sender, EventArgs e)
        {
            var lang = cmbLang.SelectedValue.ToString();
            var text = Convert.ToDecimal(txtNumber.Text).ToText("", lang);
            txtResultText.Text = text;
        }
    }
}
