using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;


namespace TorsionTools.Commands
{
    [Transaction(TransactionMode.Manual)]
    class Warnings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;
            
            //Get the application (Revit) that will Own the modeless dialog
            IWin32Window revit_window = new Helpers.JtWindowHandle(ComponentManager.ApplicationWindow);

            //create a new instance of the WarningsForm and pass it the UIApplication variable
            Forms.WarningsForm frm = new Forms.WarningsForm(uiapp);
            
            //Display the form in a modeless form which means you can continue to work in Revit when it is open
            frm.Show(revit_window);

            //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
            return Result.Succeeded;
        }
    }
}
