using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revit_2020_Add_In.Forms
{
    public partial class ElementsOfCategoryForm : System.Windows.Forms.Form
    {
        //Defin Class Variables
        Document doc;
        UIApplication uiApp;
        //Receive the UIApplication from the command
        public ElementsOfCategoryForm(UIApplication _uiApp)
        {
            InitializeComponent();
            //Set the UIApplication class variable 
            uiApp = _uiApp;
            //Set the Document class variable from the UIApplication
            doc = _uiApp.ActiveUIDocument.Document;
        }

        //When the Form loads, the Category combobox will be populated with the Document Categories
        private void ElementsOfCategoryForm_Load(object sender, EventArgs e)
        {
            //New SortedList to hold all of the Categories for binding to ComboBox
            SortedList<string, int> categories = new SortedList<string, int>();
            //Get all of the BuiltInCategories in the Document
            Categories docCategories = doc.Settings.Categories;
            try
            {
                //Loop through each Category in the Document Categories
                foreach (Category category in docCategories)
                {
                    //check to see if the is a Model or Annotation Category Type
                    if (category.CategoryType == CategoryType.Model || category.CategoryType == CategoryType.Annotation)
                    {
                        //A check for categories that are built in and cannot have parameters bound to them so not Families / Instances
                        if (category.AllowsBoundParameters)
                        {
                            //Make sure the category is not Read Only, again for built in categories that we can't manipulate
                            if (!category.IsReadOnly)
                            {
                                //Get the BuiltInCategory associated with the ComboBox selection
                                BuiltInCategory bic = (BuiltInCategory)Enum.ToObject(typeof(BuiltInCategory), category.Id.IntegerValue);
                                //Check to see if at least 1 element is of the selected category
                                if (new FilteredElementCollector(doc).OfCategory(bic).OfClass(typeof(FamilySymbol)).GetElementCount() > 0)
                                {
                                    //Add the Category Name and Id as integer value to the Sorted List from above
                                    categories.Add(category.Name, category.Id.IntegerValue);
                                }
                            }
                        }
                    }
                }
                //Bind the SortedList to the ComboBox using a ToList Linq extension
                cbCategories.DataSource = categories.ToList();
                //Set the "TKey" and "TValue" of the Sorted list to the Display and Value Members of the ComboBox
                cbCategories.DisplayMember = "Key";
                cbCategories.ValueMember = "Value";

            }
            //Catch any errors int he Try an display the information
            catch (Exception ex)
            {
                TaskDialog.Show("Category Load Error", ex.ToString());
            }
        }

        //When the value of the ComboBox is changed this method will collect all elements of that category
        private void cbCategories_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Clear the DataGridView so items are not just continously added
            dgvElements.Rows.Clear();
            //Get the BuiltInCategory associated with the ComboBox selection
            BuiltInCategory bic = (BuiltInCategory)Enum.ToObject(typeof(BuiltInCategory), cbCategories.SelectedValue);
            //FilteredElementCollector to get all elements that are not Element Types from the selected Category
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(bic).WhereElementIsNotElementType())
            {
                //Loop through each Element (Family Instance) returned by the Element Collector
                foreach (FamilyInstance famInstance in fec.ToElements())
                {
                    //Check to see if the Family Instance has a Level parameter associated. The "is" structure here can be very usefull as it would be like a elem != null check in line
                    if (doc.GetElement(famInstance.LevelId) is Level level)
                    {
                        //Adds a row to the DataGridView with various information including the level assoicated
                        dgvElements.Rows.Add(famInstance.Id, famInstance.Name, famInstance.Symbol.FamilyName, famInstance.ViewSpecific, level.Name);
                    }
                    else
                    {
                        //Adds a row to the DataGridView with various information but without the level assoicated since the Instance or Category may not permit level association
                        dgvElements.Rows.Add(famInstance.Id, famInstance.Name, famInstance.Symbol.FamilyName, famInstance.ViewSpecific, "None");
                    }
                }
            }
            //Add the Number of Elements to the Name Column Header
            dgvElements.Columns[1].HeaderText = "Name (" + dgvElements.Rows.Count + ")";
        }

        //Closes the Form
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //This method make the active selection the element in the currently selected row
        private void dgvElements_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            //make sure the header row is not selected
            if (e.RowIndex != -1)
            {
                //Create and intialize a list of ElementId class
                List<ElementId> selection = new List<ElementId>();
                //Add the ElementId by casting it from the first column of the DataGridView
                selection.Add((ElementId)dgvElements.Rows[e.RowIndex].Cells[0].Value);
                //Use the List of Element Ids to set the current selection to that element
                uiApp.ActiveUIDocument.Selection.SetElementIds(selection);
            }
        }
    }
}
