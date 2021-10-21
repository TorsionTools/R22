using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace TorsionTools.WPF
{
    /// <summary>
    /// Interaction logic for ViewFindReplaceWPF.xaml
    /// </summary>
    public partial class ViewFindReplaceWPF : Window
    {
        Document doc;
        DataTable ViewTable = new DataTable();

        public ViewFindReplaceWPF(Document _doc)
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
                ViewTable.Clear();

                //Get the parameter value of any element passed to be used in the Filtered Element Collector
                ParameterValueProvider pvp;

                //Based on the Radio buttons for Name or Number to determine which parameter to use
                if (rdoViewName.IsChecked == true)
                {
                    //Set the parameter to the ElementId of BuiltInParameter for the View Name
                    pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.VIEW_NAME));
                }
                else
                {
                    //Title On Sheet does not have a Built in Paramter enumeration, so we have to get a single view and retrive the paramter
                    //Use a Filtered Collector to get the first view that is not a template of Floor Plan Type
                    View tempView = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).Cast<View>().Where(v => v.IsTemplate == false && v.ViewType == ViewType.FloorPlan).First();
                    //Get the paramter associated with the Title On Sheet
                    Parameter param = tempView.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION);
                    //Set the parameter to the ElementId of BuiltInParameter for the Title On Sheet
                    pvp = new ParameterValueProvider(param.Id);
                }

                //Set how to compare the View Name or Title. This will check to see if it Contains the value passed
                FilterStringRuleEvaluator fsr = new FilterStringContains();

                //Set the value from the "Find" text box. The "true" at the end is for case sensativity
                FilterRule fRule = new FilterStringRule(pvp, fsr, txtFind.Text, true);

                //Create a filter based on the Filter String Rule and the Filter rule to use int he Filtered Element Collector
                ElementParameterFilter filter = new ElementParameterFilter(fRule);

                //Check to see if the Data Table already has columns so we don't need to add them again.
                if (ViewTable.Columns.Count == 0)
                {
                    //Add a new columns to the Data Table with Name and the type of Column
                    ViewTable.Columns.Add(new DataColumn("ViewId", typeof(ElementId)));
                    ViewTable.Columns.Add(new DataColumn("ViewName", typeof(string)));
                    ViewTable.Columns.Add(new DataColumn("TitleOnSheet", typeof(string)));
                    ViewTable.Columns.Add(new DataColumn("Preview", typeof(string)));
                }

                //The Filtered Element Collector will get all Views in the model that contains the "Find" value based on the Prameter Value Provider above
                using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WherePasses(filter))
                {
                    //Loop through each sheet that is returned from the Collector
                    foreach (View view in fec.ToElements())
                    {
                        Parameter viewTitle = view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION);
                        //Based on the Radio buttons for Name or Number to determine which parameter to replace the values for the Preview
                        if (rdoViewName.IsChecked == true)
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the View Name
                            ViewTable.Rows.Add(view.Id, view.Name, viewTitle.AsString(), view.Name.Replace(txtFind.Text, txtReplace.Text));
                        }
                        else
                        {
                            //Create a new row in the Data Table and add the sheet information to it when replacing the VIew Title On Sheet
                            ViewTable.Rows.Add(view.Id, view.Name, viewTitle.AsString(), viewTitle.AsString().Replace(txtFind.Text, txtReplace.Text));
                        }
                    }
                }
                //Check to make sure there is at least one View (row) in the DataTable
                if (ViewTable.Rows.Count > 0)
                {
                    //Enable the Replace button
                    btnReplace.IsEnabled = true;
                    //Use the Data Table to set the Data Source of the Data Grid View to display the sheet information
                    DataGridViews.ItemsSource = ViewTable.DefaultView;
                }
                //If no Views were found based on the critera them set the Data Source to null for the Data Grid View
                else
                {
                    //Clear the data from the Data Grid
                    DataGridViews.ItemsSource = null;
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
            // Make sure there are rows in the Data Table to change
            if (ViewTable.Rows.Count > 0)
            {
                //Use a Try block to keep any errors from crashing Revit
                try
                {
                    //Use a transaction to modify the Document Database
                    using (Transaction Trans = new Transaction(doc))
                    {
                        //Start and Name the transaction to show up in the Undo List
                        Trans.Start("View Find Replace");
                        //Iterate through each row to rename as needed
                        foreach (DataRow row in ViewTable.Rows)
                        {
                            //Cast the ElmentId from the ViewId column of the data Table
                            View view = doc.GetElement((ElementId)row["ViewId"]) as View;
                            if (rdoViewName.IsChecked == true)
                            {
                                //Change the name
                                view.Name = (string)row["Preview"];
                            }
                            else
                            {
                                //Change the Title on Sheet
                                view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set((string)row["Preview"]);
                            }
                        }
                        //Commit the Transaction to keep the changes
                        Trans.Commit();
                    }
                    //Refresh the Project Browser by getting the DockablePaneId, DockablePane, and "Showing" it
                    DockablePaneId dpId = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
                    DockablePane dP = new DockablePane(dpId);
                    dP.Show();

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

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            //Create a list to hold the rows to delete since we can't directly maniuplate the bound data
            List<DataRow> ToDelete = new List<DataRow>();
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Iterate each row in the DataGrid and get the maching selected rows
                foreach (DataRowView row in DataGridViews.SelectedItems)
                {
                    //Get the first matching data row. We can do that since View Names are unique
                    DataRow dRow = ViewTable.Select("ViewName = '" + row["ViewName"].ToString() + "'").First();
                    //Add the row to the Delete List
                    ToDelete.Add(dRow);
                }

                //Now we iterate the list of items to delte and remove them from the ViewTable Data Table
                foreach (DataRow dRow in ToDelete)
                {
                    //Validate that we information
                    if (dRow != null)
                    {
                        //remove the row from the Data Table
                        ViewTable.Rows.Remove(dRow);
                    }
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Remove Item Error", ex.ToString());
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //Set the Dialog result to false so the calling command knows the form was canceled and no work was done
            DialogResult = false;
            //Close the form
            Close();
        }
    }
}
