using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using System;


namespace TorsionTools.Commands
{
    //Used to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    class HelloWorld: IExternalCommand
    {
        //THis has to be here to make sure Revit knows this command is running inside Revit
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Show a Pop Up dialog weith specified Title and Message
            TaskDialog.Show("My Add-In", "Hello World!\nThis is your Revit Add-In.");

            //Let Revit know it was successfully executed
            return Result.Succeeded;
        }
    }
}
