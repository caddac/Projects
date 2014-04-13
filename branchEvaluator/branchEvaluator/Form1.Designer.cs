namespace branchEvaluator
{
    partial class Form1
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
            this.btnBranch = new System.Windows.Forms.Button();
            this.txtbxOutput = new System.Windows.Forms.TextBox();
            this.txtbxInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnBranch
            // 
            this.btnBranch.Location = new System.Drawing.Point(215, 34);
            this.btnBranch.Name = "btnBranch";
            this.btnBranch.Size = new System.Drawing.Size(75, 23);
            this.btnBranch.TabIndex = 0;
            this.btnBranch.Text = "Branch!";
            this.btnBranch.UseVisualStyleBackColor = true;
            this.btnBranch.Click += new System.EventHandler(this.btnBranch_Click);
            // 
            // txtbxOutput
            // 
            this.txtbxOutput.Location = new System.Drawing.Point(12, 63);
            this.txtbxOutput.Multiline = true;
            this.txtbxOutput.Name = "txtbxOutput";
            this.txtbxOutput.Size = new System.Drawing.Size(485, 241);
            this.txtbxOutput.TabIndex = 1;
            // 
            // txtbxInput
            // 
            this.txtbxInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtbxInput.Location = new System.Drawing.Point(0, 0);
            this.txtbxInput.Name = "txtbxInput";
            this.txtbxInput.Size = new System.Drawing.Size(512, 20);
            this.txtbxInput.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 320);
            this.Controls.Add(this.txtbxInput);
            this.Controls.Add(this.txtbxOutput);
            this.Controls.Add(this.btnBranch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBranch;
        private System.Windows.Forms.TextBox txtbxOutput;
        private System.Windows.Forms.TextBox txtbxInput;
    }
}

