using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace TorsionTools.Forms
{
    //This will Find and Replace values for the Sheet Name or the Sheet Number for all sheets in the Document
    public partial class SheetFindReplaceForm : System.Windows.Forms.Form
    {
        //Class Variable  for the document.
        Document doc;

        public SheetFindReplaceForm(Document _doc)
        {
            InitializeComponent();
            //Set the CLass Variable to the Document
            doc = _doc;
        }

        //Do something when the form is first loaded
        private void SheetFindReplaceForm_Load(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Get the parameter value of any element passed to be used in the Filtered Element Collector
                ParameterValueProvider pvp;
                
                //Based on the Radio buttons for Name or Number to determine which parameter to use
                if (rdoSheetNumber.Checked)
                {
                    //Set the parameter to the ElementId of BuiltInParameter for the Sheet Number
                    pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NUMBER));
                }
                else
                {
                    //Set the parameter to the ElementId of BuiltInParameter for the Sheet Name
                    pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NAME));
                }

                //Set how to compare the Sheet Name or Number. This will check to see if it Contains the value passed
                FilterStringRuleEvaluator fsr = new FilterStringContains();
                
                //Set the value from the "Find" text box. The "true" at the end is for case sensativity
                FilterRule fRule = new FilterStringRule(pvp, fsr, txtFind.Text, true);

                //Create a filter based on the Filter String Rule and the Filter rule to use int he Filtered Element Collector
                ElementParameterFilter filter = new ElementParameterFilter(fRule);

                //Create a new DataTable to store the sheet information gathered from the Filtered Element Collector
                DataTable dt = new DataTable();
                //Add a new columns to the Data Table with Name and the type of Column
                dt.Columns.Add(new DataColumn("Sheet Number", typeof(string)));
                dt.Columns.Add(new DataColumn("Sheet Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Preview", typeof(string)));
                dt.Columns.Add(new DataColumn("Sheet Id", typeof(ElementId)));

                //The Filtered Element Collector will get all Sheets in the model that contains the "Find" value based on the Prameter Value Provider above
                using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter))
                {
                    //Loop through each sheet that is returned from the Collector
                    foreach (ViewSheet sheet in fec.ToElements())
                    {
                        //Based on the Radio buttons for Name or Number to determine which parameter to replace the values for the Preview
                        if (rdoSheetNumber.Checked)
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the Sheet Number
                            dt.Rows.Add(sheet.SheetNumber, sheet.Name, sheet.SheetNumber.Replace(txtFind.Text, txtReplace.Text), sheet.Id);
                        }
                        else
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the Sheet Name
                            dt.Rows.Add(sheet.SheetNumber, sheet.Name, sheet.Name.Replace(txtFind.Text, txtReplace.Text), sheet.Id);
                        }
                    }
                }
                //Check tome sure there is at least one Sheet (row) in the DataTable
                if (dt.Rows.Count > 0)
                {
                    //Enable the Replace button
                    btnReplace.Enabled = true;
                    //Use the Data Table to set the Data Source of the Data Grid View to display the sheet information
                    dgvSheets.DataSource = dt;
                    //Set the Sheet Name column to fill the extra space in the Data Grid View
                    dgvSheets.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                //Display a message that no Sheets were found based on the critera and set the Data Source to null for the Data Grid View
                else
                {
                    TaskDialog.Show("Preview", "No Sheets match the search criteria.");
                    dgvSheets.DataSource = null;
                    //Disable the Replace button
                    btnReplace.Enabled = false;
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }

        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Create a new Transaction to modify the Document
                using (Transaction trans = new Transaction(doc))
                {
                    //Start the Transaction and provide a Name for it in the Undo / Redo Dialog
                    trans.Start("Find and Replace Sheet Parameter");
                    //Loop through each row remaining in the Data Grid
                    foreach (DataGridViewRow row in dgvSheets.Rows)
                    {
                        //Get and Cast the ViewSheet element from the Data Grid row in the 4th Column to a ViewSheet (sheet)
                        ViewSheet sheet = doc.GetElement((ElementId)row.Cells[3].Value) as ViewSheet;
                        //Based on the Radio buttons for Name or Number determine which parameter to set
                        if (rdoSheetNumber.Checked)
                        {
                            //Set the Sheet Nnumber based on the "Preview" cell for the row
                            sheet.SheetNumber = (string)row.Cells[2].Value;
                        }
                        else
                        {
                            //Set the Sheet Name based on the "Preview" cell for the row
                            sheet.Name = (string)row.Cells[2].Value;
                        }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Set the dialog result of the form to make sure the command returns a Result.Cancelled
            DialogResult = DialogResult.Cancel;
            //Close the form
            Close();
        }

        //This button will remove the row from the DataGrid so that the name will not be changed
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
    }
}
