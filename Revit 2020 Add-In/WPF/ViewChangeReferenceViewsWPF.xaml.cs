using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Revit_2020_Add_In.WPF
{
    public partial class ViewChangeReferenceViewsWPF : Window
    {
        //Class variable for the Document passed to the form
        Document doc;
        public ViewChangeReferenceViewsWPF(Document _doc)
        {
            InitializeComponent();
            //Set the class variable to the Document passed
            doc = _doc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Since View name must be unique, we can use a Sorted List to hold all of the views in the project.
            SortedList<string, View> views = new SortedList<string, View>();

            //Iterate each view and add it to the SortedList
            foreach (View view in Helpers.Collectors.ByCategoryNotElementType(doc, BuiltInCategory.OST_Views))
            {
                //Filter out Legends as they can't be referenced
                if (view.ViewType != ViewType.Legend)
                {
                    views.Add(view.Name, view);
                }
            }

            //Set the Item Source of both Comboboxes to the list of view above
            //Then make sure the display member is the "Key" of the Sorted List and the
            //SelectedValuePath is the "Value" Sorte Lists are similar to Dictionaries with Key, Value pairs
            ComboBoxOldReference.ItemsSource = views;
            ComboBoxOldReference.DisplayMemberPath = "Key";
            ComboBoxOldReference.SelectedValuePath = "Value";

            ComboBoxNewReference.ItemsSource = views;
            ComboBoxNewReference.DisplayMemberPath = "Key";
            ComboBoxNewReference.SelectedValuePath = "Value";
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            //Use a try so an exception doesn't crash revit
            try
            {
                //Case the Old and New views from the Combobox Selected Values
                View OldView = (View)ComboBoxOldReference.SelectedValue;
                View NewView = (View)ComboBoxNewReference.SelectedValue;

                //Use a Filter Rule to Collect ONLY views that have the View Name of the OldView
                FilterRule rule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.VIEW_NAME), OldView.Name, true);
                //Create a Filter from the Filter Rule
                ElementParameterFilter filter = new ElementParameterFilter(rule);
                //Use a Collecter with the Filter to get Viewers that are not ElementTypes
                if (OldView.ViewType == NewView.ViewType)
                {
                    using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewers).WherePasses(filter).WhereElementIsNotElementType())
                    {
                        //Create a new Transaction to make make changes to the Document
                        using (Transaction Trans = new Transaction(doc))
                        {
                            //Start the Transaction and Name it for the Undo/rdo Menu
                            Trans.Start("Change View Reference");
                            //Iterate each Viewer returned by the Collector
                            foreach (Element viewer in fec.ToElements())
                            {
                                //The Viewers do not contain the same parameters as the actual views, so we check for one of them
                                if (viewer.GetParameters("Discipline").Count == 0)
                                {
                                    ReferenceableViewUtils.ChangeReferencedView(doc, viewer.Id, NewView.Id);
                                }
                            }
                            //Commit the Transaction to save the changes
                            Trans.Commit();
                        }
                    }
                    //Set the Dialog result to true so the Command will keep the changes
                    DialogResult = true;
                    //Close the Form
                    Close();
                }
                else
                {
                    TaskDialog.Show("View Type Mismatch", "Current View and New View are not of the Same View Type\n\nCurrent View Type: "+OldView.ViewType.ToString()+"\nNew View Type: "+NewView.ViewType.ToString());
                }
            }
            //Catch and Display any Exeptions that are thrown
            catch (Exception ex)
            {

                TaskDialog.Show("Error", ex.ToString());
                //Set the Dialog result to False so the Command will not keep any changes
                DialogResult = false;
                //Close the Form
                Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //Set the Dialog result to False so the Command will not keep any changes
            DialogResult = false;
            //Close the Form
            Close();
        }
    }
}
