namespace TorsionTools.Forms
{
	partial class ExternalViewSelectionForm
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
			if(disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalViewSelectionForm));
			this.dgvLinkedViews = new System.Windows.Forms.DataGridView();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.cbViewPortTypes = new System.Windows.Forms.ComboBox();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboTitleBlock = new System.Windows.Forms.ComboBox();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.btnSelectFile = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvLinkedViews)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvLinkedViews
			// 
			this.dgvLinkedViews.AllowUserToAddRows = false;
			this.dgvLinkedViews.AllowUserToDeleteRows = false;
			this.dgvLinkedViews.AllowUserToOrderColumns = true;
			this.dgvLinkedViews.AllowUserToResizeRows = false;
			this.dgvLinkedViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvLinkedViews.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvLinkedViews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvLinkedViews.Location = new System.Drawing.Point(18, 286);
			this.dgvLinkedViews.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dgvLinkedViews.Name = "dgvLinkedViews";
			this.dgvLinkedViews.ReadOnly = true;
			this.dgvLinkedViews.RowHeadersVisible = false;
			this.dgvLinkedViews.RowHeadersWidth = 62;
			this.dgvLinkedViews.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvLinkedViews.Size = new System.Drawing.Size(992, 745);
			this.dgvLinkedViews.TabIndex = 6;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(898, 1040);
			this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(112, 35);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(776, 1040);
			this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(112, 35);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// cbViewPortTypes
			// 
			this.cbViewPortTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbViewPortTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cbViewPortTypes.FormattingEnabled = true;
			this.cbViewPortTypes.Location = new System.Drawing.Point(9, 83);
			this.cbViewPortTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cbViewPortTypes.Name = "cbViewPortTypes";
			this.cbViewPortTypes.Size = new System.Drawing.Size(968, 28);
			this.cbViewPortTypes.TabIndex = 2;
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(140, 246);
			this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(448, 26);
			this.txtSearch.TabIndex = 3;
			this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(18, 251);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(110, 20);
			this.label4.TabIndex = 5;
			this.label4.Text = "Search Views:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cbViewPortTypes);
			this.groupBox1.Controls.Add(this.cboTitleBlock);
			this.groupBox1.Location = new System.Drawing.Point(18, 92);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Size = new System.Drawing.Size(992, 132);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sheet and View Properties:";
			// 
			// cboTitleBlock
			// 
			this.cboTitleBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboTitleBlock.FormattingEnabled = true;
			this.cboTitleBlock.Location = new System.Drawing.Point(9, 34);
			this.cboTitleBlock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboTitleBlock.Name = "cboTitleBlock";
			this.cboTitleBlock.Size = new System.Drawing.Size(968, 28);
			this.cboTitleBlock.TabIndex = 1;
			// 
			// txtPath
			// 
			this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPath.Location = new System.Drawing.Point(18, 12);
			this.txtPath.Name = "txtPath";
			this.txtPath.ReadOnly = true;
			this.txtPath.Size = new System.Drawing.Size(992, 26);
			this.txtPath.TabIndex = 14;
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFile.Location = new System.Drawing.Point(898, 47);
			this.btnSelectFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(112, 35);
			this.btnSelectFile.TabIndex = 5;
			this.btnSelectFile.Text = "Select Model";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
			// 
			// ExternalViewSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1028, 1094);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSelectFile);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.dgvLinkedViews);
			this.Controls.Add(this.label4);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(889, 1124);
			this.Name = "ExternalViewSelectionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Linked View for Reference";
			this.Load += new System.EventHandler(this.LinkedViewSelectionForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvLinkedViews)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.DataGridView dgvLinkedViews;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ComboBox cbViewPortTypes;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboTitleBlock;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Button btnSelectFile;
	}
}