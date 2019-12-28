using System;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;

namespace Revit_2020_Add_In.Forms
{
    public partial class SheetSelectionForm : System.Windows.Forms.Form
    {
        //Create a Document for the class to use
        Document doc;

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
            foreach (ViewSheet sheet in Collectors.ByCategory(doc, BuiltInCategory.OST_Sheets))
            {
                //Create a new item in the ListVIew for each Sheet
                ListViewItem item = lvSheets.Items.Add(sheet.SheetNumber + " - " + sheet.Name);
                
                //Use the ListViewItem's Tag property to store the sheet to use
                item.Tag = sheet;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Cycle through each Checked Sheet in the ListView
            foreach (ListViewItem item in lvSheets.CheckedItems)
            {
                //Display a TaskDialog box with each Checked Sheet Name after casting it from the Tag property
                TaskDialog.Show("Sheets",((ViewSheet)item.Tag).Name);
            }
            //Set the DialogResult to make sure the form was successfuly used
            DialogResult = DialogResult.OK;
            
            //Close the form and return the DialogResult to the Command
            Close();
        }
    }
}
