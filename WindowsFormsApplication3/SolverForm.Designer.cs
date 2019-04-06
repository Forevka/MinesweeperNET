namespace WindowsFormsApplication3
{
    partial class SolverForm
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
            this.MoveHistoryList = new System.Windows.Forms.ListBox();
            this.SolverEyeList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Next Move";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MoveHistoryList
            // 
            this.MoveHistoryList.FormattingEnabled = true;
            this.MoveHistoryList.Location = new System.Drawing.Point(11, 212);
            this.MoveHistoryList.Name = "MoveHistoryList";
            this.MoveHistoryList.Size = new System.Drawing.Size(260, 134);
            this.MoveHistoryList.TabIndex = 1;
            // 
            // SolverEyeList
            // 
            this.SolverEyeList.FormattingEnabled = true;
            this.SolverEyeList.Location = new System.Drawing.Point(12, 12);
            this.SolverEyeList.Name = "SolverEyeList";
            this.SolverEyeList.Size = new System.Drawing.Size(178, 186);
            this.SolverEyeList.TabIndex = 2;
            // 
            // SolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 358);
            this.Controls.Add(this.SolverEyeList);
            this.Controls.Add(this.MoveHistoryList);
            this.Controls.Add(this.button1);
            this.Name = "SolverForm";
            this.Text = "Solver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox MoveHistoryList;
        private System.Windows.Forms.ListBox SolverEyeList;
    }
}