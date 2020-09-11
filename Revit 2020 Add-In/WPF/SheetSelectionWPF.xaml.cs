using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TorsionTools.WPF
{
    public partial class SheetSelectionWPF : Window
    {
        //Set Class Level Variables
        Document doc;
        //This will the Sheet Element Ids returned to the command that call this form
        public List<ElementId> ViewSheetIds { get; set; }
        //This is the class level container for the sheet information from the ViewSheets class
        List<ViewSheetsIdName> SheetList = new List<ViewSheetsIdName>();

        //We call this form with the current document and set the class Document variable
        public SheetSelectionWPF(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        //Method for Form Loading event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //use a try block to avoid crashing Revit if an exception is thrown
            try
            {
                //Collect and iterate every Sheet in the project
                foreach (ViewSheet sheet in Helpers.Collectors.ByCategory(doc, BuiltInCategory.OST_Sheets))
                {
                    //Check to see if the sheet is a place holder sheet before adding it to the selection
                    if (!sheet.IsPlaceholder)
                    {
                        //Create a new instance of the ViewSheets class and add it to the master SheetList list of ViewSheets
                        SheetList.Add(new ViewSheetsIdName() { Check = false, SheetName = sheet.SheetNumber + " - " + sheet.Name, SheetId = sheet.Id });
                    }
                }
                //Sort the items by the Sheet Number before setting the ListView item source
                SheetList.Sort((x, y) => x.SheetName.CompareTo(y.SheetName));
                //Set the ListView item source to the list of sheets
                ListViewSheets.ItemsSource = SheetList;
                //Provides a view model for the item source of the List view that can be used for filtering and sorting
                CollectionView ListViewFilterView = (CollectionView)CollectionViewSource.GetDefaultView(ListViewSheets.ItemsSource);
                //Set the Filter on this view to the UserFilter that returns true or false for each item in the itemsource
                ListViewFilterView.Filter = SearchFilter;
            }
            //Catch any exceptions thrown and display a TaskDialog with the information
            catch (Exception ex)
            {
                TaskDialog.Show("Error Loading Sheets", ex.ToString());
            }
        }

        //This will select ALL rows in the ListView
        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            //Iterate each items in the SheetList List of ViewSheets
            foreach (ViewSheetsIdName check in SheetList)
            {
                //Set the vool Check varaible to true or Checked in the ListView
                check.Check = true;
            }
            //Clear any selected rows in the ListView
            ListViewSheets.UnselectAll();
        }

        //This will deselect ALL rows in the ListView
        private void BtnSelectNone_Click(object sender, RoutedEventArgs e)
        {
            //Iterate each items in the SheetList List of ViewSheets
            foreach (ViewSheetsIdName check in SheetList)
            {
                //Set the vool Check varaible to false or UnChecked in the ListView
                check.Check = false;
            }
            //Clear any selected rows in the ListView
            ListViewSheets.UnselectAll();
        }

        //The method when a checkbox is checked to select the other rows if multiple are selected
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ViewSheetsIdName> TempSheets = ListViewSheets.SelectedItems.Cast<ViewSheetsIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ViewSheetsIdName Sheet in TempSheets)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ViewSheetsIdName item = SheetList.Find(x => x.SheetId == Sheet.SheetId);
                //set the bool Check variable to true or checked
                item.Check = true;
            }
        }

        //The method when a checkbox is Unchecked to deselect the other rows if multiple are selected
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ViewSheetsIdName> TempSheets = ListViewSheets.SelectedItems.Cast<ViewSheetsIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ViewSheetsIdName Sheet in TempSheets)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ViewSheetsIdName item = SheetList.Find(x => x.SheetId == Sheet.SheetId);
                //set the bool Check variable to false or Unchecked
                item.Check = false;
            }
        }

        //This method handles when the OK Button is clicked
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            //Create a List to hold the ElementIds of the checkd sheets
            List<ElementId> SheetIds = new List<ElementId>();
            //Iterate through the SheetList master list for items that have the Check variable set to true or checked
            foreach (ViewSheetsIdName sheet in SheetList)
            {
                //Test to see if it is true
                if (sheet.Check)
                {
                    //Add the SheetId variable to the local list
                    SheetIds.Add(sheet.SheetId);
                }
            }
            //set the Class ViewSheetIds to equal the Local list for visibility in he Command
            ViewSheetIds = SheetIds;
            //Set the Dialog result of the form to true so we can check that it executed correctly
            DialogResult = true;
            //Close the form
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Set the Dialog result of the form to fasle so we know it was canceled
            DialogResult = false;
            //Close the form
            Close();
        }

        //Event to watch for the text changed property of the search text box
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //This will update / refresh the Default view of the ListView in conbindation with the UserFilter
            CollectionViewSource.GetDefaultView(ListViewSheets.ItemsSource).Refresh();
        }

        //Filter used to check if the item should be visible in the List View or not
        //This filter is used to iterate every item in the collection each time the text
        //is changed, so if you have a LARGE data set, may want to use a DataTable and Default View filter
        private bool SearchFilter(object item)
        {
            //If the Search is empty then "true" shows every item
            if (string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                return true;
            }
            else
            {
                //checked each item to see if the Sheetname contains any part of the search text and return true if so or false if not
                return ((item as ViewSheetsIdName).SheetName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
    }
}
