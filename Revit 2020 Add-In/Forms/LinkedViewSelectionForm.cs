using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ToolTip = System.Windows.Forms.ToolTip;
using View = Autodesk.Revit.DB.View;

namespace TorsionTools.Forms
{
    public partial class LinkedViewSelectionForm : System.Windows.Forms.Form
    {
        //Set Class Variables
        Document doc;
        Document linkDoc;
        SortedList<string, ElementId> vpTypes = new SortedList<string, ElementId>();

        public LinkedViewSelectionForm(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        //Populate the Links, Title Block and Viewport Type combo boxes
        private void LinkedViewSelectionForm_Load(object sender, EventArgs e)
        {
            ToolTip tTip = new ToolTip();
            tTip.SetToolTip(cboTitleBlock, "Select a Title Block to use for created Sheets.");
            tTip.SetToolTip(cbViewPortTypes, "Select a Viewport type to use for created Views.");
            DataTable dtLinks = new DataTable();
            dtLinks.Columns.Add(new DataColumn("Name", typeof(string)));
            dtLinks.Columns.Add(new DataColumn("Doc", typeof(Document)));
            dtLinks.Rows.Add("<Select Linked Model>", null);

            using (FilteredElementCollector rvtLinks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
            {
                if (rvtLinks.ToElements().Count > 0)
                {
                    foreach (RevitLinkType rvtLink in rvtLinks.ToElements())
                    {
                        if (rvtLink.GetLinkedFileStatus() == LinkedFileStatus.Loaded)
                        {
                            RevitLinkInstance link = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).Where(x => x.GetTypeId() == rvtLink.Id).First() as RevitLinkInstance;
                            dtLinks.Rows.Add(rvtLink.Name, link.GetLinkDocument());
                        }
                    }
                }
            }
            cboLinks.DataSource = dtLinks;
            cboLinks.ValueMember = "Doc";
            cboLinks.DisplayMember = "Name";
            cboLinks.SelectedIndex = 0;

            SortedList<string, Element> titleBlocks = new SortedList<string, Element>();
            foreach (Element tb in Helpers.Collectors.ByCategoryElementType(doc, BuiltInCategory.OST_TitleBlocks))
            {
                titleBlocks.Add(tb.Name, tb);
            }

            if (titleBlocks.Count > 0)
            {
                cboTitleBlock.DataSource = titleBlocks.ToList();
                cboTitleBlock.DisplayMember = "Key";
                cboTitleBlock.ValueMember = "Value";
            }
            else
            {
                titleBlocks.Add("No Title Blocks Found", null);
            }

            foreach (ElementType vp in Helpers.Collectors.ViewportTypes(doc))
            {
                vpTypes.Add(vp.Name, vp.Id);
            }

            cbViewPortTypes.DataSource = vpTypes.ToList();
            cbViewPortTypes.DisplayMember = "Key";
            cbViewPortTypes.ValueMember = "Value";
        }

        //Close the Form
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        //Get the Selected Viewports from the linked model, check to see if the sheet they are on exists and if they are already placed. 
        //If not, create and set the properties of both.
        private void btnOK_Click(object sender, EventArgs e)
        {
            int itemCount = 0;
            bool TitleLocation = true;
            ElementId typeId = (ElementId)cbViewPortTypes.SelectedValue;
            ElementType vpType = doc.GetElement(typeId) as ElementType;
            //Check to see if the Viewport Type has the Show Title property set to Yes, if it is not, we can't calculate its location
            //relative to the viewport to move it to the same location as the Linked Model
            if (vpType.LookupParameter("Show Title").AsInteger() != 1)
            {
                if (TaskDialog.Show("Viewport Show Title", "The Viewport Type selected does not have the 'Show Title' property set to Yes and the View placement may not be as expected.\nWould you like to continue?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogResult.No) == TaskDialogResult.No)
                {
                    return;
                }
                else
                {
                    TitleLocation = false;
                }
            }

            //Use a Transaction Group for multiple transactions to be grouped as one. This enables the creation of sheets and views during the method
            //without throwing an exception that the elements don't exist
            using (TransactionGroup tGroup = new TransactionGroup(doc, "Create Linked Views"))
            {
                tGroup.Start();
                using (Transaction t = new Transaction(doc))
                {
                    ElementId vpTypeId = ((Element)cboTitleBlock.SelectedValue).Id;
                    foreach (DataGridViewRow row in dgvLinkedViews.SelectedRows)
                    {
                        //use a try block to make sure any errors don't crash revit
                        try
                        {
                            t.Start("Create View");
                            string detailNumber = (string)row.Cells[1].Value;
                            ViewFamilyType viewfamilyType = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().FirstOrDefault(x => x.ViewFamily == ViewFamily.Drafting);
                            TextNoteType textnoteType = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().FirstOrDefault();
                            ViewDrafting draftingView = ViewDrafting.Create(doc, viewfamilyType.Id);
                            draftingView.Name = (string)row.Cells[3].Value;
                            draftingView.LookupParameter("Title on Sheet").Set("REFERENCE VIEW - DO NOT PRINT");

                            //Set additional View Parameters based on Firm standards and Project Broswer Sorting templates
                            //if (dView.LookupParameter("Sheet Sort") is Parameter sSort)
                            //{
                            //    sSort.Set("PLANS");
                            //}

                            //Set the Linked View Yes/No Parameter to "Yes" for the Reload Function to Track
                            if (draftingView.LookupParameter("Linked View") is Parameter lView)
                            {
                                //Using 0 or 1 to set Yes (1) / No (0) parameters
                                lView.Set(1);
                            }
                            else
                            {
                                TaskDialog.Show("Missing Parameter", "Linked View Yes/No parameter is missing from View category and cannot continue.");
                                break;
                            }
                            //Set the Linked View GUID parameter to track the view in the Linked model
                            if (draftingView.LookupParameter("Linked View GUID") is Parameter lGUID)
                            {
                                lGUID.Set((string)row.Cells[5].Value);
                            }
                            else
                            {
                                TaskDialog.Show("Missing Parameter", "Linked View GUID Text parameter is missing from View category and cannot continue.");
                                break;
                            }
                            //Set the Link Name parameter to trak which Linked Model it came from.
                            if (draftingView.LookupParameter("Link Name") is Parameter lName)
                            {
                                lName.Set(cboLinks.Text);
                            }
                            else
                            {
                                TaskDialog.Show("Missing Parameter", "Link Name Text parameter is missing from View category and cannot continue.");
                                break;
                            }

                            //Creates one Text Note in the middle of the view to alert users that it is a Linked View and not to print it.
                            TextNote.Create(doc, draftingView.Id, new XYZ(0, 0, 0), "REFERENCE VIEW - DO NOT PRINT", textnoteType.Id);
                            t.Commit();

                            //Check to see if sheet with that number exits in the document
                            t.Start("Create Sheet");
                            ViewSheet sheet = CheckSheet((string)row.Cells[2].Value, vpTypeId);
                            t.Commit();

                            //Place the Drafting View reference on the sheet in the same location as in the Linked Model
                            t.Start("Place View");
                            if (sheet != null)
                            {
                                //Check to see if Viewport can be placed on sheet
                                if (Viewport.CanAddViewToSheet(doc, sheet.Id, draftingView.Id))
                                {
                                    if (CheckViewport(detailNumber, sheet))
                                    {
                                        XYZ labelPoint = (XYZ)row.Cells[7].Value;
                                        Viewport vPort = Viewport.Create(doc, sheet.Id, draftingView.Id, labelPoint);
                                        draftingView.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).Set(detailNumber);

                                        if (typeId != ElementId.InvalidElementId)
                                        {
                                            vPort.ChangeTypeId(typeId);
                                        }

                                        if (TitleLocation)
                                        {
                                            //Get the location of the Viewport and Viewport Label and Move the Viewport to match the Linked Document
                                            ElementTransformUtils.MoveElement(doc, vPort.Id, labelPoint - vPort.GetLabelOutline().MinimumPoint);
                                        }
                                        else
                                        {
                                            //If the Viewport Type does not have the Show Title property set to yes, we can't calculate the location
                                            //and we just place it int he location from the Linked model. This may not be the same location.
                                            ElementTransformUtils.MoveElement(doc, vPort.Id, labelPoint);
                                        }
                                    }
                                    else
                                    {
                                        TaskDialog.Show("Existing Viewport", "Sheet " + sheet.SheetNumber + "-" + sheet.Name + " already contains a Viewport with Detail Number " + detailNumber + ", but detail " + draftingView.Name + " was created.");
                                    }
                                }
                            }
                            t.Commit();
                            itemCount++;

                        }
                        catch (Exception ex)
                        {
                            //This Exception will check if the View already exits in the project
                            if (ex.GetType() == typeof(Autodesk.Revit.Exceptions.ArgumentException))
                            {
                                TaskDialog.Show("Existing View", "View '" + (string)row.Cells[3].Value + "' already exists and will not be created.");
                                if (t.HasStarted())
                                {
                                    t.RollBack();
                                }
                                continue;
                            }
                            else
                            {
                                TaskDialog.Show("Error", ex.ToString());
                                //Check to see if a Transaction is active and roll it back if so
                                if (t.HasStarted())
                                {
                                    t.RollBack();
                                }
                                //check to see if the Group Transaction has started and roll it back if so
                                if (tGroup.HasStarted())
                                {
                                    tGroup.RollBack();
                                }
                            }
                        }
                    }
                }
                //Commit all of the changes from the Transaction group and other transactions
                tGroup.Assimilate();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        //Loads all Viewports from the Linked Model Selection into the DataGridView
        private void cbLinks_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboLinks.SelectedValue != null && cboLinks.SelectedIndex != 0)
            {
                try
                {
                    DataTable dtViews = new DataTable();
                    dtViews.Columns.Add(new DataColumn("Type", typeof(string))); //#0
                    dtViews.Columns.Add(new DataColumn("Detail", typeof(string)));
                    dtViews.Columns.Add(new DataColumn("Sheet", typeof(string)));
                    dtViews.Columns.Add(new DataColumn("Name", typeof(string))); //#3
                    dtViews.Columns.Add(new DataColumn("Title on Sheet", typeof(string)));
                    dtViews.Columns.Add(new DataColumn("GUID", typeof(string)));
                    dtViews.Columns.Add(new DataColumn("View", typeof(View))); //#6
                    dtViews.Columns.Add(new DataColumn("Min", typeof(XYZ)));

                    linkDoc = (Document)cboLinks.SelectedValue;
                    foreach (Viewport vp in Helpers.Collectors.ByCategoryNotElementType(linkDoc, BuiltInCategory.OST_Viewports))
                    {
                        //Check to make sure the viewport type is Showing the Title so we can get the location
                        ElementType viewportType = linkDoc.GetElement(vp.GetTypeId()) as ElementType;
                        if (viewportType.LookupParameter("Show Title").AsInteger() == 1)
                        {
                            if (linkDoc.GetElement(vp.ViewId) is View view)
                            {
                                if (view.ViewType != ViewType.Legend)
                                {
                                    if (vp.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER) is Parameter sNum && vp.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER) is Parameter dNum)
                                    {
                                        if (vp.GetLabelOutline().MinimumPoint is XYZ point)
                                        {
                                            dtViews.Rows.Add(view.ViewType.ToString(), dNum.AsString(), sNum.AsString(), view.Name, view.LookupParameter("Title on Sheet").AsString(), vp.UniqueId, view, vp.GetLabelOutline().MinimumPoint);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    dgvLinkedViews.DataSource = dtViews;
                    dgvLinkedViews.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvLinkedViews.Columns[5].Visible = false;
                    dgvLinkedViews.Columns[6].Visible = false;
                    dgvLinkedViews.Columns[7].Visible = false;
                    dgvLinkedViews.ClearSelection();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error",ex.ToString());
                }
            }
        }

        //Search the list of views via the DataTable Row Filter
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (dgvLinkedViews.DataSource != null)
            {
                DataTable dtDGV = (DataTable)dgvLinkedViews.DataSource;
                dtDGV.DefaultView.RowFilter = string.Format("Name like '%{0}%' OR [Title on Sheet] like '%{0}%' OR Sheet like '%{0}%' OR Detail like '%{0}%'", txtSearch.Text);
            }
        }

        //Checks to see if a viewport with a specific detail number already exists
        private bool CheckViewport(string _detailNumber, ViewSheet _vs)
        {
            ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.VIEWPORT_DETAIL_NUMBER));
            FilterStringRuleEvaluator fsr = new FilterStringEquals();
            FilterRule fRule = new FilterStringRule(pvp, fsr, _detailNumber, true);
            ElementParameterFilter filter = new ElementParameterFilter(fRule);

            if (new FilteredElementCollector(doc, _vs.Id).OfCategory(BuiltInCategory.OST_Viewports).WherePasses(filter).FirstOrDefault() is Viewport vp)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Check to see if a Sheet Number already exists in the project and create it if not
        private ViewSheet CheckSheet(string _sheetNumber, ElementId _vpTypeId)
        {
            try
            {
                ViewSheet sheet = null;
                ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NUMBER));
                FilterStringRuleEvaluator fsr = new FilterStringEquals();
                FilterRule fRule = new FilterStringRule(pvp, fsr, _sheetNumber, true);
                ElementParameterFilter filter = new ElementParameterFilter(fRule);

                if (new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter).FirstOrDefault() is ViewSheet vs)
                {
                    sheet = vs;
                }
                else
                {
                    sheet = ViewSheet.Create(doc, _vpTypeId);
                    sheet.Name = "DO NOT PRINT";
                    sheet.SheetNumber = _sheetNumber;

                    //Set additional View Parameters based on Firm standards and Project Broswer Sorting templates
                    //sheet.LookupParameter("Sheet Sort").Set("PLANS");

                    //Set the Appears In Sheet List to False so duplicate sheets do not appear in Sheet Index Schedule
                    sheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED).Set(0);
                }
                return sheet;
            }
            catch
            {
                return null;
            }
        }
    }
}
