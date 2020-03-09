namespace Revit_2020_Add_In.Forms
{
    partial class FamilyMultipleSharedParametersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FamilyMultipleSharedParametersForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboGroups = new System.Windows.Forms.ComboBox();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.txtSPFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.ltvParameters = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.chkInstance = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(297, 626);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(216, 626);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboGroups
            // 
            this.cboGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGroups.FormattingEnabled = true;
            this.cboGroups.Location = new System.Drawing.Point(15, 85);
            this.cboGroups.Name = "cboGroups";
            this.cboGroups.Size = new System.Drawing.Size(357, 21);
            this.cboGroups.TabIndex = 2;
            this.cboGroups.SelectionChangeCommitted += new System.EventHandler(this.cboGroups_SelectionChangeCommitted);
            // 
            // btnDirectory
            // 
            this.btnDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDirectory.Location = new System.Drawing.Point(7, 597);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(95, 23);
            this.btnDirectory.TabIndex = 1;
            this.btnDirectory.Text = "Family Directory";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            // 
            // txtSPFile
            // 
            this.txtSPFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSPFile.Enabled = false;
            this.txtSPFile.Location = new System.Drawing.Point(15, 30);
            this.txtSPFile.Name = "txtSPFile";
            this.txtSPFile.Size = new System.Drawing.Size(357, 20);
            this.txtSPFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Shared Parameter File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Parameter Group:";
            // 
            // lblDirectory
            // 
            this.lblDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDirectory.Location = new System.Drawing.Point(113, 597);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(259, 23);
            this.lblDirectory.TabIndex = 5;
            this.lblDirectory.Text = "Select a Directory of Revit Families";
            this.lblDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ltvParameters
            // 
            this.ltvParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltvParameters.CheckBoxes = true;
            this.ltvParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ltvParameters.FullRowSelect = true;
            this.ltvParameters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ltvParameters.HideSelection = false;
            this.ltvParameters.LabelWrap = false;
            this.ltvParameters.Location = new System.Drawing.Point(12, 139);
            this.ltvParameters.Name = "ltvParameters";
            this.ltvParameters.Size = new System.Drawing.Size(360, 452);
            this.ltvParameters.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ltvParameters.TabIndex = 3;
            this.ltvParameters.UseCompatibleStateImageBehavior = false;
            this.ltvParameters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 350;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parameters:";
            // 
            // chkInstance
            // 
            this.chkInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInstance.AutoSize = true;
            this.chkInstance.Checked = true;
            this.chkInstance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInstance.Location = new System.Drawing.Point(254, 117);
            this.chkInstance.Name = "chkInstance";
            this.chkInstance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInstance.Size = new System.Drawing.Size(118, 17);
            this.chkInstance.TabIndex = 6;
            this.chkInstance.Text = "Instance Parameter";
            this.chkInstance.UseVisualStyleBackColor = true;
            // 
            // FamilyMultipleSharedParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 661);
            this.Controls.Add(this.chkInstance);
            this.Controls.Add(this.ltvParameters);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSPFile);
            this.Controls.Add(this.cboGroups);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 700);
            this.Name = "FamilyMultipleSharedParametersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Shared Parameters";
            this.Load += new System.EventHandler(this.SharedParameterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboGroups;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.TextBox txtSPFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.ListView ltvParameters;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkInstance;
    }
}