using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revit_2020_Add_In.Forms
{
    public partial class ElementsOfCategoryForm : System.Windows.Forms.Form
    {
        Document doc;
        UIApplication uiApp;
        public ElementsOfCategoryForm(UIApplication _uiApp)
        {
            InitializeComponent();
            uiApp = _uiApp;
            doc = _uiApp.ActiveUIDocument.Document;
        }

        private void ElementsOfCategoryForm_Load(object sender, EventArgs e)
        {
            SortedList<string, int> categories = new SortedList<string, int>();
            Categories cats = doc.Settings.Categories;
            try
            {
                foreach (Category cat in cats)
                {
                    if (cat.CategoryType == CategoryType.Model || cat.CategoryType == CategoryType.Annotation)
                    {
                        if (cat.AllowsBoundParameters)
                        {
                            if (!cat.IsReadOnly)
                            {
                                BuiltInCategory bic = (BuiltInCategory)Enum.ToObject(typeof(BuiltInCategory), cat.Id.IntegerValue);
                                if (new FilteredElementCollector(doc).OfCategory(bic).OfClass(typeof(FamilySymbol)).GetElementCount() > 0)
                                {
                                    categories.Add(cat.Name, cat.Id.IntegerValue);
                                }
                            }
                        }
                    }
                }
                cbCategories.DataSource = categories.ToList();
                cbCategories.DisplayMember = "Key";
                cbCategories.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Category Load Error", ex.ToString());
            }
        }

        private void cbCategories_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvElements.Rows.Clear();
            BuiltInCategory bic = (BuiltInCategory)Enum.ToObject(typeof(BuiltInCategory), cbCategories.SelectedValue);
            //using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(bic).OfClass(typeof(FamilySymbol)))
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(bic).WhereElementIsNotElementType())
            {
                foreach (FamilyInstance famInstance in fec.ToElements())
                {
                    if (!famInstance.ViewSpecific)
                    {
                        dgvElements.Rows.Add(famInstance.Id, famInstance.Name, famInstance.Symbol.FamilyName, famInstance.ViewSpecific, doc.GetElement(famInstance.LevelId).Name);
                    }
                    else
                    {
                        dgvElements.Rows.Add(famInstance.Id, famInstance.Name, famInstance.Symbol.FamilyName, famInstance.ViewSpecific, "None");
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvElements_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                List<ElementId> selection = new List<ElementId>();
                selection.Add((ElementId)dgvElements.Rows[e.RowIndex].Cells[0].Value);
                uiApp.ActiveUIDocument.Selection.SetElementIds(selection);
            }
        }
    }
}
