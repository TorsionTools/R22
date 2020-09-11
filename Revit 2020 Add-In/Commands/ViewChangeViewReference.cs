using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace TorsionTools.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    //Change the Class Name to something other than 'TEMPLATE'
    class ViewChangeViewReference : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Create a new instance of the SheetFindReplaceForm form and pass the current document as a variable
            WPF.ViewChangeReferenceViewsWPF form = new WPF.ViewChangeReferenceViewsWPF(doc);

            //Checks to see if the DialogResult of the form is OK and resturn the correct result as needed.
            if (form.ShowDialog().Value)
            {
                //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
                return Result.Succeeded;
            }
            else
            {
                //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
                return Result.Cancelled;
            }
        }
    }
}
