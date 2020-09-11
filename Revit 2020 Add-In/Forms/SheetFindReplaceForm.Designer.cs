namespace TorsionTools.Forms
{
    partial class SheetFindReplaceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetFindReplaceForm));
            this.dgvSheets = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoSheetName = new System.Windows.Forms.RadioButton();
            this.rdoSheetNumber = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheets)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSheets
            // 
            this.dgvSheets.AllowUserToAddRows = false;
            this.dgvSheets.AllowUserToResizeRows = false;
            this.dgvSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSheets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSheets.Location = new System.Drawing.Point(218, 12);
            this.dgvSheets.Name = "dgvSheets";
            this.dgvSheets.RowHeadersVisible = false;
            this.dgvSheets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSheets.Size = new System.Drawing.Size(554, 408);
            this.dgvSheets.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoSheetName);
            this.groupBox1.Controls.Add(this.rdoSheetNumber);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 75);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Parameter";
            // 
            // rdoSheetName
            // 
            this.rdoSheetName.AutoSize = true;
            this.rdoSheetName.Checked = true;
            this.rdoSheetName.Location = new System.Drawing.Point(22, 19);
            this.rdoSheetName.Name = "rdoSheetName";
            this.rdoSheetName.Size = new System.Drawing.Size(84, 17);
            this.rdoSheetName.TabIndex = 0;
            this.rdoSheetName.TabStop = true;
            this.rdoSheetName.Text = "Sheet Name";
            this.rdoSheetName.UseVisualStyleBackColor = true;
            // 
            // rdoSheetNumber
            // 
            this.rdoSheetNumber.AutoSize = true;
            this.rdoSheetNumber.Location = new System.Drawing.Point(22, 45);
            this.rdoSheetNumber.Name = "rdoSheetNumber";
            this.rdoSheetNumber.Size = new System.Drawing.Size(93, 17);
            this.rdoSheetNumber.TabIndex = 1;
            this.rdoSheetNumber.Text = "Sheet Number";
            this.rdoSheetNumber.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Find:";
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(12, 130);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(200, 20);
            this.txtFind.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Replace With:";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(12, 182);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(200, 20);
            this.txtReplace.TabIndex = 3;
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Enabled = false;
            this.btnReplace.Location = new System.Drawing.Point(697, 426);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
            this.btnReplace.TabIndex = 5;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(137, 208);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 4;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(218, 426);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(616, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SheetFindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvSheets);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 500);
            this.Name = "SheetFindReplaceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find and Replace Sheet Name or Number";
            this.Load += new System.EventHandler(this.SheetFindReplaceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheets)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSheets;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoSheetName;
        private System.Windows.Forms.RadioButton rdoSheetNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCancel;
    }
}