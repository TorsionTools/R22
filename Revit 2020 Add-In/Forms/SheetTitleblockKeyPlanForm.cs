using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace Revit_2020_Add_In.Forms
{
    public partial class SheetTitleblockKeyPlanForm : Form
    {
        Document doc;
        IList<Element> tBlocks;
        IList<Element> tBlockTypes;
        public SheetTitleblockKeyPlanForm(Document _doc)
        {
            InitializeComponent();
            doc = _doc;

            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType())
            {
                tBlockTypes = fec.ToElements();
            }

            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsNotElementType())
            {
                tBlocks = fec.ToElements();
            }
        }

        private void SheetTitleblockKeyPlanForm_Load(object sender, EventArgs e)
        {
            try
            {
                SortedList<string, Element> titleBlocks = new SortedList<string, Element>();

                foreach (Element tBlock in tBlockTypes)
                {
                    titleBlocks.Add(tBlock.Name, tBlock);
                }

                cboTitleBlock.DataSource = titleBlocks.ToList();
                cboTitleBlock.DisplayMember = "Key";
                cboTitleBlock.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    using (Transaction trans = new Transaction(doc))
                    {
                        trans.Start("Title Block Key Plan Visibility");
                        foreach (Element tBLock in tBlocks)
                        {
                            if (rdoNumber.Checked)
                            {
                                if (tBLock.LookupParameter("Sheet Number").AsString().Contains(txtSearch.Text))
                                {
                                    tBLock.LookupParameter(((Parameter)cboParameter.SelectedValue).Definition.Name).Set(Convert.ToInt32(rdoShow.Checked));
                                }
                            }
                            else
                            {
                                if (tBLock.LookupParameter("Sheet Name").AsString().Contains(txtSearch.Text))
                                {
                                    tBLock.LookupParameter(((Parameter)cboParameter.SelectedValue).Definition.Name).Set(Convert.ToInt32(rdoShow.Checked));
                                }
                            }
                        }
                        trans.Commit();
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    TaskDialog.Show("Invalid Search", "The Search box cannot be empty");
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Setting Parameters", ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cboTitleBlock_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SortedList<string, Parameter> visibilityParams = new SortedList<string, Parameter>();

                foreach (Element tBlock in tBlocks)
                {
                    if (tBlock.GetTypeId() == ((Element)cboTitleBlock.SelectedValue).Id)
                    {
                        foreach (Parameter param in tBlock.Parameters)
                        {
                            if (param.Definition.ParameterType == ParameterType.YesNo)
                            {
                                visibilityParams.Add(param.Definition.Name, param);
                            }
                        }
                        break;
                    }
                }

                cboParameter.DataSource = visibilityParams.ToList();
                cboParameter.DisplayMember = "Key";
                cboParameter.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Getting Titleblock Parameters", ex.ToString());
            }
        }
    }
}
