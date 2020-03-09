using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace Revit_2020_Add_In.Settings
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    //Change the Class Name to something other than 'TEMPLATE'
    class UpdatersOn : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Enable All Dynamic Updaters in the Application
            App.EnableUpdaters(Helpers.StaticVariables.revitApplication);

            //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
            return Result.Succeeded;
        }
    }
}
