using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using View = Autodesk.Revit.DB.View;

namespace Revit_2020_Add_In.Forms
{
    public partial class ViewLegendCopyForm : System.Windows.Forms.Form
    {
        //Create a Class variable for the Document
        Document Doc;
        //Create a Class variable for the temporary Legend used by the Copy Utility
        View TempLegend;
        
        public ViewLegendCopyForm(Document _doc, View _TempLegend)
        {
            InitializeComponent();
            //set the Doc variable with the one passed to the Form
            Doc = _doc;
            //set the TempLegen varaible with the one pass to the From
            TempLegend = _TempLegend;
        }

        private void ViewLegendCopyForm_Load(object sender, EventArgs e)
        {
            //Create a DataTable to hold the Revit Link information
            DataTable dtLinkedDocuments = new DataTable();
            dtLinkedDocuments.Columns.Add(new DataColumn("Name", typeof(string)));
            dtLinkedDocuments.Columns.Add(new DataColumn("Document", typeof(Document)));
            //Add a row inthe DataTable for when no Link is Selected and give it a null value
            dtLinkedDocuments.Rows.Add("--NONE--", null);

            //Get all of the Link Types in the Current Document
            using (FilteredElementCollector LinkedDocumentsCollector = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
            {
                //Check to make sure there is at least one Link
                if (LinkedDocumentsCollector.ToElements().Count > 0)
                {
                    //Since we
                    foreach (RevitLinkType LinkedDocumentType in LinkedDocumentsCollector.ToElements())
                    {
                        //We only wan tto check Links that are currently Loaded
                        if (LinkedDocumentType.GetLinkedFileStatus() == LinkedFileStatus.Loaded)
                        {
                            //Iterate through the Link Instances in the Document and match the first one with the Link Type. This is because you need the Link Instance to get the Link Document, not the Link Type
                            RevitLinkInstance LinkedDocumentInstance = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).Where(x => x.GetTypeId() == LinkedDocumentType.Id).First() as RevitLinkInstance;
                            //Add the Link Name and Link Document to the Data Table
                            dtLinkedDocuments.Rows.Add(LinkedDocumentType.Name, LinkedDocumentInstance.GetLinkDocument());
                        }
                    }
                }
                //Tell the user when there aren't any linked document and close the form
                else
                {
                    TaskDialog.Show("No Revit Link", "Document does not contain any Revit Links.");
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            //Set the ComboBox data source to the Data Table
            ComboBoxLinks.DataSource = dtLinkedDocuments;
            //Set the Value member to the Document column of the Data Table
            ComboBoxLinks.ValueMember = "Document";
            //Set the Display (Visible) member to the Name column of the Data Table
            ComboBoxLinks.DisplayMember = "Name";
            //Set the selected Index to 0 or the Row we manually added above
            ComboBoxLinks.SelectedIndex = 0;
        }

        //This Method will get all of the Legends from the Linked Document when the Combo Box is changed and a New Link is selected
        private void ComboBoxLinks_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Clear the Items in the List View so they aren't added to existing ones when the Link is changed
            ListViewLegends.Items.Clear();
            //Check to make sure the Selected value is not null, which is the Value for the row we added
            if (ComboBoxLinks.SelectedValue != null)
            {
                //Get the Link Document by casting the Selected Value of the Combo Box to a Document
                Document linkDoc = (Document)ComboBoxLinks.SelectedValue;
                
                //Use a Try block to keep any errors from crashing Revit
                try
                {
                    //Use a Filtered Element Collector to get all of the Views in the Linked Document
                    using (FilteredElementCollector LinkViews = new FilteredElementCollector(linkDoc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType())
                    {
                        //Loop through each view in the Element Collector
                        foreach (View LinkView in LinkViews.ToElements())
                        {
                            //Check to see if the ViewTypes is a Legend and make sure it isn't a Legend View Template
                            if (LinkView.ViewType == ViewType.Legend  && !LinkView.IsTemplate)
                            {
                                {
                                    //Create a list View item set to the Linked View name
                                    ListViewItem item = ListViewLegends.Items.Add(LinkView.Name);
                                    //Set the List View Item's Tag (Object Container) to the View element
                                    item.Tag = LinkView;
                                }
                            }
                        }
                    }
                }
                //Display a message that an error occured when trying to change the Link Selection
                catch (Exception ex)
                {
                    TaskDialog.Show("Selection Change Error", ex.ToString());
                }
            }
        }

        //This is the Method that is called when you press the "Close" button
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            //Set the Dialogt Result to make sure no changes are made to the document from the Command
            DialogResult = DialogResult.Cancel;
            //Closes the Form
            Close();
        }

        //This is the Method called when you press the "Copy" button
        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Integer variable to count the number of Legends transferred
                int count = 0;
                //Get the Link Document by casting the Selected Value of the Combo Box to a Document
                Document linkDoc = (Document)ComboBoxLinks.SelectedValue;
                //Use CopyPasteOptions to control the behavior of like elements on copy "Use Current Project or Import Types"
                CopyPasteOptions options = new CopyPasteOptions();
                //Set the Copy Paste Optiosn by useing a Copy Handler class in the Functions Class
                options.SetDuplicateTypeNamesHandler(new Helpers.CopyHandler());

                //Use a Transaction for the Document you are pasting INTO to copy the Legends into
                using (Transaction trans = new Transaction(Doc))
                {
                    //Start the transaction and give it a name for the Undo / Redo list
                    trans.Start("Copy Legends");
                    //Loop through each of the Checked Items (views) in the List view to copy
                    foreach (ListViewItem item in ListViewLegends.CheckedItems)
                    {
                        //Cast the View from the List View Item Tag property set previously
                        View view = (View)item.Tag;
                        //Use a Element Collector to get ALL element Ids in the Legend using the Linked view.Id modifier and put them into a list
                        ICollection<ElementId> elemIds = new FilteredElementCollector(linkDoc, view.Id).ToElementIds();
                        //Use the Trsnform Utilities Class with the CopyElements method to copy the Legend View and elements into a new legend in the current document
                        //Although we supply the "TempLegend" view for this method to copy the elements into, the Linnked View ElemExt element is also copied and actually
                        //creates an entirely new Legend WITHOUT copying any elements into the "TempLegend" view.
                        ElementTransformUtils.CopyElements(view, elemIds, TempLegend, Transform.Identity, options);
                        //Add 1 to the counte integer we will use to tell the user how many legends were copied
                        count++;
                    }
                    //Commit the transaction to save the changed to the document
                    trans.Commit();
                }
                //Tell the user how many legends were copied
                TaskDialog.Show("Copy Legends", "Copied " + count + " Legends");
                //Set the Dialog Result so that the calling Method resturns the correct result and the changes stick
                DialogResult = DialogResult.OK;
                //Close the form
                Close();
            }
            //Display a message that an error occured when trying to copy the legend, set the Dialog Result, and close the form.
            catch (Exception ex)
            {
                TaskDialog.Show("Copy Legends Error", ex.ToString());
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
