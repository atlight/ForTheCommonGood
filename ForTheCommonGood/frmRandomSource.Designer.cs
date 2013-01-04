namespace ForTheCommonGood
{
    partial class frmRandomSource
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
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.optCategory = new System.Windows.Forms.RadioButton();
            this.optTextFile = new System.Windows.Forms.RadioButton();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(24, 28);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(360, 21);
            this.txtCategory.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(312, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "<cancel>";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(232, 102);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "<ok>";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // optCategory
            // 
            this.optCategory.AutoSize = true;
            this.optCategory.Checked = true;
            this.optCategory.Location = new System.Drawing.Point(8, 8);
            this.optCategory.Name = "optCategory";
            this.optCategory.Size = new System.Drawing.Size(84, 17);
            this.optCategory.TabIndex = 4;
            this.optCategory.TabStop = true;
            this.optCategory.Text = "<category>";
            this.optCategory.UseVisualStyleBackColor = true;
            // 
            // optTextFile
            // 
            this.optTextFile.AutoSize = true;
            this.optTextFile.Location = new System.Drawing.Point(8, 56);
            this.optTextFile.Name = "optTextFile";
            this.optTextFile.Size = new System.Drawing.Size(78, 17);
            this.optTextFile.TabIndex = 4;
            this.optTextFile.Text = "<text file>";
            this.optTextFile.UseVisualStyleBackColor = true;
            this.optTextFile.CheckedChanged += new System.EventHandler(this.CheckChange);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(24, 76);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(282, 21);
            this.txtFileName.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(312, 75);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "<browse>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "All files (*.*)|*.*";
            this.openFileDialog1.Title = "Open text file";
            // 
            // frmRandomSource
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 132);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.optTextFile);
            this.Controls.Add(this.optCategory);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.txtCategory);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRandomSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<random source>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRandomSource_FormClosing);
            this.Load += new System.EventHandler(this.CheckChange);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        internal System.Windows.Forms.TextBox txtCategory;
        internal System.Windows.Forms.RadioButton optCategory;
        internal System.Windows.Forms.RadioButton optTextFile;
        internal System.Windows.Forms.TextBox txtFileName;
        internal System.Windows.Forms.Button button3;
    }
}