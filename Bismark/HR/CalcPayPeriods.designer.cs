namespace Bismark.GUI
{
    partial class CalcPayPeriods
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstPayPeriods = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstPayPeriods
            // 
            this.lstPayPeriods.FormattingEnabled = true;
            this.lstPayPeriods.Location = new System.Drawing.Point(12, 12);
            this.lstPayPeriods.Name = "lstPayPeriods";
            this.lstPayPeriods.Size = new System.Drawing.Size(196, 368);
            this.lstPayPeriods.TabIndex = 0;
            // 
            // CalcPayPeriods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 395);
            this.Controls.Add(this.lstPayPeriods);
            this.Name = "CalcPayPeriods";
            this.Text = "CalcPayPeriods";
            this.Load += new System.EventHandler(this.CalcPayPeriods_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstPayPeriods;
    }
}