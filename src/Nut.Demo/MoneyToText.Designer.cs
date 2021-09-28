namespace Nut.Demo {
    partial class MoneyToText {
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
            this.cbAddAndBetweenUnits = new System.Windows.Forms.CheckBox();
            this.cbUseShortenedUnits = new System.Windows.Forms.CheckBox();
            this.cbRadixInCurrencyNotConvertedToText = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnMoneyToText
            // 
            this.btnMoneyToText.Location = new System.Drawing.Point(485, 36);
            this.btnMoneyToText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMoneyToText.Name = "btnMoneyToText";
            this.btnMoneyToText.Size = new System.Drawing.Size(119, 27);
            this.btnMoneyToText.TabIndex = 9;
            this.btnMoneyToText.Text = "Convert To Text";
            this.btnMoneyToText.UseVisualStyleBackColor = true;
            this.btnMoneyToText.Click += new System.EventHandler(this.btnMoneyToText_Click);
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(169, 39);
            this.txtNumber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(168, 23);
            this.txtNumber.TabIndex = 8;
            // 
            // cmbLang
            // 
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(35, 38);
            this.cmbLang.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(111, 23);
            this.cmbLang.TabIndex = 7;
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.FormattingEnabled = true;
            this.cmbCurrency.Location = new System.Drawing.Point(359, 38);
            this.cmbCurrency.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(111, 23);
            this.cmbCurrency.TabIndex = 11;
            // 
            // txtResultText
            // 
            this.txtResultText.Location = new System.Drawing.Point(35, 238);
            this.txtResultText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtResultText.Multiline = true;
            this.txtResultText.Name = "txtResultText";
            this.txtResultText.Size = new System.Drawing.Size(569, 86);
            this.txtResultText.TabIndex = 12;
            // 
            // cbSubUnitZeroNotIncluded
            // 
            this.cbSubUnitZeroNotIncluded.AutoSize = true;
            this.cbSubUnitZeroNotIncluded.Location = new System.Drawing.Point(253, 165);
            this.cbSubUnitZeroNotIncluded.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSubUnitZeroNotIncluded.Name = "cbSubUnitZeroNotIncluded";
            this.cbSubUnitZeroNotIncluded.Size = new System.Drawing.Size(176, 19);
            this.cbSubUnitZeroNotIncluded.TabIndex = 13;
            this.cbSubUnitZeroNotIncluded.Text = "Sub-unit Zero Not Displayed";
            this.cbSubUnitZeroNotIncluded.UseVisualStyleBackColor = true;
            // 
            // cbSubUnitNotConvertToText
            // 
            this.cbSubUnitNotConvertToText.AutoSize = true;
            this.cbSubUnitNotConvertToText.Location = new System.Drawing.Point(253, 81);
            this.cbSubUnitNotConvertToText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSubUnitNotConvertToText.Name = "cbSubUnitNotConvertToText";
            this.cbSubUnitNotConvertToText.Size = new System.Drawing.Size(179, 19);
            this.cbSubUnitNotConvertToText.TabIndex = 14;
            this.cbSubUnitNotConvertToText.Text = "Sub-unit Not Convert To Text";
            this.cbSubUnitNotConvertToText.UseVisualStyleBackColor = true;
            // 
            // cbMainUnitFirstCharUpper
            // 
            this.cbMainUnitFirstCharUpper.AutoSize = true;
            this.cbMainUnitFirstCharUpper.Location = new System.Drawing.Point(35, 123);
            this.cbMainUnitFirstCharUpper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbMainUnitFirstCharUpper.Name = "cbMainUnitFirstCharUpper";
            this.cbMainUnitFirstCharUpper.Size = new System.Drawing.Size(166, 19);
            this.cbMainUnitFirstCharUpper.TabIndex = 13;
            this.cbMainUnitFirstCharUpper.Text = "Main Unit First Char Upper";
            this.cbMainUnitFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbCurrencyFirstCharUpper
            // 
            this.cbCurrencyFirstCharUpper.AutoSize = true;
            this.cbCurrencyFirstCharUpper.Location = new System.Drawing.Point(35, 165);
            this.cbCurrencyFirstCharUpper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbCurrencyFirstCharUpper.Name = "cbCurrencyFirstCharUpper";
            this.cbCurrencyFirstCharUpper.Size = new System.Drawing.Size(162, 19);
            this.cbCurrencyFirstCharUpper.TabIndex = 14;
            this.cbCurrencyFirstCharUpper.Text = "Currency First Char Upper";
            this.cbCurrencyFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbMainUnitNotConvertedToText
            // 
            this.cbMainUnitNotConvertedToText.AutoSize = true;
            this.cbMainUnitNotConvertedToText.Location = new System.Drawing.Point(35, 81);
            this.cbMainUnitNotConvertedToText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbMainUnitNotConvertedToText.Name = "cbMainUnitNotConvertedToText";
            this.cbMainUnitNotConvertedToText.Size = new System.Drawing.Size(198, 19);
            this.cbMainUnitNotConvertedToText.TabIndex = 15;
            this.cbMainUnitNotConvertedToText.Text = "Main Unit Not Converted To Text";
            this.cbMainUnitNotConvertedToText.UseVisualStyleBackColor = true;
            // 
            // cbSubUnitFirstCharUpper
            // 
            this.cbSubUnitFirstCharUpper.AutoSize = true;
            this.cbSubUnitFirstCharUpper.Location = new System.Drawing.Point(253, 123);
            this.cbSubUnitFirstCharUpper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSubUnitFirstCharUpper.Name = "cbSubUnitFirstCharUpper";
            this.cbSubUnitFirstCharUpper.Size = new System.Drawing.Size(160, 19);
            this.cbSubUnitFirstCharUpper.TabIndex = 16;
            this.cbSubUnitFirstCharUpper.Text = "Sub-unit First Char Upper";
            this.cbSubUnitFirstCharUpper.UseVisualStyleBackColor = true;
            // 
            // cbAddAndBetweenUnits
            // 
            this.cbAddAndBetweenUnits.AutoSize = true;
            this.cbAddAndBetweenUnits.Location = new System.Drawing.Point(35, 207);
            this.cbAddAndBetweenUnits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbAddAndBetweenUnits.Name = "cbAddAndBetweenUnits";
            this.cbAddAndBetweenUnits.Size = new System.Drawing.Size(204, 19);
            this.cbAddAndBetweenUnits.TabIndex = 18;
            this.cbAddAndBetweenUnits.Text = "Add & between Main and Sub units";
            this.cbAddAndBetweenUnits.UseVisualStyleBackColor = true;
            // 
            // cbUseShortenedUnits
            // 
            this.cbUseShortenedUnits.AutoSize = true;
            this.cbUseShortenedUnits.Location = new System.Drawing.Point(253, 207);
            this.cbUseShortenedUnits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbUseShortenedUnits.Name = "cbUseShortenedUnits";
            this.cbUseShortenedUnits.Size = new System.Drawing.Size(185, 19);
            this.cbUseShortenedUnits.TabIndex = 17;
            this.cbUseShortenedUnits.Text = "Use shortened version of units";
            this.cbUseShortenedUnits.UseVisualStyleBackColor = true;
            // 
            // cbRadixInCurrencyNotConvertedToText
            // 
            this.cbRadixInCurrencyNotConvertedToText.AutoSize = true;
            this.cbRadixInCurrencyNotConvertedToText.Location = new System.Drawing.Point(445, 81);
            this.cbRadixInCurrencyNotConvertedToText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbRadixInCurrencyNotConvertedToText.Name = "cbRadixInCurrencyNotConvertedToText";
            this.cbRadixInCurrencyNotConvertedToText.Size = new System.Drawing.Size(174, 19);
            this.cbRadixInCurrencyNotConvertedToText.TabIndex = 19;
            this.cbRadixInCurrencyNotConvertedToText.Text = "Radix Not Converted to Text";
            this.cbRadixInCurrencyNotConvertedToText.UseVisualStyleBackColor = true;
            // 
            // MoneyToText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 338);
            this.Controls.Add(this.cbRadixInCurrencyNotConvertedToText);
            this.Controls.Add(this.cbAddAndBetweenUnits);
            this.Controls.Add(this.cbUseShortenedUnits);
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
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MoneyToText";
            this.Text = "Money To Text";
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
        private System.Windows.Forms.CheckBox cbAddAndBetweenUnits;
        private System.Windows.Forms.CheckBox cbUseShortenedUnits;
        private System.Windows.Forms.CheckBox cbRadixInCurrencyNotConvertedToText;
    }
}