using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace TorsionTools.Forms
{
    public partial class FamilyMultipleSharedParametersForm : System.Windows.Forms.Form
    {
        //Class variables
        Document doc;
        string FamilyDirectoryPath;

        public FamilyMultipleSharedParametersForm(Document _doc)
        {
            InitializeComponent();
            //Set the variable "doc" from the Document sent to the form
            doc = _doc;
        }

        private void SharedParameterForm_Load(object sender, EventArgs e)
        {
            //Create a sorted list to hold the Shared Parameter Definition Groups that are read from the Shared Parameter File
            SortedList<string, DefinitionGroup> DefinitionGroupList = new SortedList<string, DefinitionGroup>();
            //Create a definition file to hole the Shared Parameters File Definitions
            DefinitionFile definitionFile;

            //Check to make sure there is a Shared Parameter File
            if ((definitionFile = doc.Application.OpenSharedParameterFile()) == null)
            {
                TaskDialog.Show("No Shared Parameter File", "No Shared Parameter Definition File could be found in the current Document.");
                return;
            }

            //Set the Form Label to the File Name of the Shared Parameter File
            txtSPFile.Text = definitionFile.Filename;

            //Add an item to the top of the DefinitionGroupList
            DefinitionGroupList.Add("--Select Group--", null);

            //Loop each Definition Group and add it to the Sorted List created earlier
            foreach (DefinitionGroup current in definitionFile.Groups)
            {
                DefinitionGroupList.Add(current.Name, current);
            }

            //Use the Sorted List as the Data source for the Shared Parameter Group Dropdown
            cboGroups.DataSource = DefinitionGroupList.ToList();
            cboGroups.DisplayMember = "Key";
            cboGroups.ValueMember = "Value";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Verify that the User wants to proceed adding the Shared Parameters
            if (TaskDialog.Show("Modify Families", "This will add Shared Parameters and Upgrade " + Directory.GetFiles(FamilyDirectoryPath, "*.rfa").Length + " Revit Families.\n\nDo you want to Continue?", Autodesk.Revit.UI.TaskDialogCommonButtons.Yes | Autodesk.Revit.UI.TaskDialogCommonButtons.No, Autodesk.Revit.UI.TaskDialogResult.Yes) != Autodesk.Revit.UI.TaskDialogResult.Yes)
            {
                return;
            }

            //Find every Revit Family in the selected Directory
            foreach (string file in Directory.GetFiles(FamilyDirectoryPath, "*.rfa"))
            {
                //Wrap the events in a Try / Catch block to prevent any exceptions thrown from crashing or corrupting Revit
                try
                {
                    //Open the Revit Family File
                    if ((doc.Application.OpenDocumentFile(file)) is Document FamilyDoc)
                    {
                        //Verify is it in face a Family Document
                        if (FamilyDoc.IsFamilyDocument)
                        {
                            //Create a Transaction to make the changes in the Family Document
                            using (Transaction transaction = new Transaction(FamilyDoc))
                            {
                                //Start the Transaction and give it a Name
                                transaction.Start("Add Shared Parameters");
                                //Loop through each Shared Parameter checking in the List View
                                foreach (ListViewItem checkedItem in ltvParameters.CheckedItems)
                                {
                                    //Get the Parameter definition from the Tag property of the List View Item
                                    Definition paramDefinition = (Definition)checkedItem.Tag;
                                    //Cast the Definition as an External Defintion for use as a Shared Parameter
                                    ExternalDefinition externalDefinition = paramDefinition as ExternalDefinition;
                                    //Add the Shared Parameter to the family. Use the Instance Check box to indicated Type or Instance Parameters
                                    FamilyDoc.FamilyManager.AddParameter(externalDefinition, paramDefinition.ParameterGroup, chkInstance.Checked);
                                }
                                //Commit the transaction to keep the changes.
                                transaction.Commit();
                            }
                            //Close the Family and save the changes with "True"
                            FamilyDoc.Close(true);
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Parameter Error", "There was a problem adding the Shared Parameters to the File: " + Environment.NewLine + file);
                    }

                }
                //Catch and exceptions to keep Revit from crashing and display the exception information.
                catch (Exception ex)
                {
                    TaskDialog.Show("Shared Parameter Error", ex.ToString());
                    //Try to continue to the next file
                    continue;
                }
            }
            //Set the Dialog result of the Form to OK so the Comamnd knows it finished
            DialogResult = DialogResult.OK;
            //Close the form
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Set the Dialog result of the Form to Cancel
            DialogResult = DialogResult.Cancel;
            //Close the form
            Close();
        }

        //When the drop down is changed the list of Shared Parameters changes
        private void cboGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Check to see if it is the top selection where we manually added a value
            if (cboGroups.SelectedValue != null)
            {
                //Clear the list view of any current items
                ltvParameters.Items.Clear();
                //Get all Definitions from the Definition Group Selected
                foreach (Definition def in ((DefinitionGroup)cboGroups.SelectedValue).Definitions)
                {
                    ListViewItem item = ltvParameters.Items.Add(def.Name);
                    item.Tag = def;
                }
            }
        }

        //Set the Directory for where the families are located
        private void btnDirectory_Click(object sender, EventArgs e)
        {
            //Use a Folder Browser Dialog to browse to the directory containing the families
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;
                FamilyDirectoryPath = folderBrowserDialog.SelectedPath;
                lblDirectory.Text = FamilyDirectoryPath;
            }
        }
    }
}
