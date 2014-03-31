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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cmbLang.DataSource = new List<string>() { "en", "es", "ru", "tr" };
        }

        private void btnNumberToText_Click(object sender, EventArgs e)
        {
            var lang = cmbLang.SelectedValue.ToString();
            var text = Convert.ToInt64(txtNumber.Text).ToText(lang);
            lblNumberToWord.Text = text;
        }
    }
}
