using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Revit_2020_Add_In.Commands
{
    //This Transaction Attribute allows us to control transactions manually, but also placed the entire class in a transaction that is controlled by the Result returned to prevent unwanted changes
    //during Transactions or sub-transactions if changes are made and then an exception is raised
    [Transaction(TransactionMode.Manual)]
    class SheetLegendToMultiple : IExternalCommand
    {
        //Create a Class level variable to use for a Viewport
        Viewport viewPort = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the current Document from the Command Data
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //Check to make sure the user is on a sheet before continuing.
            if (doc.ActiveView.ViewType == ViewType.DrawingSheet)
            {
                //Get a new UIDocument from the current document to use to make a selection
                UIDocument uidoc = new UIDocument(doc);
                //Create a new instance of the ViewSelectionFilter to only allow the user to select a Legend Viewport
                ISelectionFilter selectionFilter = new ViewSelectionFilter();
                //A selection of elements from the current Document
                Selection picked = uidoc.Selection;
                //Try block to catch if the user cancels the selection process
                try
                {
                    //Prompt the user to make a single selection in the Revit interface. Only Legend Viewports will be available based onour SelectionFilter. The string is what the status bar displays in the Lower Left
                    Reference selection = picked.PickObject(ObjectType.Element, selectionFilter, "Select Legend on Sheet");
                    //Make sure the user made a selection
                    if (picked != null)
                    {
                        //Set the Class variable to the Viewport the user selected
                        viewPort = doc.GetElement(selection) as Viewport;
                        //Get the Location of the Legend Viewport selected
                        XYZ locPt = viewPort.GetBoxCenter();
                        //Prompt the user with a model dialog to select the sheets to place or update the Legends on
                        Forms.SheetSelectionForm frm = new Forms.SheetSelectionForm(doc);
                        //Make sure the Sheet Selection Form returns the correct DialogResult
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            //Make sure the user selected at least 1 sheet
                            if (frm.ViewSheetIds.Count > 0)
                            {
                                //Use this try to make sure Revit doesn't crash when trying to place or update the Legend Viewports
                                try
                                {
                                    //Create a Transaction within a using block to dispose of everything when complete
                                    using (Transaction Trans = new Transaction(doc))
                                    {
                                        //Provide a name for the transaction in the Undo / Redo List
                                        Trans.Start("Place Multiple Legends");
                                        //Loop through each Sheet Element Id from the sheets selected by the User
                                        foreach (ElementId viewSheetId in frm.ViewSheetIds)
                                        {
                                            //Get the ViewSheet (Sheet) fro each Element Id in the Document
                                            ViewSheet viewSheet = doc.GetElement(viewSheetId) as ViewSheet;
                                            //Get all of the Viewport Element Ids on that ViewSheet 
                                            ICollection<ElementId> viewPortIds = viewSheet.GetAllViewports();
                                            //Use this check to see if the viewport already exists or if it needs to be created
                                            bool PlaceViewport = true;
                                            //Loop through each ElementId for all Viewports on the Sheet
                                            foreach (ElementId elementId in viewPortIds)
                                            {
                                                //Get the Viewport from the ElementId so we can get the ViewId
                                                Viewport vp = doc.GetElement(elementId) as Viewport;
                                                //If the Viewport ViewId of the Legend selected matches the ViewId of the current Viewport then we now have the matching legend on the Sheet
                                                if (vp.ViewId == viewPort.ViewId)
                                                {
                                                    //Set the bool parameter to False since we will not need to Place a new viewport
                                                    PlaceViewport = false;
                                                    //Set the location on the sheet to the same location as the Legend selected
                                                    vp.SetBoxCenter(locPt);
                                                    //Break the loop so we don't have to loop through any extra Viewports
                                                    break;
                                                }
                                            }
                                            //If the Legend Viewport wasn't found, then the bool parameters remains True and a new Viewport for the Legend will be created
                                            if (PlaceViewport)
                                            {
                                                //Create a new viewport at the correct location and sheet 
                                                Viewport viewport = Viewport.Create(doc, viewSheetId, viewPort.ViewId, locPt);
                                                //Set the Viewport Type to the same as the Legend selected
                                                viewport.ChangeTypeId(viewPort.GetTypeId());
                                            }
                                        }
                                        //Commit the Transaction to keep the changes
                                        Trans.Commit();
                                        //Return a successful result to Revit so the changes are kept
                                        return Result.Succeeded;
                                    }
                                }
                                //Catch an exceptions when adding or updating the Legend Viewports. Tell the user and return a Failed Result so Revit does not keep any changes
                                catch (Exception ex)
                                {
                                    TaskDialog.Show("Legend Placement Error", ex.ToString());
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
                    //Retrun a Cancelled Reseult if no eleemnts are picked
                    return Result.Cancelled;
                }
                //Catch the Operation canceled Exception which is raised when the user "Escapes" during a Pick Object operation and return a Failed result
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    return Result.Failed;
                }
            }
            //Tell the user that they need to be in a Sheet View to run this Command
            else
            {
                TaskDialog.Show("Sheet Required", "Active view must be a Sheet");
                return Result.Failed;
            }
        }

        //Use the ISelectionFilter to filter which elemnts are selectable when using a UIDocument Selection method like PickObject
        private class ViewSelectionFilter : ISelectionFilter
        {
            //Return a bool if the element being "hovered" on passes the criteria
            public bool AllowElement(Element element)
            {
                //Make sure the element is a Viewport
                Viewport v = element as Viewport;
                //Check to see if the View Family Type is a 'Legend' 
                if (v.get_Parameter(BuiltInParameter.VIEW_FAMILY).AsString() == "Legends")
                {
                    //Return true which means the Element can be selected
                    return true;
                }
                //Return False which means Revit will not highlight or allow the element to be selected
                return false;
            }

            //Use this to make sure the user can not select a Reference like Grid or Level
            public bool AllowReference(Reference refer, XYZ point)
            {
                //return false for ALL reference elements
                return false;
            }
        }
    }
}
