namespace Revit_2020_Add_In.Forms
{
    partial class SheetTitleblockKeyPlanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetTitleblockKeyPlanForm));
            this.cboParameter = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoHide = new System.Windows.Forms.RadioButton();
            this.rdoShow = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTitleBlock = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoName = new System.Windows.Forms.RadioButton();
            this.rdoNumber = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboParameter
            // 
            this.cboParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboParameter.FormattingEnabled = true;
            this.cboParameter.Location = new System.Drawing.Point(12, 81);
            this.cboParameter.Name = "cboParameter";
            this.cboParameter.Size = new System.Drawing.Size(200, 21);
            this.cboParameter.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoHide);
            this.groupBox1.Controls.Add(this.rdoShow);
            this.groupBox1.Location = new System.Drawing.Point(12, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Yes / No Parameter";
            // 
            // rdoHide
            // 
            this.rdoHide.AutoSize = true;
            this.rdoHide.Location = new System.Drawing.Point(105, 19);
            this.rdoHide.Name = "rdoHide";
            this.rdoHide.Size = new System.Drawing.Size(81, 17);
            this.rdoHide.TabIndex = 6;
            this.rdoHide.Text = "Unchecked";
            this.rdoHide.UseVisualStyleBackColor = true;
            // 
            // rdoShow
            // 
            this.rdoShow.AutoSize = true;
            this.rdoShow.Checked = true;
            this.rdoShow.Location = new System.Drawing.Point(6, 19);
            this.rdoShow.Name = "rdoShow";
            this.rdoShow.Size = new System.Drawing.Size(68, 17);
            this.rdoShow.TabIndex = 5;
            this.rdoShow.TabStop = true;
            this.rdoShow.Text = "Checked";
            this.rdoShow.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Yes/No Parameter:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(137, 275);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select a Title Block Type:";
            // 
            // cboTitleBlock
            // 
            this.cboTitleBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTitleBlock.FormattingEnabled = true;
            this.cboTitleBlock.Location = new System.Drawing.Point(12, 28);
            this.cboTitleBlock.Name = "cboTitleBlock";
            this.cboTitleBlock.Size = new System.Drawing.Size(200, 21);
            this.cboTitleBlock.TabIndex = 0;
            this.cboTitleBlock.SelectionChangeCommitted += new System.EventHandler(this.cboTitleBlock_SelectionChangeCommitted);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(12, 134);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Search For:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoName);
            this.groupBox2.Controls.Add(this.rdoNumber);
            this.groupBox2.Location = new System.Drawing.Point(12, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 47);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // rdoName
            // 
            this.rdoName.AutoSize = true;
            this.rdoName.Checked = true;
            this.rdoName.Location = new System.Drawing.Point(6, 19);
            this.rdoName.Name = "rdoName";
            this.rdoName.Size = new System.Drawing.Size(84, 17);
            this.rdoName.TabIndex = 3;
            this.rdoName.TabStop = true;
            this.rdoName.Text = "Sheet Name";
            this.rdoName.UseVisualStyleBackColor = true;
            // 
            // rdoNumber
            // 
            this.rdoNumber.AutoSize = true;
            this.rdoNumber.Location = new System.Drawing.Point(105, 19);
            this.rdoNumber.Name = "rdoNumber";
            this.rdoNumber.Size = new System.Drawing.Size(93, 17);
            this.rdoNumber.TabIndex = 4;
            this.rdoNumber.Text = "Sheet Number";
            this.rdoNumber.UseVisualStyleBackColor = true;
            // 
            // SheetTitleblockKeyPlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 306);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cboTitleBlock);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboParameter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 345);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(240, 345);
            this.Name = "SheetTitleblockKeyPlanForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title Block Key Plan";
            this.Load += new System.EventHandler(this.SheetTitleblockKeyPlanForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboParameter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoHide;
        private System.Windows.Forms.RadioButton rdoShow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTitleBlock;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoName;
        private System.Windows.Forms.RadioButton rdoNumber;
    }
}