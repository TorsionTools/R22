using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
using View = Autodesk.Revit.DB.View;

namespace Revit_2020_Add_In.Forms
{
    public partial class WarningsForm : Form
    {
        //Create global variables
        DataTable dtWarnings = new DataTable();
        Document doc;
        UIApplication uiApp;

        public WarningsForm(UIApplication _uiapp)
        {
            InitializeComponent();
            //Set the global Document variable from the UIApplicaiton variable passed to the form
            doc = _uiapp.ActiveUIDocument.Document;
            //Set the global UIApplicaiton variable (used for selection later)
            uiApp = _uiapp;
        }

        private void WarningsForm_Load(object sender, EventArgs e)
        {
            //Display project informaiton on the form for reference
            lblDoc.Text = doc.ProjectInformation.ClientName + " - " + doc.ProjectInformation.Name;
            //Call the SetData method to get the Warning Information
            SetData(doc);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Close the Form
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear the Datatable contents
            dtWarnings.Clear();
            //Runs the SetData method to get latest information
            SetData(doc);
        }

        //This is the primary method for gathering the Warning information from the Document
        private void SetData(Document doc)
        {
            //Create a new List to hold the Warning Type names
            List<string> warningType = new List<string>();
            //Add the first item to the list so it can be controlled
            warningType.Add("--NONE--");

            //Check to see if the DataTable has had columns created not. If not (first time) then create olumns with the correct type of storage element
            if (dtWarnings.Columns.Count == 0)
            {
                //This is a bit redundent because they are all "warnings" as "errors" can't be ignored, but if you use custom error handlers, this may be more useful
                dtWarnings.Columns.Add(new DataColumn("Severity", typeof(FailureSeverity)));
                dtWarnings.Columns.Add(new DataColumn("Description", typeof(string)));
                dtWarnings.Columns.Add(new DataColumn("Element Ids", typeof(ICollection<ElementId>)));
                dtWarnings.Columns.Add(new DataColumn("Name", typeof(string)));
                dtWarnings.Columns.Add(new DataColumn("Creator", typeof(string)));
                dtWarnings.Columns.Add(new DataColumn("Phase Created", typeof(string)));
                dtWarnings.Columns.Add(new DataColumn("Phase Demo'd", typeof(string)));
            }
            //Use these two parameters in case it is not a workshared project
            string elemName = "None";
            string userId = "None";

            //Loop through every warning in the document
            foreach (FailureMessage warning in doc.GetWarnings())
            {
                //Use these two parameters when the elements cannot have a phase create / demolished
                string pCreate = "";
                string pDemo = "";

                //Set Issue Description to a string variable
                string description = warning.GetDescriptionText();

                //Check to see if the Warning Type list already has this Warning Type in it, and add it if not
                if (!warningType.Contains(description))
                {
                    warningType.Add(description);
                }

                //Get all of the Element Ids of the elements associated with the warning
                ICollection<ElementId> failingElements = warning.GetFailingElements();

                //This is a bit redundent because they are all "warnings" as "errors" can't be ignored, but if you use custom error handlers, this may be more useful
                FailureSeverity severity = warning.GetSeverity();

                //Check to make sure there are Element Ids for the elements. Some warning types (MECH Systems) do not always provide Element Ids
                if (failingElements.OfType<ElementId>().FirstOrDefault() is ElementId first)
                {
                    //Get the First element of the warning elements in the Document 
                    Element elem = doc.GetElement(first);
                    //Set the parameter to the actual name instead of "None" from above
                    elemName = elem.Name;
                    //Checks to see if the Element has any phases associated with it and sets the pCreate and pDemo variables previousy defined
                    if (elem.HasPhases())
                    {
                        if (elem.CreatedPhaseId != ElementId.InvalidElementId)
                        {
                            pCreate = doc.GetElement(elem.CreatedPhaseId).Name;
                        }
                        if (elem.DemolishedPhaseId != ElementId.InvalidElementId)
                        {
                            pDemo = doc.GetElement(elem.DemolishedPhaseId).Name;
                        }
                    }
                    //Checks to see if the Document has Worksharing enables and gets the User informaiton associated with the creation of the element for refernce
                    if (doc.IsWorkshared)
                    {
                        userId = WorksharingUtils.GetWorksharingTooltipInfo(doc, first).Creator;
                    }
                }

                //Add a reow to the DataTable with all of the informaiton for the warning
                dtWarnings.Rows.Add(severity, description, failingElements, elemName, userId, pCreate, pDemo);
            }

            //Set the DataGridView's data source to the DataTable
            dgWarnings.DataSource = dtWarnings;
            //Change the AutoSize mode to fill the width of the DataGridView as it resizes
            dgWarnings.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //Changes the text to include the number of Wanrings for reference to the User
            dgWarnings.Columns[1].HeaderText = "Description (" + dtWarnings.Rows.Count.ToString() + ")";
            dgWarnings.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Hides the column that contains the Element Ids to keep the DataGridView clean
            dgWarnings.Columns[2].Visible = false;
            dgWarnings.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgWarnings.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgWarnings.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgWarnings.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //Set the DataGridView to sort based on the Description 
            dgWarnings.Sort(dgWarnings.Columns[1], System.ComponentModel.ListSortDirection.Ascending);
            //Clears the auto selection of any rows in the DataGridivew so no elements are selected initially
            dgWarnings.ClearSelection();

            //Clears the Warning Type combo box of all items
            cboWarningType.DataSource = null;
            //Reset the Warning Type combo box with the List of warnings Types
            cboWarningType.DataSource = warningType.ToList();
        }

