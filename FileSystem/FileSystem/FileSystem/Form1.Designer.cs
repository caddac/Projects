namespace FileSystem
{
    partial class frmFileSystem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileSystem));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnPut = new System.Windows.Forms.Button();
            this.cbMounted = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGet = new System.Windows.Forms.Button();
            this.lstbxFilesList = new System.Windows.Forms.ListBox();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbxFSName = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnMountBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnUnMount = new System.Windows.Forms.Button();
            this.btnSaveFS = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPut
            // 
            this.btnPut.Location = new System.Drawing.Point(11, 344);
            this.btnPut.Name = "btnPut";
            this.btnPut.Size = new System.Drawing.Size(75, 23);
            this.btnPut.TabIndex = 3;
            this.btnPut.Text = "Put File";
            this.btnPut.UseVisualStyleBackColor = true;
            this.btnPut.Click += new System.EventHandler(this.btnPut_Click);
            // 
            // cbMounted
            // 
            this.cbMounted.FormattingEnabled = true;
            this.cbMounted.Location = new System.Drawing.Point(99, 99);
            this.cbMounted.Name = "cbMounted";
            this.cbMounted.Size = new System.Drawing.Size(149, 21);
            this.cbMounted.TabIndex = 4;
            this.cbMounted.SelectedIndexChanged += new System.EventHandler(this.cbMounted_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mounted Disks:";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(92, 344);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(75, 23);
            this.btnGet.TabIndex = 8;
            this.btnGet.Text = "Get File";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // lstbxFilesList
            // 
            this.lstbxFilesList.FormattingEnabled = true;
            this.lstbxFilesList.Location = new System.Drawing.Point(11, 126);
            this.lstbxFilesList.Name = "lstbxFilesList";
            this.lstbxFilesList.Size = new System.Drawing.Size(237, 212);
            this.lstbxFilesList.TabIndex = 9;
            // 
            // numericUpDown
            // 
            this.numericUpDown.Location = new System.Drawing.Point(67, 11);
            this.numericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown.TabIndex = 10;
            this.numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "FS Size: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "FS Name:";
            // 
            // txtbxFSName
            // 
            this.txtbxFSName.Location = new System.Drawing.Point(68, 37);
            this.txtbxFSName.MaxLength = 16;
            this.txtbxFSName.Name = "txtbxFSName";
            this.txtbxFSName.Size = new System.Drawing.Size(100, 20);
            this.txtbxFSName.TabIndex = 13;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(174, 35);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 14;
            this.btnCreate.Text = "Create FS";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnMountBrowse
            // 
            this.btnMountBrowse.Location = new System.Drawing.Point(11, 67);
            this.btnMountBrowse.Name = "btnMountBrowse";
            this.btnMountBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnMountBrowse.TabIndex = 17;
            this.btnMountBrowse.Text = "Mount FS";
            this.btnMountBrowse.UseVisualStyleBackColor = true;
            this.btnMountBrowse.Click += new System.EventHandler(this.btnMountBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnUnMount
            // 
            this.btnUnMount.Location = new System.Drawing.Point(173, 67);
            this.btnUnMount.Name = "btnUnMount";
            this.btnUnMount.Size = new System.Drawing.Size(75, 23);
            this.btnUnMount.TabIndex = 21;
            this.btnUnMount.Text = "UnMount FS";
            this.btnUnMount.UseVisualStyleBackColor = true;
            this.btnUnMount.Click += new System.EventHandler(this.btnUnMount_Click);
            // 
            // btnSaveFS
            // 
            this.btnSaveFS.Location = new System.Drawing.Point(92, 67);
            this.btnSaveFS.Name = "btnSaveFS";
            this.btnSaveFS.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFS.TabIndex = 22;
            this.btnSaveFS.Text = "Save FS";
            this.btnSaveFS.UseVisualStyleBackColor = true;
            this.btnSaveFS.Click += new System.EventHandler(this.btnSaveFS_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(173, 344);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete File";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(190, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "bytes";
            // 
            // frmFileSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 382);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSaveFS);
            this.Controls.Add(this.btnUnMount);
            this.Controls.Add(this.btnMountBrowse);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtbxFSName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.lstbxFilesList);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbMounted);
            this.Controls.Add(this.btnPut);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(278, 421);
            this.MinimumSize = new System.Drawing.Size(278, 421);
            this.Name = "frmFileSystem";
            this.Text = "File System";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnPut;
        private System.Windows.Forms.ComboBox cbMounted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.ListBox lstbxFilesList;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtbxFSName;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnMountBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnUnMount;
        private System.Windows.Forms.Button btnSaveFS;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label2;
    }
}

