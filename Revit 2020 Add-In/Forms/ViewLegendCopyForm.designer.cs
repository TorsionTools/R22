namespace Revit_2020_Add_In.Forms
{
    partial class ViewLegendCopyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewLegendCopyForm));
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBoxLinks = new System.Windows.Forms.ComboBox();
            this.ListViewLegends = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonCopy = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select Linked Model:";
            // 
            // ComboBoxLinks
            // 
            this.ComboBoxLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxLinks.FormattingEnabled = true;
            this.ComboBoxLinks.Location = new System.Drawing.Point(12, 27);
            this.ComboBoxLinks.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ComboBoxLinks.Name = "ComboBoxLinks";
            this.ComboBoxLinks.Size = new System.Drawing.Size(357, 21);
            this.ComboBoxLinks.TabIndex = 4;
            this.ComboBoxLinks.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxLinks_SelectionChangeCommitted);
            // 
            // ListViewLegends
            // 
            this.ListViewLegends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewLegends.CheckBoxes = true;
            this.ListViewLegends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ListViewLegends.FullRowSelect = true;
            this.ListViewLegends.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListViewLegends.HideSelection = false;
            this.ListViewLegends.Location = new System.Drawing.Point(12, 82);
            this.ListViewLegends.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ListViewLegends.Name = "ListViewLegends";
            this.ListViewLegends.Size = new System.Drawing.Size(357, 438);
            this.ListViewLegends.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ListViewLegends.TabIndex = 6;
            this.ListViewLegends.UseCompatibleStateImageBehavior = false;
            this.ListViewLegends.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 350;
            // 
            // ButtonCopy
            // 
            this.ButtonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCopy.Location = new System.Drawing.Point(294, 526);
            this.ButtonCopy.Name = "ButtonCopy";
            this.ButtonCopy.Size = new System.Drawing.Size(75, 23);
            this.ButtonCopy.TabIndex = 7;
            this.ButtonCopy.Text = "OK";
            this.ButtonCopy.UseVisualStyleBackColor = true;
            this.ButtonCopy.Click += new System.EventHandler(this.ButtonCopy_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonClose.Location = new System.Drawing.Point(12, 526);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 8;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select Legends to Copy:";
            // 
            // ViewLegendCopyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonCopy);
            this.Controls.Add(this.ListViewLegends);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboBoxLinks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "ViewLegendCopyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Linked Legends";
            this.Load += new System.EventHandler(this.ViewLegendCopyForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBoxLinks;
        private System.Windows.Forms.ListView ListViewLegends;
        private System.Windows.Forms.Button ButtonCopy;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
    }
}