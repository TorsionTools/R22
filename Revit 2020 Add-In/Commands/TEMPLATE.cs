using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using System;


namespace Revit_2020_Add_In.Commands
{
    [Transaction(TransactionMode.Manual)]
    class TEMPLATE : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //This is where your feature will do its work.

            //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
            return Result.Succeeded;
        }
    }
}
