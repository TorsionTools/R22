using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Revit_2020_Add_In.Commands
{
    [Transaction(TransactionMode.Manual)]
    class LinksReloadAll : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            int count = 0;
            using (FilteredElementCollector rvtLinks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
            {
                if (rvtLinks.ToElements().Count > 0)
                {
                    foreach (RevitLinkType rvtLink in rvtLinks.ToElements())
                    {
                        try
                        {
                            if (rvtLink.GetLinkedFileStatus() == LinkedFileStatus.Loaded)
                            {
                                rvtLink.Reload();
                                count++;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.GetType() == typeof(InvalidOperationException))
                            {
                                TaskDialog.Show("Model Open Warning", rvtLink.Name + " is open in another Document and cannot be reloaded");
                                continue;
                            }
                        }
                    }

                    if (count > 0)
                    {
                        TaskDialog.Show("Links Reloaded", "Reloaded " + count + " Links");
                    }
                    else
                    {
                        TaskDialog.Show("Check Link Status", "Links were found, but no Links were reloaded.\nCheck Linked File Status and try again.");
                    }
                    return Result.Succeeded;
                }
                else
                {
                    TaskDialog.Show("No Links", "No Revit Links Found");
                }
            }
            return Result.Failed;
        }
    }
}
