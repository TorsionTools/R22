using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using System;
using System.Linq;

namespace TorsionTools.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    //Change the Class Name to something other than 'TEMPLATE'
    class ViewActiveToSheet : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

            //Create a new instance of the SheetSelection form.
            //This implementation is a Windows Form. For WPF see WPF.SheetSelectionWPF
            WPF.SheetSelectionWPF form = new WPF.SheetSelectionWPF(doc);
            //Make sure the Sheet Selection Form returns the correct DialogResult
            if (form.ShowDialog().Value)
            {
                //Make sure the user selected at least 1 sheet
                if (form.ViewSheetIds.Count >= 1)
                {
                    //Check to see if more than one sheet was selected and alert the user if so.
                    if (form.ViewSheetIds.Count > 1)
                    {
                        TaskDialog.Show("Information", "More than one Sheet was selected.\nThe first Sheet in the selection will be used.");
                    }

                    //Use this try to make sure Revit doesn't crash when trying to place or update the Viewports
                    try
                    {
                        //Create a Transaction within a using block to dispose of everything when complete
                        using (Transaction Trans = new Transaction(doc))
                        {
                            //Provide a name for the transaction in the Undo / Redo List
                            Trans.Start("Place Active View");
                            ElementId viewSheetId = form.ViewSheetIds[0];

                            //Get the ViewSheet (Sheet) for the Element Id in the Document
                            ViewSheet viewSheet = doc.GetElement(viewSheetId) as ViewSheet;
                            
                            //Get the Title Block on the sheet to extract Height and Width for View Placement
                            FamilyInstance TitleBlock = new FilteredElementCollector(doc, viewSheetId).OfCategory(BuiltInCategory.OST_TitleBlocks).Cast<FamilyInstance>().FirstOrDefault();

                            //Use the built in parameters for Sheet Width and Height to get their values
                            double SheetWidth = TitleBlock.get_Parameter(BuiltInParameter.SHEET_WIDTH).AsDouble();
                            double SheetHeight = TitleBlock.get_Parameter(BuiltInParameter.SHEET_HEIGHT).AsDouble();

                            //Check to see if the active view can be placed on the selected sheet
                            if (Viewport.CanAddViewToSheet(doc, viewSheetId, doc.ActiveView.Id))
                            {
                                //Create a viewport for the active view on the sheet selected. 
                                //Viewport will be centered on the sheet based on the Height and Width parameters
                                Viewport.Create(doc, viewSheetId, doc.ActiveView.Id, new XYZ(SheetWidth / 2, SheetHeight / 2, 0));
                            }
                            else
                            {
                                TaskDialog.Show("Information", "Active View cannot be placed on the selected Sheet");
                                return Result.Cancelled;
                            }

                            //Commit the Transaction to keep the changes
                            Trans.Commit();

                            //Check to see if the user would like to change views to the sheet after the viewport was created
                            if (TaskDialog.Show("Change View","Would you like to go to Sheet:\n"+viewSheet.SheetNumber+" - "+viewSheet.Name+"?",TaskDialogCommonButtons.Yes|TaskDialogCommonButtons.No) == TaskDialogResult.Yes)
                            {
                                uiapp.ActiveUIDocument.ActiveView = viewSheet;
                            }

                            //Return a successful result to Revit so the changes are kept
                            return Result.Succeeded;
                        }
                    }
                    //Catch an exceptions when adding or updating the Viewports. Tell the user and return a Failed Result so Revit does not keep any changes
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Viewport Placement Error", ex.ToString());
                        return Result.Failed;
                    }
                }
                //Tell the user that they did not select any sheets and return a Cancelled result so Revit does not keep any changes
                else
                {
                    TaskDialog.Show("Sheet Selection", "No Sheets were selected");
                    return Result.Cancelled;
                }
            }
            //If the Sheet Selection form was closed or cancelled then return a Cancelled result so Revit does not keep any changes
            else
            {
                return Result.Cancelled;
            }
        }
    }
}
