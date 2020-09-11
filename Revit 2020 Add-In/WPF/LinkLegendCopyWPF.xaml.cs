using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TorsionTools.WPF
{
    /// <summary>
    /// Interaction logic for LinkViewCopyWPF.xaml
    /// </summary>
    public partial class LinkLegendCopyWPF : Window
    {
        Document doc;
        View TempLegend;
        List<ElementIdName> LinkedLegends = new List<ElementIdName>();

        public LinkLegendCopyWPF(Document _doc, View _temp)
        {
            InitializeComponent();
            doc = _doc;
            TempLegend = _temp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Create a DataTable to hold the Revit Link information
            DataTable dtLinkedDocuments = new DataTable();

            dtLinkedDocuments.Columns.Add(new DataColumn("Name", typeof(string)));
            dtLinkedDocuments.Columns.Add(new DataColumn("Document", typeof(Document)));
            //Add a row inthe DataTable for when no Link is Selected and give it a null value

            //Get all of the Link Types in the Current Document
            using (FilteredElementCollector LinkedDocumentsCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
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
                            RevitLinkInstance LinkedDocumentInstance = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).Where(x => x.GetTypeId() == LinkedDocumentType.Id).First() as RevitLinkInstance;
                            //Add the Link Name and Link Document to the Data Table
                            dtLinkedDocuments.Rows.Add(LinkedDocumentType.Name, LinkedDocumentInstance.GetLinkDocument());
                        }
                    }
                }
                //Tell the user when there aren't any linked document and close the form
                else
                {
                    TaskDialog.Show("No Revit Link", "Document does not contain any Revit Links.");
                    DialogResult = false;
                    Close();
                }
            }
            //Set the ComboBox data source to the Data Table
            ComboBoxLinks.ItemsSource = dtLinkedDocuments.DefaultView;
            ComboBoxLinks.SelectedValuePath = "Document";
            ComboBoxLinks.DisplayMemberPath = "Name";
            //Set the Value member to the Document column of the Data Table
        }

        private void ComboBoxRevitLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxLinks.SelectedItem is DataRowView row)
            {
                //Clear the Items in the List View so they aren't added to existing ones when the Link is changed
                ListViewLinkedViews.Items.Clear();
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
                                if (LinkView.ViewType == ViewType.Legend && !LinkView.IsTemplate)
                                {
                                    {
                                        //Create a list View item set to the Linked View name
                                        LinkedLegends.Add(new ElementIdName() { Name = LinkView.Name, ElemId = LinkView.Id, Check = false });
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

                ListViewLinkedViews.ItemsSource = LinkedLegends;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ElementIdName> TempViews = ListViewLinkedViews.SelectedItems.Cast<ElementIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ElementIdName View in TempViews)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ElementIdName item = LinkedLegends.Find(x => x.ElemId == View.ElemId);
                //set the bool Check variable to true or checked
                item.Check = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ElementIdName> TempViews = ListViewLinkedViews.SelectedItems.Cast<ElementIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ElementIdName View in TempViews)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ElementIdName item = LinkedLegends.Find(x => x.ElemId == View.ElemId);
                //set the bool Check variable to true or checked
                item.Check = false;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnCopyViews_Click(object sender, RoutedEventArgs e)
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
                using (Transaction trans = new Transaction(doc))
                {
                    //Start the transaction and give it a name for the Undo / Redo list
                    trans.Start("Copy Linked Legends");
                    //Loop through each of the Checked Items (views) in the List view to copy
                    foreach (ElementIdName Legend in LinkedLegends)
                    {
                        if (Legend.Check)
                        {
                            //Cast the View from the List View Item Tag property set previously
                            View view = (View)linkDoc.GetElement(Legend.ElemId);
                            //Use a Element Collector to get ALL element Ids in the Legend using the Linked view.Id modifier and put them into a list
                            ICollection<ElementId> elemIds = new FilteredElementCollector(linkDoc, view.Id).ToElementIds();
                            //Use the Trsnform Utilities Class with the CopyElements method to copy the Legend View and elements into a new legend in the current document
                            //Although we supply the "TempLegend" view for this method to copy the elements into, the Linnked View ElemExt element is also copied and actually
                            //creates an entirely new Legend WITHOUT copying any elements into the "TempLegend" view.
                            ElementTransformUtils.CopyElements(view, elemIds, TempLegend, Transform.Identity, options);
                            //Add 1 to the counte integer we will use to tell the user how many legends were copied
                            count++;
                        }
                    }
                    //Commit the transaction to save the changed to the document
                    trans.Commit();
                }
                //Tell the user how many legends were copied
                TaskDialog.Show("Copy Legends", "Copied " + count + " Legends");
                //Set the Dialog Result so that the calling Method resturns the correct result and the changes stick
                DialogResult = true;
                //Close the form
                Close();
            }
            //Display a message that an error occured when trying to copy the legend, set the Dialog Result, and close the form.
            catch (Exception ex)
            {
                TaskDialog.Show("Copy Legends Error", ex.ToString());
                DialogResult = false;
                Close();
            }
        }
    }
}
