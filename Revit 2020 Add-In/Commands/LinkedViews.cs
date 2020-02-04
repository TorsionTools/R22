using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using System;


namespace Revit_2020_Add_In.Commands
{
    [Transaction(TransactionMode.Manual)]
    class LinkedViews : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            using (Forms.LinkedViewSelectionForm form = new Forms.LinkedViewSelectionForm(doc))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    return Result.Succeeded;
                }
                else
                {
                    return Result.Cancelled;
                }
            }
        }
    }
}
