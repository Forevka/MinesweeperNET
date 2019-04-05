namespace WindowsFormsApplication3
{
    partial class CustomFieldForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.HeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.MinesNumeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.WidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinesNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(139, 103);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mines";
            // 
            // WidthNumeric
            // 
            this.WidthNumeric.Location = new System.Drawing.Point(52, 52);
            this.WidthNumeric.Name = "WidthNumeric";
            this.WidthNumeric.Size = new System.Drawing.Size(73, 20);
            this.WidthNumeric.TabIndex = 8;
            // 
            // HeightNumeric
            // 
            this.HeightNumeric.Location = new System.Drawing.Point(52, 80);
            this.HeightNumeric.Name = "HeightNumeric";
            this.HeightNumeric.Size = new System.Drawing.Size(73, 20);
            this.HeightNumeric.TabIndex = 9;
            // 
            // MinesNumeric
            // 
            this.MinesNumeric.Location = new System.Drawing.Point(52, 106);
            this.MinesNumeric.Name = "MinesNumeric";
            this.MinesNumeric.Size = new System.Drawing.Size(73, 20);
            this.MinesNumeric.TabIndex = 10;
            // 
            // CustomFieldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 174);
            this.Controls.Add(this.MinesNumeric);
            this.Controls.Add(this.HeightNumeric);
            this.Controls.Add(this.WidthNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "CustomFieldForm";
            this.Text = "CustomFieldForm";
            ((System.ComponentModel.ISupportInitialize)(this.WidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinesNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown WidthNumeric;
        private System.Windows.Forms.NumericUpDown HeightNumeric;
        private System.Windows.Forms.NumericUpDown MinesNumeric;
    }
}