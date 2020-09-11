using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;


namespace TorsionTools.Commands
{
    //This Attribute sets how transactions are handled. The Manual Transaction Mode allows us to control any 
    //transactions individually, but roll the entire function back if we return any Result other than Succeded
    [Transaction(TransactionMode.Manual)]
    //This class will call the View Legend Copy form copy legends from a Linked Document
    class LinkedViews : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;
            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Create a new instance of the LinkedViewSelectionForm form and pass the current document as variables
            using (Forms.LinkedViewSelectionForm form = new Forms.LinkedViewSelectionForm(doc))
            {
                //Checks to see if the DialogResult of the form is OK and resturn the correct result as needed.
                if (form.ShowDialog() == DialogResult.OK)
                {
                    //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
                    return Result.Succeeded;
                }
                else
                {
                    //Let Revit know the Execute method did not finish successfully. All modifications to the Document will be rolled back based on the TransactionMode
                    return Result.Cancelled;
                }
            }
        }
    }
}
