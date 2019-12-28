using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;


namespace Revit_2020_Add_In.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]

    //Change the Class Name to something other than 'TEMPLATE'
    class SheetSelection : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Create a new instance of the SheetSelection form
            using (Forms.SheetSelectionForm form = new Forms.SheetSelectionForm(doc))
            {
                //Show the Form as a modal window
                if (form.ShowDialog() == DialogResult.OK)
                {
                    //If the DialogResult is .OK, return a Successful Result
                    return Result.Succeeded;
                }
                else
                {
                    //If the DialogResult is anything other than .OK, return a Cancelled Result and the Entire Command will be rolled back
                    return Result.Cancelled;
                }
            }
        }
    }
}
