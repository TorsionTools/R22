using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Revit_2020_Add_In.WPF
{
    public partial class ViewDeleteUnplacedWPF : Window
    {
        //Class Level Variables
        Document doc;
        //Containers for each View Type
        List<ElementIdName> Views = new List<ElementIdName>();
        List<ElementIdName> ViewTemplates = new List<ElementIdName>();
        List<ElementIdName> Schedules = new List<ElementIdName>();
        List<ElementIdName> Legends = new List<ElementIdName>();

        //Set the class Document variable to the document passed to the form
        public ViewDeleteUnplacedWPF(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Lists to store the ElementId of Views placed on sheets
            List<ElementId> LegendsPlacedIds = new List<ElementId>();
            List<ElementId> SchedulesPlacedIds = new List<ElementId>();
            List<ElementId> ViewTemplatesUsedIds = new List<ElementId>();
            List<View> RootViews = new List<View>();

            try
            {
                //Get all sheets using a Filtered Element Collector
                foreach (ViewSheet sheet in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)))
                {
                    //Get each view on each sheet and store them in the applicable list
                    foreach (ElementId elementId in sheet.GetAllPlacedViews())
                    {
                        //Cast the Element to a View
                        View v = doc.GetElement(elementId) as View;
                        if (v.ViewType == ViewType.Legend)
                        {
                            //For items like Legends and Schedules, we just need one instance so check if the list already
                            //contains the Element Id
                            if (!LegendsPlacedIds.Contains(elementId))
                            {
                                //If the Legend container List doesn't already contain the Element Id, add it
                                LegendsPlacedIds.Add(elementId);
                            }
                        }
                    }
                }

                //Collect all instances of schedules placed on a sheet
                foreach (ScheduleSheetInstance scheduleInstance in new FilteredElementCollector(doc).OfClass(typeof(ScheduleSheetInstance)).ToElements())
                {
                    //Check to see if the container list already has the Schedule Id
                    //Note that you need to get the ScheduleId from the Schedule Instance
                    if (!SchedulesPlacedIds.Contains(scheduleInstance.ScheduleId))
                    {
                        //If the Schedule isn't in the list, add the Element Id for it to the palced List
                        SchedulesPlacedIds.Add(scheduleInstance.ScheduleId);
                    }
                }

                //Collect all Views in the Document and store them in an IList so we can iterate them multiple times
                IList<Element> AllDocumentViews = Helpers.Collectors.ByCategory(doc, BuiltInCategory.OST_Views);
                //Iterate All views to check certain criteria of each view
                foreach (View view in AllDocumentViews)
                {
                    //Check to make sure the view is Not a View Template
                    if (!view.IsTemplate)
                    {
                        //Check to see if the view has a ViewTemplate applied
                        if (view.ViewTemplateId != ElementId.InvalidElementId)
                        {
                            //Check to see if the View Template placed list already has the Element Id
                            if (!ViewTemplatesUsedIds.Contains(view.ViewTemplateId))
                            {
                                //if it does have a view template, add it to the list
                                ViewTemplatesUsedIds.Add(view.ViewTemplateId);
                            }
                        }
                        //Check to make sure the view is not a Legend
                        if (view.ViewType != ViewType.Legend)
                        {
                            //Check to see if the View has a viewport Sheet name or Number parameter
                            if (string.IsNullOrEmpty(view.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NAME).AsString()) || string.IsNullOrEmpty(view.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString()))
                            {
                                //Need to make sure this view isn't a parent view with dependents. Often the dependent views are placed, but if you delete
                                //the parent, the dependents are removed as well
                                if (view.GetDependentViewIds().Count == 0)
                                {
                                    //Add the view to the Master Container with the Custom ElementIdName class
                                    Views.Add(new ElementIdName() { Check = false, Name = view.Name, ElemId = view.Id });
                                }
                                else
                                {
                                    //Store all views that have dependents to check and see if they too can be deleted
                                    RootViews.Add(view);
                                }
                            }
                        }
                        else
                        {
                            //If it is a legend, check to see if it is placed
                            if (!LegendsPlacedIds.Contains(view.Id))
                            {
                                //Add it to the list if not place
                                Legends.Add(new ElementIdName() { Check = false, Name = view.Name, ElemId = view.Id });
                            }
                        }
                    }
                }

                //We have to iterate the views again after we have caught all the applied View templates above
                //to make sure we know which are not being used
                foreach (View view in AllDocumentViews)
                {
                    //Make sure we only get View Templates
                    if (view.IsTemplate)
                    {
                        //If it is a view template, check to see if it is in the Placed list
                        if (!ViewTemplatesUsedIds.Contains(view.Id))
                        {
                            //If not, add it to the master container with the Custom ElementIdName class
                            ViewTemplates.Add(new ElementIdName() { Check = false, Name = view.Name, ElemId = view.Id });
                        }
                    }
                }

                //Iterate through all Root views that had dependents
                foreach (View RootView in RootViews)
                {
                    //Use a bool to be the switch if any dependent is placed
                    bool placed = false;
                    //Iterate each ViewId of each dependent 
                    foreach (ElementId viewId in RootView.GetDependentViewIds())
                    {
                        //Use the Element Id to get the view associated
                        View view = doc.GetElement(viewId) as View;
                        //Check to see if the View has a viewport Sheet name or Number parameter
                        if (!string.IsNullOrEmpty(view.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NAME).AsString()) || !string.IsNullOrEmpty(view.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString()))
                        {
                            //If ANY of the views along the way have this information, we don't want to delete the Root view 
                            //so make the bool true
                            placed = true;
                        }
                    }
                    //Check the bool variable and add the Root view only if none of the dependents were placed
                    if (!placed)
                    {
                        //Add the view to the Master Container with the Custom ElementIdName class
                        Views.Add(new ElementIdName() { Check = false, Name = RootView.Name, ElemId = RootView.Id });
                    }
                }

                //Collect all Schedules in the project
                IList<Element> AllSchedules = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).ToElements();
                foreach (ViewSchedule Schedule in AllSchedules)
                {
                    //check to make sure it is not a View Template schedule
                    if (!Schedule.IsTemplate)
                    {
                        //Check to see if the view has a ViewTemplate applied
                        if (Schedule.ViewTemplateId != ElementId.InvalidElementId)
                        {
                            //Check to see if the View Template placed list already has the Element Id
                            if (!ViewTemplatesUsedIds.Contains(Schedule.ViewTemplateId))
                            {
                                //if it does have a view template, add it to the list
                                ViewTemplatesUsedIds.Add(Schedule.ViewTemplateId);
                            }
                        }
                        //Make sure it is not a built in Title Block Revision Schedule becasue these can't be deleted
                        if (!Schedule.IsTitleblockRevisionSchedule)
                        {
                            //Check to see if the Placed list contains the schedule
                            if (!SchedulesPlacedIds.Contains(Schedule.Id))
                            {
                                //If not, add it to the master container with the Custom ElementIdName class
                                Schedules.Add(new ElementIdName() { Check = false, Name = Schedule.Name, ElemId = Schedule.Id});
                            }
                        }
                    }
                }

                //We have to iterate the views again after we have caught all the applied View templates above
                //to make sure we know which are not being used
                foreach (ViewSchedule Schedule in AllSchedules)
                {
                    //Make sure we only get View Templates
                    if (Schedule.IsTemplate && !Schedule.IsTitleblockRevisionSchedule)
                    {
                        //If it is a view template, check to see if it is in the Placed list
                        if (!ViewTemplatesUsedIds.Contains(Schedule.Id))
                        {
                            //If not, add it to the master container with the Custom ElementIdName class
                            ViewTemplates.Add(new ElementIdName() { Check = false, Name = Schedule.Name, ElemId = Schedule.Id });
                        }
                    }
                }

                //Set the Tab header text to include the number of items. just a visual touch, doesnt do anything
                TabViewTemplates.Header = "View Templates (" + ViewTemplates.Count + ")";
                TabViews.Header = "Views (" + Views.Count + ")";
                TabLegends.Header = "Legends (" + Legends.Count + ")";
                TabSchedules.Header = "Schedules (" + Schedules.Count + ")";


                //Set the Item source for the 4 different list views to the Master Containers
                ListViewViews.ItemsSource = Views;
                ListViewViewTemplates.ItemsSource = ViewTemplates;
                ListViewSchedules.ItemsSource = Schedules;
                ListViewLegends.ItemsSource = Legends;

                //Use a collection view on the item source of the list view to sort the items
                CollectionView ViewsSort = (CollectionView)CollectionViewSource.GetDefaultView(ListViewViews.ItemsSource);
                //We are using the "Name" column / property to sort in this case
                ViewsSort.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            }
            //Catch any exceptions thrown and provide the user with the information
            catch (Exception ex)
            {
                TaskDialog.Show("View Checking Error", ex.ToString());
            }
        }

        //This is the Cancel button event
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Set the dialog result to false so we know how to hanlde it in the command
            DialogResult = false;
            //close the form
            Close();
        }

        //This is the Delete Views button event
        private void BtnDeleteViews_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Create a single list to hold all ElementIds to delete
                List<ElementId> DeleteList = new List<ElementId>();
                //Iterate each one of the Master Container lists and for any items that is "checked"
                //add the Element Id to the Delete list
                foreach (ElementIdName view in Views)
                {
                    if (view.Check)
                    {
                        DeleteList.Add(view.ElemId);
                    }
                }

                foreach (ElementIdName view in ViewTemplates)
                {
                    if (view.Check)
                    {
                        DeleteList.Add(view.ElemId);
                    }
                }

                foreach (ElementIdName view in Schedules)
                {
                    if (view.Check)
                    {
                        DeleteList.Add(view.ElemId);
                    }
                }

                foreach (ElementIdName view in Legends)
                {
                    if (view.Check)
                    {
                        DeleteList.Add(view.ElemId);
                    }
                }
                //Aler the user that this action cannot be undone and ask them to save if they want
                TaskDialogResult result = TaskDialog.Show("Save Document", "Deleting Views, View Templates, Schedules, or Legends from the Document cannot be undone.\n\nWould you like to Save before continuing?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel, TaskDialogResult.Cancel);

                //Save the docuemnt if they select Yes, or do nothing if they press cancel
                if (result == TaskDialogResult.Yes)
                {
                    doc.Save();
                }
                else if (result == TaskDialogResult.Cancel)
                {
                    return;
                }

                //Create a transaction to modify the database
                using (Transaction Trans = new Transaction(doc))
                {
                    //start transaction
                    Trans.Start("Delete Unplaced Views");
                    //delete all element Ids
                    doc.Delete(DeleteList);
                    //commit transaction to keep changes
                    Trans.Commit();
                }

                //set the dialog result to true so the returning command know to return a successful result and retain the changes
                DialogResult = true;
                //Close the form
                Close();
            }
            //Catch any exceptions thrown and provide the user with the information
            catch (Exception ex)
            {
                TaskDialog.Show("Error Deleting", ex.ToString(), TaskDialogCommonButtons.Ok);
            }
        }
        //All of the following are events to hadle the checking and unchecking of multiple selected items at a time
        #region Checking / UnChecking ListViews
        private void ViewsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewViews.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Views, TempViews, true);
        }

        private void ViewsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewViews.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Views, TempViews, false);
        }

        private void TemplateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewViewTemplates.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(ViewTemplates, TempViews, true);
        }

        private void TemplateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewViewTemplates.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(ViewTemplates, TempViews, false);
        }

        private void SchedulesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewSchedules.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Schedules, TempViews, true);
        }

        private void SchedulesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewSchedules.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Schedules, TempViews, false);
        }

        private void LegendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewLegends.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Legends, TempViews, true);
        }

        private void LegendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewLegends.SelectedItems.Cast<ElementIdName>().ToList();

            CheckItem(Legends, TempViews, false);
        }
        #endregion

        //This method is what iterates the selected items in the ListView against the entire master list to change the value of the Check variable
        private void CheckItem(List<ElementIdName> _list, List<ElementIdName> _temp, bool _check)
        {
            //Iterate through each of the selected rows in the List View
            foreach (ElementIdName view in _temp)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ElementIdName item = _list.Find(x => x.ElemId == view.ElemId);
                //set the bool Check variable to true or checked
                item.Check = _check;
            }
        }
    }
}
