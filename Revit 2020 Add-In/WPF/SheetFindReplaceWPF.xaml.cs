using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace Revit_2020_Add_In.WPF
{
    /// <summary>
    /// Interaction logic for SheetFindReplaceWPF.xaml
    /// </summary>
    public partial class SheetFindReplaceWPF : Window
    {
        //Set Class Level Variables
        Document doc;
        //Create a new DataTable to store the sheet information gathered from the Filtered Element Collector
        DataTable SheetTable = new DataTable();

        //We call this form with the current document and set the class Document variable
        public SheetFindReplaceWPF(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Clear the data in the Data Table for each search
                SheetTable.Clear();

                //Get the parameter value of any element passed to be used in the Filtered Element Collector
                ParameterValueProvider pvp;

                //Based on the Radio buttons for Name or Number to determine which parameter to use
                if (rdoSheetNumber.IsChecked == true)
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

                //Check to see if the Data Table already has columns so we don't need to add them again.
                if (SheetTable.Columns.Count == 0)
                {
                    //Add a new columns to the Data Table with Name and the type of Column
                    SheetTable.Columns.Add(new DataColumn("SheetId", typeof(ElementId)));
                    SheetTable.Columns.Add(new DataColumn("SheetNumber", typeof(string)));
                    SheetTable.Columns.Add(new DataColumn("SheetName", typeof(string)));
                    SheetTable.Columns.Add(new DataColumn("Preview", typeof(string)));
                }

                //The Filtered Element Collector will get all Sheets in the model that contains the "Find" value based on the Prameter Value Provider above
                using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter))
                {
                    //Loop through each sheet that is returned from the Collector
                    foreach (ViewSheet sheet in fec.ToElements())
                    {
                        //Based on the Radio buttons for Name or Number to determine which parameter to replace the values for the Preview
                        if (rdoSheetNumber.IsChecked == true)
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the Sheet Number
                            SheetTable.Rows.Add(sheet.Id, sheet.SheetNumber, sheet.Name, sheet.SheetNumber.Replace(txtFind.Text, txtReplace.Text));
                        }
                        else
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the Sheet Name
                            SheetTable.Rows.Add(sheet.Id, sheet.SheetNumber, sheet.Name, sheet.Name.Replace(txtFind.Text, txtReplace.Text));
                        }
                    }
                }
                //Check to make sure there is at least one Sheet (row) in the DataTable
                if (SheetTable.Rows.Count > 0)
                {
                    //Enable the Replace button
                    btnReplace.IsEnabled = true;
                    //Use the Data Table to set the Data Source of the Data Grid View to display the sheet information
                    DataGridSheets.ItemsSource = SheetTable.DefaultView;
                }
                //Display a message that no Sheets were found based on the critera and set the Data Source to null for the Data Grid View
                else
                {
                    //Clear the data from the Data Grid
                    DataGridSheets.ItemsSource = null;
                    //Disable the Replace button
                    btnReplace.IsEnabled = false;
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }
        }

        private void BtnReplace_Click(object sender, RoutedEventArgs e)
        {
            //Make sure there are rows in the Data Table to change
            if (SheetTable.Rows.Count > 0)
            {
                //Use a Try block to keep any errors from crashing Revit
                try
                {
                    //Use a transaction to modify the Document Database
                    using (Transaction Trans = new Transaction(doc))
                    {
                        //Start and Name the transaction to show up in the Undo List
                        Trans.Start("Sheet Find Replace");
                        //Iterate through each row to rename as needed
                        foreach (DataRow row in SheetTable.Rows)
                        {
                            //Cast the ElmentId from the SheetId column of the data Table
                            ViewSheet sheet = doc.GetElement((ElementId)row["SheetId"]) as ViewSheet;
                            //Change the name
                            sheet.Name = (string)row["Preview"];
                        }
                        //Commit the Transaction to keep the changes
                        Trans.Commit();
                    }
                    //Set the Dialog result so the calling command knows the form completed successfuly
                    DialogResult = true;
                    //Close the form
                    Close();
                }
                //Catch any errors and display a Dialog with the informaiton
                catch (Exception ex)
                {
                    TaskDialog.Show("Rename Error", ex.ToString() + " : " + ex.InnerException);
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //Set the Dialog result to false so the calling command knows the form was canceled and no work was done
            DialogResult = false;
            //Close the form
            Close();
        }

        //This method is what removes rows from the Data Grid so they are not renamed
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            //Create a list to hold the rows to delete since we can't directly maniuplate the bound data
            List<DataRow> ToDelete = new List<DataRow>();
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Iterate each row in the DataGrid and get the maching selected rows
                foreach (DataRowView row in DataGridSheets.SelectedItems)
                {
                    //Get the first matching data row. We can do that since Sheet Names are unique
                    DataRow dRow = SheetTable.Select("SheetName = '" + row["SheetName"].ToString() + "'").First();
                    //Add the row to the Delete List
                    ToDelete.Add(dRow);
                }

                //Now we iterate the list of items to delte and remove them from the SheetTable Data Table
                foreach (DataRow dRow in ToDelete)
                {
                    //Validate that we information
                    if (dRow != null)
                    {
                        //remove the row from the Data Table
                        SheetTable.Rows.Remove(dRow);
                    }
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Remove Item Error", ex.ToString());
            }
        }
    }
}
