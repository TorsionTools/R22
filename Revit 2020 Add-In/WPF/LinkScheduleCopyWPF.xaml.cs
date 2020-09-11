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
    public partial class LinkScheduleCopyWPF : Window
    {
        Document doc;
        List<ElementIdName> LinkedSchedules = new List<ElementIdName>();

        public LinkScheduleCopyWPF(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
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
                        using (FilteredElementCollector LinkSchedules = new FilteredElementCollector(linkDoc).OfCategory(BuiltInCategory.OST_Schedules))
                        {
                            //Loop through each view in the Element Collector
                            foreach (ViewSchedule LinkSchedule in LinkSchedules.ToElements())
                            {
                                //Check to see if the ViewTypes is a Legend and make sure it isn't a Legend View Template
                                if (!LinkSchedule.IsTitleblockRevisionSchedule)
                                {
                                    {
                                        //Create a list View item set to the Linked View name
                                        LinkedSchedules.Add(new ElementIdName() { Name = LinkSchedule.Name, ElemId = LinkSchedule.Id, Check = false });
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
                ListViewLinkedViews.ItemsSource = LinkedSchedules;
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
                ElementIdName item = LinkedSchedules.Find(x => x.ElemId == View.ElemId);
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
                ElementIdName item = LinkedSchedules.Find(x => x.ElemId == View.ElemId);
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
                //Create List of Schedule Element Ids to Copy
                List<ElementId> ScheduleIds = new List<ElementId>();

                foreach (ElementIdName Schedule in LinkedSchedules)
                {
                    //Test and make sure the "Check" bool variable is true
                    if (Schedule.Check)
                    {
                        //Use a helper method to see if a Schedule with the same name already exists
                        if (Helpers.Collectors.CheckSchedule(doc, Schedule.Name))
                        {
                            //Add the Schedule Element Id to the List of items to copy
                            ScheduleIds.Add(Schedule.ElemId);
                            //Add 1 to the counte integer we will use to tell the user how many legends were copied
                            count++;
                        }
                        else
                        {
                            //If a Schedule with the same name does exist, ask the user what they want to do
                            if (TaskDialog.Show("Schedule Exists", "Schedule " + Schedule.Name + " already exits in the current Document.\n\nWould you like to copy the Schedule?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogResult.No) == TaskDialogResult.Yes)
                            {
                                //If the User wants to copy it anyway, add the Id to the list
                                ScheduleIds.Add(Schedule.ElemId);
                                //Add 1 to the counte integer we will use to tell the user how many legends were copied
                                count++;
                            }
                        }
                    }
                }

                //Check to make sure there is a least one Schedule to be copied
                if (count > 0)
                {
                    //Use a Transaction for the Document you are pasting INTO to copy the Schedules into
                    using (Transaction trans = new Transaction(doc))
                    {
                        //Start the transaction and give it a name for the Undo / Redo list
                        trans.Start("Copy Linked Schedules");
                        //Use the Transform Utilties Class with the CopyElemens method to copy the Schedules from the Linked Document
                        //to the Current Document. This can be done one time with ALL Schedule Ids and doesn't need to be part of a loop
                        ElementTransformUtils.CopyElements(linkDoc, ScheduleIds, doc, Transform.Identity, options);
                        //Commit the transaction to save the changed to the document
                        trans.Commit();
                    }
                    //Tell the user how many Schedules were copied
                    TaskDialog.Show("Copy Schedules", "Copied " + count + " Schedules");
                    //Set the Dialog Result so that the calling Method resturns the correct result and the changes stick
                    DialogResult = true;
                    //Close the form
                    Close();
                }
            }
            //Display a message that an error occured when trying to copy the legend, set the Dialog Result, and close the form.
            catch (Exception ex)
            {
                TaskDialog.Show("Copy Schedules Error", ex.ToString());
                DialogResult = false;
                Close();
            }
        }
    }
}
