using System;

namespace TorsionTools.Forms
{
    public partial class SheetPreviewForm : System.Windows.Forms.Form
    {
        public SheetPreviewForm()
        {
            InitializeComponent();
        }
        
        //Create a public property to gain access to the preview control ElementHost on the form.
        public System.Windows.Forms.Integration.ElementHost PreviewHost
        {
            get
            {
                return PreviewControl;
            }
        }
    }
}
