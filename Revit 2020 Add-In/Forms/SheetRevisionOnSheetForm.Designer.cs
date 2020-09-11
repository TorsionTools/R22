namespace TorsionTools.Forms
{
    partial class SheetRevisionOnSheetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetRevisionOnSheetForm));
            this.ButtonClose = new System.Windows.Forms.Button();
            this.lvSheets = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(320, 721);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // lvSheets
            // 
            this.lvSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSheets.CheckBoxes = true;
            this.lvSheets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvSheets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSheets.HideSelection = false;
            this.lvSheets.Location = new System.Drawing.Point(12, 107);
            this.lvSheets.Name = "lvSheets";
            this.lvSheets.Size = new System.Drawing.Size(360, 608);
            this.lvSheets.TabIndex = 3;
            this.lvSheets.UseCompatibleStateImageBehavior = false;
            this.lvSheets.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 300;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(409, 725);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // SheetRevisionOnSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 760);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.lvSheets);
            this.Controls.Add(this.ButtonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SheetRevisionOnSheetForm";
            this.Text = "Revision On Sheets";
            this.Load += new System.EventHandler(this.SheetRevisionOnSheetForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.ListView lvSheets;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button ButtonOK;
    }
}