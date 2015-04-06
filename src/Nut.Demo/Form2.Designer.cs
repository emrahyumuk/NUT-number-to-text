namespace Nut.Demo {
    partial class Form2 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnMoneyToText = new System.Windows.Forms.Button();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.cmbCurrency = new System.Windows.Forms.ComboBox();
            this.txtResultText = new System.Windows.Forms.TextBox();
            this.cbFractionZeroNotIncluded = new System.Windows.Forms.CheckBox();
            this.cbFractionNotConvertToText = new System.Windows.Forms.CheckBox();
            this.cbFirstCharUpper = new System.Windows.Forms.CheckBox();
            this.cbCurrencyFirstCharUpper = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnMoneyToText
            // 
            this.btnMoneyToText.Location = new System.Drawing.Point(416, 31);
            this.btnMoneyToText.Name = "btnMoneyToText";
            this.btnMoneyToText.Size = new System.Drawing.Size(102, 23);
            this.btnMoneyToText.TabIndex = 9;
            this.btnMoneyToText.Text = "Convert To Text";
            this.btnMoneyToText.UseVisualStyleBackColor = true;
            this.btnMoneyToText.Click += new System.EventHandler(this.btnMoneyToText_Click);
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(145, 34);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(145, 20);
            this.txtNumber.TabIndex = 8;
            // 
            // cmbLang
            // 
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(30, 33);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(96, 21);
            this.cmbLang.TabIndex = 7;
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.FormattingEnabled = true;
            this.cmbCurrency.Location = new System.Drawing.Point(308, 33);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(96, 21);
            this.cmbCurrency.TabIndex = 11;
            // 
            // txtResultText
            // 
            this.txtResultText.Location = new System.Drawing.Point(30, 147);
            this.txtResultText.Multiline = true;
            this.txtResultText.Name = "txtResultText";
            this.txtResultText.Size = new System.Drawing.Size(488, 75);
            this.txtResultText.TabIndex = 12;
            // 
            // cbFractionZeroNotIncluded
            // 
            this.cbFractionZeroNotIncluded.AutoSize = true;
            this.cbFractionZeroNotIncluded.Location = new System.Drawing.Point(30, 70);
            this.cbFractionZeroNotIncluded.Name = "cbFractionZeroNotIncluded";
            this.cbFractionZeroNotIncluded.Size = new System.Drawing.Size(153, 17);
            this.cbFractionZeroNotIncluded.TabIndex = 13;
            this.cbFractionZeroNotIncluded.Text = "Fraction Zero Not Included";
            this.cbFractionZeroNotIncluded.UseVisualStyleBackColor = true;
            // 
            // cbFractionNotConvertToText
            // 
            this.cbFractionNotConvertToText.AutoSize = true;
            this.cbFractionNotConvertToText.Location = new System.Drawing.Point(217, 70);
            this.cbFractionNotConvertToText.Name = "cbFractionNotConvertToText";
            this.cbFractionNotConvertToText.Size = new System.Drawing.Size(164, 17);
            this.cbFractionNotConvertToText.TabIndex = 14;
            this.cbFractionNotConvertToText.Text = "Fraction Not Convert To Text";
            this.cbFractionNotConvertToText.UseVisualStyleBackColor = true;
            // 
            // cbFirstCharUpper
            // 
            this.cbFirstCharUpper.AutoSize = true;
            this.cbFirstCharUpper.Location = new System.Drawing.Point(30, 107);
            this.cbFirstCharUpper.Name = "cbFirstCharUpper";
            this.cbFirstCharUpper.Size = new System.Drawing.Size(102, 17);
            this.cbFirstCharUpper.TabIndex = 13;
            this.cbFirstCharUpper.Text = "First Char Upper";
            this.cbFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbCurrencyFirstCharUpper
            // 
            this.cbCurrencyFirstCharUpper.AutoSize = true;
            this.cbCurrencyFirstCharUpper.Location = new System.Drawing.Point(217, 107);
            this.cbCurrencyFirstCharUpper.Name = "cbCurrencyFirstCharUpper";
            this.cbCurrencyFirstCharUpper.Size = new System.Drawing.Size(147, 17);
            this.cbCurrencyFirstCharUpper.TabIndex = 14;
            this.cbCurrencyFirstCharUpper.Text = "Currency First Char Upper";
            this.cbCurrencyFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 251);
            this.Controls.Add(this.cbCurrencyFirstCharUpper);
            this.Controls.Add(this.cbFirstCharUpper);
            this.Controls.Add(this.cbFractionNotConvertToText);
            this.Controls.Add(this.cbFractionZeroNotIncluded);
            this.Controls.Add(this.txtResultText);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.btnMoneyToText);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.cmbLang);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMoneyToText;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.ComboBox cmbCurrency;
        private System.Windows.Forms.TextBox txtResultText;
        private System.Windows.Forms.CheckBox cbFractionZeroNotIncluded;
        private System.Windows.Forms.CheckBox cbFractionNotConvertToText;
        private System.Windows.Forms.CheckBox cbFirstCharUpper;
        private System.Windows.Forms.CheckBox cbCurrencyFirstCharUpper;
    }
}