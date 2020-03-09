using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Revit_2020_Add_In.Forms
{
    public partial class SheetSelectionForm : System.Windows.Forms.Form
    {
        //Create a Document for the class to use
        Document doc;

        //Public Property to hold Sheet Element Ids
        public List<ElementId> ViewSheetIds { get; set; }

        //Have the Command pass the Document as a variable
        public SheetSelectionForm(Document _doc)
        {
            InitializeComponent();

            //Set the local Document variable to the one passed to the form
            doc = _doc;
        }

        private void SheetSelectionForm_Load(object sender, EventArgs e)
        {
            //Use the Collectors helper class to get all sheets in the project
            foreach (ViewSheet sheet in Helpers.Collectors.ByCategory(doc, BuiltInCategory.OST_Sheets))
            {
                //Check to see if the sheet is a place holder sheet before adding it to the selection
                if (!sheet.IsPlaceholder)
                {
                    //Create a new item in the ListVIew for each Sheet
                    ListViewItem item = lvSheets.Items.Add(sheet.SheetNumber + " - " + sheet.Name);

                    //Use the ListViewItem's Tag property to store the Sheet ElementId to use
                    item.Tag = sheet.Id;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Create a list of ElementIds to hold the Element Id of each selected sheet
            List<ElementId> sheetIds = new List<ElementId>();
            //Cycle through each Checked Sheet in the ListView
            foreach (ListViewItem item in lvSheets.CheckedItems)
            {
                //Cast the ListViewItem's Tag property to ElementId and add it to the sheetIds list
                sheetIds.Add(item.Tag as ElementId);
            }

            //public Property to the list of Sheet Element Ids so we can iterate them back in the command.
            ViewSheetIds = sheetIds;

            //Set the DialogResult to make sure the form was successfuly used
            DialogResult = DialogResult.OK;

            //Close the form and return the DialogResult to the Command
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Set the DialogResult to make sure the command knows the form was canceled
            DialogResult = DialogResult.Cancel;
            //Close the form and return the DialogResult to the Command
            Close();
        }
    }
}
