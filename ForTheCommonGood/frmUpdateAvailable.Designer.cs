namespace ForTheCommonGood
{
    partial class frmUpdateAvailable
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panBack = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNoticePre = new System.Windows.Forms.Label();
            this.lblNoticeLink = new System.Windows.Forms.LinkLabel();
            this.lblNoticePost = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panBack.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(350, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "<ok>";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ForTheCommonGood.Properties.Resources.Ambox_notice;
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(62, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(346, 20);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "<update available>";
            // 
            // panBack
            // 
            this.panBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
            this.panBack.Controls.Add(this.flowLayoutPanel1);
            this.panBack.Controls.Add(this.pictureBox1);
            this.panBack.Controls.Add(this.lblTitle);
            this.panBack.Location = new System.Drawing.Point(8, 8);
            this.panBack.Name = "panBack";
            this.panBack.Size = new System.Drawing.Size(416, 152);
            this.panBack.TabIndex = 8;
            this.panBack.Paint += new System.Windows.Forms.PaintEventHandler(this.panBack_Paint);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.lblNoticePre);
            this.flowLayoutPanel1.Controls.Add(this.lblNoticeLink);
            this.flowLayoutPanel1.Controls.Add(this.lblNoticePost);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(60, 37);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(349, 109);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // lblNoticePre
            // 
            this.lblNoticePre.AutoSize = true;
            this.lblNoticePre.Location = new System.Drawing.Point(3, 0);
            this.lblNoticePre.Name = "lblNoticePre";
            this.lblNoticePre.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblNoticePre.Size = new System.Drawing.Size(128, 24);
            this.lblNoticePre.TabIndex = 0;
            this.lblNoticePre.Text = "<new version available>";
            this.lblNoticePre.UseCompatibleTextRendering = true;
            // 
            // lblNoticeLink
            // 
            this.lblNoticeLink.AutoSize = true;
            this.lblNoticeLink.Location = new System.Drawing.Point(3, 24);
            this.lblNoticeLink.Name = "lblNoticeLink";
            this.lblNoticeLink.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblNoticeLink.Size = new System.Drawing.Size(79, 24);
            this.lblNoticeLink.TabIndex = 9;
            this.lblNoticeLink.TabStop = true;
            this.lblNoticeLink.Text = "<download it>";
            this.lblNoticeLink.UseCompatibleTextRendering = true;
            this.lblNoticeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblNoticeLink_LinkClicked);
            // 
            // lblNoticePost
            // 
            this.lblNoticePost.AutoSize = true;
            this.lblNoticePost.Location = new System.Drawing.Point(3, 48);
            this.lblNoticePost.Name = "lblNoticePost";
            this.lblNoticePost.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblNoticePost.Size = new System.Drawing.Size(125, 26);
            this.lblNoticePost.TabIndex = 10;
            this.lblNoticePost.TabStop = true;
            this.lblNoticePost.Text = "<disable update check>";
            this.lblNoticePost.UseCompatibleTextRendering = true;
            this.lblNoticePost.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // frmUpdateAvailable
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(432, 198);
            this.Controls.Add(this.panBack);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateAvailable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<update available>";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panBack.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panBack;
        private System.Windows.Forms.LinkLabel lblNoticeLink;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblNoticePre;
        private System.Windows.Forms.LinkLabel lblNoticePost;
    }
}