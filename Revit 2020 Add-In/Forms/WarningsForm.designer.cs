namespace Revit_2020_Add_In.Forms
{
    partial class WarningsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningsForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.dgWarnings = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSelectionBox = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDoc = new System.Windows.Forms.Label();
            this.cboWarningType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIsolate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgWarnings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(897, 629);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgWarnings
            // 
            this.dgWarnings.AllowUserToAddRows = false;
            this.dgWarnings.AllowUserToDeleteRows = false;
            this.dgWarnings.AllowUserToResizeRows = false;
            this.dgWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgWarnings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWarnings.Location = new System.Drawing.Point(12, 56);
            this.dgWarnings.MultiSelect = false;
            this.dgWarnings.Name = "dgWarnings";
            this.dgWarnings.ReadOnly = true;
            this.dgWarnings.RowHeadersVisible = false;
            this.dgWarnings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgWarnings.ShowEditingIcon = false;
            this.dgWarnings.Size = new System.Drawing.Size(960, 567);
            this.dgWarnings.TabIndex = 3;
            this.dgWarnings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgWarnings_CellClick);
            this.dgWarnings.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgWarnings_CellDoubleClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(816, 629);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSelectionBox
            // 
            this.btnSelectionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectionBox.Location = new System.Drawing.Point(12, 629);
            this.btnSelectionBox.Name = "btnSelectionBox";
            this.btnSelectionBox.Size = new System.Drawing.Size(100, 23);
            this.btnSelectionBox.TabIndex = 2;
            this.btnSelectionBox.Text = "Selection Box";
            this.btnSelectionBox.UseVisualStyleBackColor = true;
            this.btnSelectionBox.Click += new System.EventHandler(this.btnSelectionBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Document:";
            // 
            // lblDoc
            // 
            this.lblDoc.AutoSize = true;
            this.lblDoc.Location = new System.Drawing.Point(65, 9);
            this.lblDoc.Name = "lblDoc";
            this.lblDoc.Size = new System.Drawing.Size(0, 13);
            this.lblDoc.TabIndex = 4;
            // 
            // cboWarningType
            // 
            this.cboWarningType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWarningType.FormattingEnabled = true;
            this.cboWarningType.Location = new System.Drawing.Point(131, 29);
            this.cboWarningType.Name = "cboWarningType";
            this.cboWarningType.Size = new System.Drawing.Size(841, 21);
            this.cboWarningType.TabIndex = 5;
            this.cboWarningType.SelectionChangeCommitted += new System.EventHandler(this.cboWarningType_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Filter by Warning Type:";
            // 
            // btnIsolate
            // 
            this.btnIsolate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIsolate.Location = new System.Drawing.Point(118, 629);
            this.btnIsolate.Name = "btnIsolate";
            this.btnIsolate.Size = new System.Drawing.Size(100, 23);
            this.btnIsolate.TabIndex = 7;
            this.btnIsolate.Text = "Isolate Filtered";
            this.btnIsolate.UseVisualStyleBackColor = true;
            this.btnIsolate.Click += new System.EventHandler(this.btnIsolate_Click);
            // 
            // WarningsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.btnIsolate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboWarningType);
            this.Controls.Add(this.lblDoc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectionBox);
            this.Controls.Add(this.dgWarnings);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "WarningsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warnings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WarningsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgWarnings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgWarnings;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSelectionBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDoc;
        private System.Windows.Forms.ComboBox cboWarningType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnIsolate;
    }
}