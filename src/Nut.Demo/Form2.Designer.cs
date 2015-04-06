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
            this.cbSubUnitZeroNotIncluded = new System.Windows.Forms.CheckBox();
            this.cbSubUnitNotConvertToText = new System.Windows.Forms.CheckBox();
            this.cbMainUnitFirstCharUpper = new System.Windows.Forms.CheckBox();
            this.cbCurrencyFirstCharUpper = new System.Windows.Forms.CheckBox();
            this.cbMainUnitNotConvertedToText = new System.Windows.Forms.CheckBox();
            this.cbSubUnitFirstCharUpper = new System.Windows.Forms.CheckBox();
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
            this.txtResultText.Location = new System.Drawing.Point(30, 183);
            this.txtResultText.Multiline = true;
            this.txtResultText.Name = "txtResultText";
            this.txtResultText.Size = new System.Drawing.Size(488, 75);
            this.txtResultText.TabIndex = 12;
            // 
            // cbSubUnitZeroNotIncluded
            // 
            this.cbSubUnitZeroNotIncluded.AutoSize = true;
            this.cbSubUnitZeroNotIncluded.Location = new System.Drawing.Point(217, 143);
            this.cbSubUnitZeroNotIncluded.Name = "cbSubUnitZeroNotIncluded";
            this.cbSubUnitZeroNotIncluded.Size = new System.Drawing.Size(159, 17);
            this.cbSubUnitZeroNotIncluded.TabIndex = 13;
            this.cbSubUnitZeroNotIncluded.Text = "Sub-unit Zero Not Displayed";
            this.cbSubUnitZeroNotIncluded.UseVisualStyleBackColor = true;
            // 
            // cbSubUnitNotConvertToText
            // 
            this.cbSubUnitNotConvertToText.AutoSize = true;
            this.cbSubUnitNotConvertToText.Location = new System.Drawing.Point(217, 70);
            this.cbSubUnitNotConvertToText.Name = "cbSubUnitNotConvertToText";
            this.cbSubUnitNotConvertToText.Size = new System.Drawing.Size(165, 17);
            this.cbSubUnitNotConvertToText.TabIndex = 14;
            this.cbSubUnitNotConvertToText.Text = "Sub-unit Not Convert To Text";
            this.cbSubUnitNotConvertToText.UseVisualStyleBackColor = true;
            // 
            // cbMainUnitFirstCharUpper
            // 
            this.cbMainUnitFirstCharUpper.AutoSize = true;
            this.cbMainUnitFirstCharUpper.Location = new System.Drawing.Point(30, 107);
            this.cbMainUnitFirstCharUpper.Name = "cbMainUnitFirstCharUpper";
            this.cbMainUnitFirstCharUpper.Size = new System.Drawing.Size(150, 17);
            this.cbMainUnitFirstCharUpper.TabIndex = 13;
            this.cbMainUnitFirstCharUpper.Text = "Main Unit First Char Upper";
            this.cbMainUnitFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbCurrencyFirstCharUpper
            // 
            this.cbCurrencyFirstCharUpper.AutoSize = true;
            this.cbCurrencyFirstCharUpper.Location = new System.Drawing.Point(30, 143);
            this.cbCurrencyFirstCharUpper.Name = "cbCurrencyFirstCharUpper";
            this.cbCurrencyFirstCharUpper.Size = new System.Drawing.Size(147, 17);
            this.cbCurrencyFirstCharUpper.TabIndex = 14;
            this.cbCurrencyFirstCharUpper.Text = "Currency First Char Upper";
            this.cbCurrencyFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbMainUnitNotConvertedToText
            // 
            this.cbMainUnitNotConvertedToText.AutoSize = true;
            this.cbMainUnitNotConvertedToText.Location = new System.Drawing.Point(30, 70);
            this.cbMainUnitNotConvertedToText.Name = "cbMainUnitNotConvertedToText";
            this.cbMainUnitNotConvertedToText.Size = new System.Drawing.Size(183, 17);
            this.cbMainUnitNotConvertedToText.TabIndex = 15;
            this.cbMainUnitNotConvertedToText.Text = "Main Unit Not Converted To Text";
            this.cbMainUnitNotConvertedToText.UseVisualStyleBackColor = true;
            // 
            // cbSubUnitFirstCharUpper
            // 
            this.cbSubUnitFirstCharUpper.AutoSize = true;
            this.cbSubUnitFirstCharUpper.Location = new System.Drawing.Point(217, 107);
            this.cbSubUnitFirstCharUpper.Name = "cbSubUnitFirstCharUpper";
            this.cbSubUnitFirstCharUpper.Size = new System.Drawing.Size(144, 17);
            this.cbSubUnitFirstCharUpper.TabIndex = 16;
            this.cbSubUnitFirstCharUpper.Text = "Sub-unit First Char Upper";
            this.cbSubUnitFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 293);
            this.Controls.Add(this.cbSubUnitFirstCharUpper);
            this.Controls.Add(this.cbMainUnitNotConvertedToText);
            this.Controls.Add(this.cbCurrencyFirstCharUpper);
            this.Controls.Add(this.cbMainUnitFirstCharUpper);
            this.Controls.Add(this.cbSubUnitNotConvertToText);
            this.Controls.Add(this.cbSubUnitZeroNotIncluded);
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
        private System.Windows.Forms.CheckBox cbSubUnitZeroNotIncluded;
        private System.Windows.Forms.CheckBox cbSubUnitNotConvertToText;
        private System.Windows.Forms.CheckBox cbMainUnitFirstCharUpper;
        private System.Windows.Forms.CheckBox cbCurrencyFirstCharUpper;
        private System.Windows.Forms.CheckBox cbMainUnitNotConvertedToText;
        private System.Windows.Forms.CheckBox cbSubUnitFirstCharUpper;
    }
}