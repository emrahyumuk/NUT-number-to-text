namespace Nut.Demo {
    partial class Form1 {
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
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnNumberToText = new System.Windows.Forms.Button();
            this.txtResultText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbLang
            // 
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(32, 31);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(96, 21);
            this.cmbLang.TabIndex = 3;
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(147, 32);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(145, 20);
            this.txtNumber.TabIndex = 4;
            // 
            // btnNumberToText
            // 
            this.btnNumberToText.Location = new System.Drawing.Point(313, 29);
            this.btnNumberToText.Name = "btnNumberToText";
            this.btnNumberToText.Size = new System.Drawing.Size(102, 23);
            this.btnNumberToText.TabIndex = 5;
            this.btnNumberToText.Text = "Convert To Text";
            this.btnNumberToText.UseVisualStyleBackColor = true;
            this.btnNumberToText.Click += new System.EventHandler(this.btnNumberToText_Click);
            // 
            // txtResultText
            // 
            this.txtResultText.Location = new System.Drawing.Point(32, 82);
            this.txtResultText.Multiline = true;
            this.txtResultText.Name = "txtResultText";
            this.txtResultText.Size = new System.Drawing.Size(383, 117);
            this.txtResultText.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 237);
            this.Controls.Add(this.txtResultText);
            this.Controls.Add(this.btnNumberToText);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.cmbLang);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Button btnNumberToText;
        private System.Windows.Forms.TextBox txtResultText;

    }
}

