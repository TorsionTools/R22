using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace TorsionTools.Forms
{
    public partial class ViewSheetUpdaterForm : Form
    {
        Document doc;
        ViewSheet sheet;

        public ViewSheetUpdaterForm(Document _doc, ViewSheet _viewSheet)
        {
            InitializeComponent();
            doc = _doc;
            sheet = _viewSheet;
        }

        private void ViewSheetUpdaterForm_Load(object sender, EventArgs e)
        {
            lblName.Text = sheet.Name;
            lblNumber.Text = sheet.SheetNumber;
            txtSheetNumber.Text = sheet.SheetNumber;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                sheet.Name = txtSheetName.Text;
                sheet.SheetNumber = txtSheetNumber.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Sheet Parameter Error", ex.ToString());
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
