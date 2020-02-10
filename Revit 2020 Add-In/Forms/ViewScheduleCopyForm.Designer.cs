namespace Revit_2020_Add_In.Forms
{
    partial class ViewScheduleCopyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewScheduleCopyForm));
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonCopy = new System.Windows.Forms.Button();
            this.ListViewSchedules = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBoxLinks = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonClose.Location = new System.Drawing.Point(12, 526);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 14;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonCopy
            // 
            this.ButtonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCopy.Location = new System.Drawing.Point(294, 526);
            this.ButtonCopy.Name = "ButtonCopy";
            this.ButtonCopy.Size = new System.Drawing.Size(75, 23);
            this.ButtonCopy.TabIndex = 13;
            this.ButtonCopy.Text = "OK";
            this.ButtonCopy.UseVisualStyleBackColor = true;
            this.ButtonCopy.Click += new System.EventHandler(this.ButtonCopy_Click);
            // 
            // ListViewSchedules
            // 
            this.ListViewSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewSchedules.CheckBoxes = true;
            this.ListViewSchedules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ListViewSchedules.FullRowSelect = true;
            this.ListViewSchedules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListViewSchedules.HideSelection = false;
            this.ListViewSchedules.Location = new System.Drawing.Point(12, 82);
            this.ListViewSchedules.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ListViewSchedules.Name = "ListViewSchedules";
            this.ListViewSchedules.Size = new System.Drawing.Size(357, 438);
            this.ListViewSchedules.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ListViewSchedules.TabIndex = 12;
            this.ListViewSchedules.UseCompatibleStateImageBehavior = false;
            this.ListViewSchedules.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 350;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Select Schedules to Copy:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 11;
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
            this.ComboBoxLinks.TabIndex = 9;
            this.ComboBoxLinks.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxLinks_SelectionChangeCommitted);
            // 
            // ViewScheduleCopyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonCopy);
            this.Controls.Add(this.ListViewSchedules);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboBoxLinks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "ViewScheduleCopyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Linked Schedules";
            this.Load += new System.EventHandler(this.ViewScheduleCopyForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonCopy;
        private System.Windows.Forms.ListView ListViewSchedules;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBoxLinks;
    }
}