        //This method is activated by double clicking a row in teh DataGridView so show / zoom too the elements associated with the warning
        private void dgWarnings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //create a RevitCommandId to utilize Revit's built in SelectionBox feature for our selected ElementIds
            RevitCommandId rvtId = RevitCommandId.LookupPostableCommandId(PostableCommand.SelectionBox);
            //Get the ElementIds associated with the Warning from the DataGridView
            ICollection<ElementId> eId = (ICollection<ElementId>)dgWarnings.Rows[e.RowIndex].Cells[2].Value;
            //Don't know why, but without creating another ICollection and setting it, the SelectionBox will not work
            ICollection<ElementId> iDs = eId;
            //Use a try statment so an error won't crash revit
            try
            {
                //Get the first element in the Document from the list of Warning Elements
                if (doc.GetElement(iDs.FirstOrDefault()) is Element elem)
                {
                    //Use this check to see if it is a view item like a Room Tag
                    if (elem.ViewSpecific)
                    {
                        //If it is a view item, then open and change to that view
                        View view = doc.GetElement(elem.OwnerViewId) as View;
                        uiApp.ActiveUIDocument.ActiveView = view;
                    }
                    else
                    {
                        //If it is a 3D element, then provide a selection box around all elements for that warning
                        uiApp.PostCommand(rvtId);
                    }
                    //Sets the current selection to the Warning Element(s)
                    uiApp.ActiveUIDocument.Selection.SetElementIds(iDs);
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Cell Double Click Error", ex.ToString());
            }
        }

        //This is the Method that creates the Selection box around the elements for the selected row
        private void btnSelectionBox_Click(object sender, EventArgs e)
        {
            //Check to make sure that only 1 row is selected
            if (dgWarnings.SelectedRows.Count == 1)
            {
                //create a RevitCommandId to utilize Revit's built in SelectionBox feature for our selected ElementIds
                RevitCommandId rvtId = RevitCommandId.LookupPostableCommandId(PostableCommand.SelectionBox);
                //Get the ElementIds associated from the DataGridView
                ICollection<ElementId> eId = new List<ElementId>();

                //Use a try statment so an error won't crash revit
                try
                {
                    //Get the Element Ids for the selected row int he DataGridView
                    eId = (ICollection<ElementId>)dgWarnings.SelectedRows[0].Cells[2].Value;
                    //Don't know why, but without creating another ICollection and setting it, the SelectionBox will not work
                    ICollection<ElementId> iDs = eId;
                    //Sets the current selection to the Warning Element(s)
                    uiApp.ActiveUIDocument.Selection.SetElementIds(iDs);
                    //Creates a selection box around all elements for that warning
                    uiApp.PostCommand(rvtId);
                }
                //Catch any errors and display a Dialog with the informaiton
                catch (Exception ex)
                {
                    TaskDialog.Show("Selection Box Error", ex.ToString());
                }
            }
            //Tell the user they can only have one row selected
            else
            {
                TaskDialog.Show("Single Selection", "Select single row for Selection Box");
            }
        }

        //SIngle clicking the row selects the elements for the selected warning int he model
        private void dgWarnings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to make sure the row selected isnt the header row or on DataBinding
            if (e.RowIndex != -1)
            {
                //Use a try statment so an error won't crash revit
                try
                {
                    //Get the ElementIds associated with the Warning from the DataGridView
                    ICollection<ElementId> eId = (ICollection<ElementId>)dgWarnings.Rows[e.RowIndex].Cells[2].Value;
                    //Don't know why, but without creating another ICollection and setting it, the SelectionBox will not work
                    ICollection<ElementId> iDs = eId;

                    //Sets the current selection to the Warning Element(s)
                    uiApp.ActiveUIDocument.Selection.SetElementIds(iDs);
                }
                //Catch any errors and display a Dialog with the informaiton
                catch (Exception ex)
                {
                    TaskDialog.Show("Cell Click Error", ex.ToString());
                }
            }
        }

        //This method is to filter the DataGridView based on the Selected Warning Type
        private void cboWarningType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Use a try statment so an error won't crash revit
            try
            {
                //Check to make sure it the index of the Combo box is not 0, becasue we added "None" to the combo box on form load
                if (cboWarningType.SelectedIndex != 0)
                {
                    //Since we bound the DataGridvView to the DataTable, we filter the DataTable which then changes the values available in the DataGridView. Also need to account for apostrophe with replacing it for escape
                    dtWarnings.DefaultView.RowFilter = string.Format("Description = '{0}'", ((string)cboWarningType.SelectedValue).Replace("'", "''"));
                }
                else
                {
                    //Clears the filter if it is index 0
                    dtWarnings.DefaultView.RowFilter = null;
                }
            }
            //Catch any errors and display a Dialog with the informaiton
            catch (Exception ex)
            {
                TaskDialog.Show("Cell Click Error", ex.ToString());
            }
        }

        //This method will isolate all of the elements associated with the visible rows in the DataGridView
        private void btnIsolate_Click(object sender, EventArgs e)
        {
            //Get the current active view
            View view = doc.ActiveView;
            //Get the UiDocument from the Document
            UIDocument uiDoc = new UIDocument(doc);
            //Create a list to hold all of the Element Ids
            List<ElementId> ids = new List<ElementId>();
            //Loop through all rows visible int he DataGridView
            foreach (DataGridViewRow row in dgWarnings.Rows)
            {
                //Get the ElementIds associated with the Warning from the DataGridView
                ICollection<ElementId> rIds = (ICollection<ElementId>)row.Cells[2].Value;
                //Loop through each Element Id in the Element Ids from the Warning
                foreach (ElementId id in rIds)
                {
                    //Add each Element Id to the List created above
                    ids.Add(id);
                }
            }

            //Islocate each Element by Element Id in the Active view
            view.IsolateElementsTemporary(ids);
            //Refersh the view to make it update and show the isolated elemments
            uiDoc.RefreshActiveView();

        }
    }
}
