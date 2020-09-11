using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace TorsionTools.Forms
{
    public partial class SheetTitleblockKeyPlanForm : Form
    {
        Document doc;
        IList<Element> tBlocks;
        IList<Element> tBlockTypes;
        //Set the Document and get all Titleblocks and Titleblock types
        public SheetTitleblockKeyPlanForm(Document _doc)
        {
            InitializeComponent();
            doc = _doc;

            //Titleblock types using the "WhereElementIsElementType" filter for the Collector
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType())
            {
                tBlockTypes = fec.ToElements();
            }

            //Titleblock types using the "WhereElementIsNotElementType" filter for the Collector
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsNotElementType())
            {
                tBlocks = fec.ToElements();
            }
        }

        private void SheetTitleblockKeyPlanForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Create a new sorted list to add the Titleblock Types to
                SortedList<string, Element> titleBlocks = new SortedList<string, Element>();

                //loop the Titleblock types and add them to the Sorted list
                foreach (Element tBlock in tBlockTypes)
                {
                    titleBlocks.Add(tBlock.Name, tBlock);
                }

                //Bind the Sorted List to the ComboBox on the form
                cboTitleBlock.DataSource = titleBlocks.ToList();
                //Since the sorted list is very similar to a dictionary, you can use the "Key" and "Value" pairs to get the information
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
                //Check to see if there was any text in the Search box first
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    //create a Transaction to modify the document
                    using (Transaction trans = new Transaction(doc))
                    {
                        //Start and name the transaction
                        trans.Start("Title Block Key Plan Visibility");
                        //Look through every Titleblock in the document
                        foreach (Element tBLock in tBlocks)
                        {
                            //Use the Radio button selection to determine if it will search the Sheet Number or Sheet Name
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
                        //Commit the Transaction to save the changes to the document
                        trans.Commit();
                    }
                    //Set the Form's Dialog Result to OK to be used in the Command for the Result
                    DialogResult = DialogResult.OK;
                    Close();
                }
                //If the Search Box is empty, tell the user they need to enter text
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
            //Set the Dialog Result of the Form to Cancel so no changes will be made to the Document
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cboTitleBlock_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                //Created a Sorted List for each Yes / No Parameter for the Titleblock Type selected
                SortedList<string, Parameter> visibilityParams = new SortedList<string, Parameter>();

                foreach (Element tBlock in tBlocks)
                {
                    //Loop each Titleblock instance until it finds the first one that matches the Titleblock Type you selected
                    if (tBlock.GetTypeId() == ((Element)cboTitleBlock.SelectedValue).Id)
                    {
                        //Get all Parameters from the Titleblock Instance
                        foreach (Parameter param in tBlock.Parameters)
                        {
                            //Check to see if they are Yes / No Parameter Types
                            if (param.Definition.ParameterType == ParameterType.YesNo)
                            {
                                visibilityParams.Add(param.Definition.Name, param);
                            }
                        }
                        //Break the look when the first one is found
                        break;
                    }
                }
                //Bind the Sorted List to the Combo Box for Parameters
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
