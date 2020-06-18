using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit_2020_Add_In.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    class MEPSpacesFromRooms : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            WPF.MEPSpaceFromRoomWPF form = new WPF.MEPSpaceFromRoomWPF(doc);

            if (form.ShowDialog().Value)
            {
                //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
                return Result.Succeeded;
            }
            else
            {
                //Let Revit know it was cancelled and all changes will be automatically rolled back if any were made.
                return Result.Cancelled;
            }
        }
    }
}
