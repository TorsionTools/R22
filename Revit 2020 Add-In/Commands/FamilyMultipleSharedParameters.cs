using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace TorsionTools.Commands
{
    [Transaction(TransactionMode.Manual)]
    class FamilyMultipleSharedParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            using (Forms.FamilyMultipleSharedParametersForm sharedParameterForm = new Forms.FamilyMultipleSharedParametersForm(doc))
            {
                if (sharedParameterForm.ShowDialog() == DialogResult.OK)
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
