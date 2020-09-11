using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit_2020_Add_In.WPF
{
    /// <summary>
    /// Interaction logic for SheetIndexSetWPF.xaml
    /// </summary>
    public partial class SheetIndexSetWPF : Window
    {
        Document doc;
        //This is the class level container for the sheet information from the ViewSheets class
        List<ViewSheetsIdNumberName> SheetList = new List<ViewSheetsIdNumberName>();

        public SheetIndexSetWPF(Document _doc)
        {
            InitializeComponent();
            doc = _doc;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

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
                        //Create a new instance of the ViewSheetsIdNumberName class and add it to the master SheetList list of ViewSheets
                        SheetList.Add(new ViewSheetsIdNumberName() { Check = Convert.ToBoolean(sheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED).AsInteger()), SheetName = sheet.Name, SheetNumber=sheet.SheetNumber, SheetId = sheet.Id });
                    }
                }
                //Sort the items by the Sheet Number before setting the DataGrid item source
                SheetList.Sort((x, y) => x.SheetNumber.CompareTo(y.SheetNumber));
                //Set the DataGrid item source to the list of sheets
                dgSheetList.ItemsSource = SheetList;
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
            foreach (ViewSheetsIdNumberName check in SheetList)
            {
                //Set the vool Check varaible to true or Checked in the ListView
                check.Check = true;
            }
            //Clear any selected rows in the DataGrid
            dgSheetList.UnselectAll();
        }

        //This will deselect ALL rows in the ListView
        private void BtnSelectNone_Click(object sender, RoutedEventArgs e)
        {
            //Iterate each items in the SheetList List of ViewSheets
            foreach (ViewSheetsIdNumberName check in SheetList)
            {
                //Set the vool Check varaible to false or UnChecked in the ListView
                check.Check = false;
            }
            //Clear any selected rows in the DataGrid
            dgSheetList.UnselectAll();
        }

        //The method when a checkbox is checked to select the other rows if multiple are selected
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ViewSheetsIdNumberName> TempSheets = dgSheetList.SelectedItems.Cast<ViewSheetsIdNumberName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ViewSheetsIdNumberName Sheet in TempSheets)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ViewSheetsIdNumberName item = SheetList.Find(x => x.SheetId == Sheet.SheetId);
                //set the bool Check variable to true or checked
                item.Check = true;
            }
        }

        //The method when a checkbox is Unchecked to deselect the other rows if multiple are selected
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ViewSheets class
            List<ViewSheetsIdNumberName> TempSheets = dgSheetList.SelectedItems.Cast<ViewSheetsIdNumberName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ViewSheetsIdNumberName Sheet in TempSheets)
            {
                //Try and find the item in the main SheetList list and get that object
                //We can't directly change the SheetList because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ViewSheetsIdNumberName item = SheetList.Find(x => x.SheetId == Sheet.SheetId);
                //set the bool Check variable to false or Unchecked
                item.Check = false;
            }
        }

        //This method handles when the OK Button is clicked
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            //Iterate through the SheetList master list for items that have the Check variable set to true or checked
            foreach (ViewSheetsIdNumberName sheet in SheetList)
            {
                //Test to see if it is true
                ViewSheet viewSheet = doc.GetElement(sheet.SheetId) as ViewSheet;
                if (sheet.Check)
                {
                    //Set the Sheet parameters
                    viewSheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED).Set(1);
                }
                else
                {
                    viewSheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED).Set(0);
                }
            }

            //Set the Dialog result of the form to true so we can check that it executed correctly
            DialogResult = true;
            //Close the form
            Close();
        }
    }

}
