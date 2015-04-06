using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nut.Demo
{
    public partial class NumberToText : Form
    {
        public NumberToText()
        {
            InitializeComponent();

            cmbLang.DataSource = new List<string>() { "en", "es", "fr", "ru", "tr" };
        }

        private void btnNumberToText_Click(object sender, EventArgs e)
        {
            var lang = cmbLang.SelectedValue.ToString();
            var text = Convert.ToInt32(txtNumber.Text).ToText(lang);
            txtResultText.Text = text;
        }
    }
}
