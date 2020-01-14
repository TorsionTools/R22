using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;


namespace Revit_2020_Add_In.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    //Change the Class Name to something other than 'TEMPLATE'
    class ElementsOfCategory : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the application (Revit) that will Own the modeless dialog
            IWin32Window revit_window = new Helpers.JtWindowHandle(ComponentManager.ApplicationWindow);

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //create a new instance of the ElementsOfCategory Form and pass it the UIApplication variable
            Forms.ElementsOfCategoryForm form = new Forms.ElementsOfCategoryForm(uiapp);

            //Display the form in a modeless form which means you can continue to work in Revit when it is open
            form.Show(revit_window);

            //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
            return Result.Succeeded;
        }
    }
}
