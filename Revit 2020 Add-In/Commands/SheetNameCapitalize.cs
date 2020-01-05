using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Revit_2020_Add_In.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    //This will Capitalize the name of ALL sheets in the Document
    class SheetNameCapitalize : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Create a new Transaction to modify the Document
                using (Transaction trans = new Transaction(doc))
                {
                    //Start the Transaction and provide a Name for it in the Undo / Redo Dialog
                    trans.Start("Capitalize Sheet Name");
                    //Initalize a new Filtered ELement Collector and collect all Sheets in the Document
                    using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets))
                    {
                        //Loop through each sheet in the Collector
                        foreach (ViewSheet sheet in fec.ToElements())
                        {
                            //Capitalize the Sheet Name
                            sheet.Name = sheet.Name.ToUpper();
                        }
                    }
                    //Commit the transaction to save the changes to the Document
                    trans.Commit();
                }
            }
            //Catch any errors and display a Dialog with the information
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString() + " : " + ex.InnerException);
            }

            //Let Revit know it executed successfully. This is also how you can roll back the entire command with Result.Failed or Result.Cancelled
            return Result.Succeeded;
        }
    }
}
