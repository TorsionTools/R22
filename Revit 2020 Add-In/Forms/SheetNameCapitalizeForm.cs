using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace TorsionTools.Forms
{
    public partial class SheetNameCapitalizeForm : System.Windows.Forms.Form
    {
        //Class Variable  for the document.
        Document doc;

        public SheetNameCapitalizeForm(Document _doc)
        {
            InitializeComponent();
            //Set the CLass Variable to the Document
            doc = _doc;
        }

        //Do something when the form is first loaded
        private void SheetNameCapitalizeForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Create a new DataTable to store the sheet information gathered from the Filtered Element Collector
                DataTable dt = new DataTable();
                //Add a new columns to the Data Table with Name and the type of Column
                dt.Columns.Add(new DataColumn("Sheet Number", typeof(string)));
                dt.Columns.Add(new DataColumn("Sheet Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Preview", typeof(string)));
                dt.Columns.Add(new DataColumn("Sheet Id", typeof(ElementId)));

                //The Filtered Element Collector will get all Sheets in the model
                using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets))
                {
                    //Loop through each sheet that is returned from the Collector
                    foreach (ViewSheet sheet in fec.ToElements())
                    {
                        //If the sheet name is already capitalized do not add it to the list
                        if (sheet.Name != sheet.Name.ToUpper())
                        {
                            //Create a new row in the Data Table and add the sheet information to it when Capitalizing the Sheet Name
                            dt.Rows.Add(sheet.SheetNumber, sheet.Name, sheet.Name.ToUpper(), sheet.Id);
                        }
                    }
                }
                //Check to make sure there is at least one Sheet (row) in the DataTable
                if (dt.Rows.Count > 0)
                {
                    //Enable the Replace button
                    btnCapitalize.Enabled = true;
                    //Use the Data Table to set the Data Source of the Data Grid View to display the sheet information
                    dgvSheets.DataSource = dt;
                    //Set the Sheet Name column to be as wide as te largest cell in Data Grid View
                    dgvSheets.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //Set the Preview column to fill the extra space in the Data Grid View
                    dgvSheets.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
                //Display a message that no Sheets were found based on the critera and set the Data Source to null for the Data Grid View
                else
                {
                    TaskDialog.Show("Preview", "No Sheets match the search criteria.");
                    dgvSheets.DataSource = null;
                    //Disable the Replace button
                    btnCapitalize.Enabled = false;
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Set the dialog result of the form to make sure the command returns a Result.Cancelled
            DialogResult = DialogResult.Cancel;
            //Close the form
            Close();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Loop through each Selected Row in the Data Grid View
                foreach (DataGridViewRow row in dgvSheets.SelectedRows)
                {
                    //Remove each selected row from the Data Grid View
                    dgvSheets.Rows.RemoveAt(row.Index);
                }
            }
            //Catch any errors and display a Dialog with the information
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }
        }

        private void btnCapitalize_Click(object sender, EventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Create a new Transaction to modify the Document
                using (Transaction trans = new Transaction(doc))
                {
                    //Start the Transaction and provide a Name for it in the Undo / Redo Dialog
                    trans.Start("Capitalize Sheet Names");
                    //Loop through each row remaining in the Data Grid
                    foreach (DataGridViewRow row in dgvSheets.Rows)
                    {
                        //Get and Cast the ViewSheet element from the Data Grid row in the 4th Column to a ViewSheet (sheet)
                        ViewSheet sheet = doc.GetElement((ElementId)row.Cells[3].Value) as ViewSheet;
                        //Set the Sheet Name based on the "Preview" cell for the row
                        sheet.Name = (string)row.Cells[2].Value;
                    }
                    //Commit the transaction to save the changes to the Document
                    trans.Commit();
                }
                //Set the dialog result of the form to make sure the command returns a Result.Succeeded
                DialogResult = DialogResult.OK;
                //Close the form
                Close();
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }
        }
    }
}
