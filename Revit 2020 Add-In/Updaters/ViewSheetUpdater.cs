using Autodesk.Revit.DB;
using System;

namespace Revit_2020_Add_In.Updaters
{
    //This class is for Dynamic Model  Updating (DMU) that can manipulate newly created elements that pass the filters st for them
    public class ViewSheetUpdater : IUpdater
    {
        //Create the static variable needed to Register and Enable the Updaters
        static AddInId m_appId;
        static UpdaterId m_updaterId;

        //Use this Method to return the UpdaterId for registration and Enabling / Disabling the updater
        public ViewSheetUpdater(AddInId id)
        {
            m_appId = id;
            //Make usre you use a new GUID for each updater or there will be conflicts
            m_updaterId = new UpdaterId(m_appId, new Guid("1BD20525-4471-49A1-9D29-3351436DFB2C"));
        }

        //This is where all of the work is done, and the Data is the information supplied by Revit when new elemnts are created
        public void Execute(UpdaterData data)
        {
            //Get the current Document
            Document doc = data.GetDocument();
            //You can Cycle through the AddedElementIds or the Deleted or Modified 
            foreach (ElementId addedElemId in data.GetAddedElementIds()) //data.GetDeletedElementIds, data.GetModifiedElementIds
            {
                //Cast the ElmentIds to the type of element you are working with, Sheets here
                ViewSheet sheet = doc.GetElement(addedElemId) as ViewSheet;
                //This form will ask the User to Input the Name and Number of the New sheet during creation
                using (Forms.ViewSheetUpdaterForm form = new Forms.ViewSheetUpdaterForm(doc, sheet))
                {
                    //Show the form as a modal dialog. This means that it is "Owned" by the Main Window in revit and is the thing active
                    form.ShowDialog();
                }
            }
        }

        //These attitional methods are all required for each updater
        //This can be quereyed for Task dialogs or error messages
        public string GetAdditionalInformation()
        {
            return "Check to see when Sheets are added";
        }

        //Set the ChangePriority of the Updater so that it will narrow the items it is "watching"
        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Views;
        }

        //used to Retrieve the Static UpdaterId
        public UpdaterId GetUpdaterId()
        {
            return m_updaterId;
        }

        //Sets the Updater Name to be called or shown for Task Dialogs
        public string GetUpdaterName()
        {
            return "Torsion Tools Sheet Information Updater";
        }

    }
}
