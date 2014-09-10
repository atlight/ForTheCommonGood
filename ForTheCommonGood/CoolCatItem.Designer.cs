namespace ForTheCommonGood
{
    partial class CoolCatItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboCatName = new System.Windows.Forms.ComboBox();
            this.lnkCatLink = new System.Windows.Forms.LinkLabel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.panEdit = new System.Windows.Forms.FlowLayoutPanel();
            this.panView = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.panEdit.SuspendLayout();
            this.panView.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnModify.Location = new System.Drawing.Point(30, 0);
            this.btnModify.Margin = new System.Windows.Forms.Padding(0);
            this.btnModify.Name = "btnModify";
            this.btnModify.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnModify.Size = new System.Drawing.Size(24, 24);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "±";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRemove.Location = new System.Drawing.Point(6, 0);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "–";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Location = new System.Drawing.Point(254, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnCancel.Size = new System.Drawing.Size(64, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "<cancel>";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(204, 1);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnOK.Size = new System.Drawing.Size(48, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "<OK>";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cboCatName
            // 
            this.cboCatName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCatName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboCatName.FormattingEnabled = true;
            this.cboCatName.Location = new System.Drawing.Point(0, 2);
            this.cboCatName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.cboCatName.Name = "cboCatName";
            this.cboCatName.Size = new System.Drawing.Size(184, 21);
            this.cboCatName.Sorted = true;
            this.cboCatName.TabIndex = 0;
            this.cboCatName.TextUpdate += new System.EventHandler(this.cboCatName_TextUpdate);
            this.cboCatName.DropDownClosed += new System.EventHandler(this.cboCatName_DropDownClosed);
            this.cboCatName.TextChanged += new System.EventHandler(this.cboCatName_TextChanged);
            this.cboCatName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCatName_KeyDown);
            // 
            // lnkCatLink
            // 
            this.lnkCatLink.AutoEllipsis = true;
            this.lnkCatLink.AutoSize = true;
            this.lnkCatLink.Location = new System.Drawing.Point(3, 5);
            this.lnkCatLink.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lnkCatLink.MaximumSize = new System.Drawing.Size(350, 25);
            this.lnkCatLink.Name = "lnkCatLink";
            this.lnkCatLink.Size = new System.Drawing.Size(0, 13);
            this.lnkCatLink.TabIndex = 0;
            this.lnkCatLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCatLink_LinkClicked);
            // 
            // picIcon
            // 
            this.picIcon.Image = global::ForTheCommonGood.Properties.Resources.Small_cross;
            this.picIcon.Location = new System.Drawing.Point(186, 4);
            this.picIcon.Margin = new System.Windows.Forms.Padding(2, 4, 0, 0);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(16, 16);
            this.picIcon.TabIndex = 7;
            this.picIcon.TabStop = false;
            // 
            // panEdit
            // 
            this.panEdit.Controls.Add(this.cboCatName);
            this.panEdit.Controls.Add(this.picIcon);
            this.panEdit.Controls.Add(this.btnOK);
            this.panEdit.Controls.Add(this.btnCancel);
            this.panEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.panEdit.Location = new System.Drawing.Point(0, 0);
            this.panEdit.Margin = new System.Windows.Forms.Padding(0);
            this.panEdit.MaximumSize = new System.Drawing.Size(99999, 40);
            this.panEdit.Name = "panEdit";
            this.panEdit.Size = new System.Drawing.Size(555, 27);
            this.panEdit.TabIndex = 8;
            this.panEdit.WrapContents = false;
            // 
            // panView
            // 
            this.panView.AutoSize = true;
            this.panView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panView.Controls.Add(this.lnkCatLink);
            this.panView.Controls.Add(this.btnRemove);
            this.panView.Controls.Add(this.btnModify);
            this.panView.Location = new System.Drawing.Point(0, 0);
            this.panView.MaximumSize = new System.Drawing.Size(99999, 40);
            this.panView.Name = "panView";
            this.panView.Size = new System.Drawing.Size(54, 24);
            this.panView.TabIndex = 9;
            // 
            // CoolCatItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panView);
            this.Controls.Add(this.panEdit);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.Name = "CoolCatItem";
            this.Size = new System.Drawing.Size(318, 27);
            this.Load += new System.EventHandler(this.CoolCatItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.panEdit.ResumeLayout(false);
            this.panEdit.PerformLayout();
            this.panView.ResumeLayout(false);
            this.panView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboCatName;
        private System.Windows.Forms.LinkLabel lnkCatLink;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.FlowLayoutPanel panEdit;
        private System.Windows.Forms.FlowLayoutPanel panView;
    }
}
