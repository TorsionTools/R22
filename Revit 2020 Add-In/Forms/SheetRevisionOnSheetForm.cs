using Autodesk.Revit.DB;
using System;
using System.Windows.Forms;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit_2020_Add_In.Forms
{
    public partial class SheetRevisionOnSheetForm : System.Windows.Forms.Form
    {
        Document doc;
        public SheetRevisionOnSheetForm(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SheetRevisionOnSheetForm_Load(object sender, EventArgs e)
        {
            //Use the Collectors helper class to get all sheets in the project
            foreach (ViewSheet sheet in Helpers.Collectors.ByCategory(doc, BuiltInCategory.OST_Sheets))
            {
                //Create a new item in the ListVIew for each Sheet
                ListViewItem item = lvSheets.Items.Add(sheet.SheetNumber + " - " + sheet.Name);

                //Use the ListViewItem's Tag property to store the sheet to use
                item.Tag = sheet;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            //Cycle through each Checked Sheet in the ListView
            foreach (ListViewItem item in lvSheets.CheckedItems)
            {
                //Display a TaskDialog box with each Checked Sheet Number after casting it from the Tag property
                ViewSheet sheet = item.Tag as ViewSheet;
                TaskDialog.Show("Selected Sheet", "You selected sheet number " + sheet.SheetNumber);
            }
            //Set the DialogResult to make sure the form was successfuly used
            DialogResult = DialogResult.OK;

            //Close the form and return the DialogResult to the Command
            Close();
        }
    }
}
