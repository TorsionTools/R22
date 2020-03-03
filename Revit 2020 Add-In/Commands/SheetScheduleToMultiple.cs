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
    class SheetScheduleToMultiple : IExternalCommand
    {
        //Create a Class level variable to use for a ScheduleSheetInstance
        ScheduleSheetInstance ScheduleInstanceSelected = null;
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
                ISelectionFilter filter = new ViewSelectionFilter();
                //A selection of elements from the current Document
                Selection picked = uidoc.Selection;
                //Try block to catch if the user cancels the selection process
                try
                {
                    //Prompt the user to make a single selection in the Revit interface. Only Shceule Instances will be available based onour SelectionFilter. 
                    //The string is what the status bar displays in the Lower Left
                    Reference selection = picked.PickObject(ObjectType.Element, filter, "Select Schedule on Sheet");
                    //Make sure the user made a selection
                    if (picked != null)
                    {
                        //Set the Class variable to the Viewport the user selected
                        ScheduleInstanceSelected = doc.GetElement(selection) as ScheduleSheetInstance;
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
                                        Trans.Start("Place Multiple Schedules");
                                        foreach (ElementId viewSheetId in frm.ViewSheetIds)
                                        {
                                            //Get the ViewSheet (Sheet) fro each Element Id in the Document
                                            ViewSheet viewSheet = doc.GetElement(viewSheetId) as ViewSheet;
                                            //Use a Filtered Element Collector on the Sheet to get all ScheduleSheetInstances which is the reference to the Schedule you see
                                            //We do this to check if the Sheet already contains the Schedule we are using
                                            ICollection<Element> ScheduleInstances = new FilteredElementCollector(doc, viewSheetId).OfClass(typeof(ScheduleSheetInstance)).ToElements();
                                            //Use this check to see if the Schedule already exists or if it needs to be created
                                            bool PlaceSchedule = true;
                                            //Loop through each Shcedule Instance on the Sheet
                                            foreach (ScheduleSheetInstance ScheduleInstance in ScheduleInstances)
                                            {
                                                //Check to see if the 'Master' ScheduleId matches the one that was selected by the User
                                                if (ScheduleInstance.ScheduleId == ScheduleInstanceSelected.ScheduleId)
                                                {
                                                    //Set the bool to False since the Sheet already contains the schedule, we don't need to create it
                                                    PlaceSchedule = false;
                                                    //Move the Schedule on the Sheet to match the same location as the one Selected
                                                    ScheduleInstance.Point = ScheduleInstanceSelected.Point;
                                                }
                                            }
                                            //If the Schedule was not found on the sheet, the bool stayed True so this will execute
                                            if (PlaceSchedule)
                                            {
                                                //Create a new Sheet Schedule Intance on the Sheet based on the Schedule Selected and at the same location
                                                ScheduleSheetInstance.Create(doc, viewSheetId, ScheduleInstanceSelected.ScheduleId, ScheduleInstanceSelected.Point);
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
                                    TaskDialog.Show("Schedule Placement Error", ex.ToString());
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
                return Result.Cancelled;
            }
        }

        //Use the ISelectionFilter to filter which elemnts are selectable when using a UIDocument Selection method like PickObject
        private class ViewSelectionFilter : ISelectionFilter
        {
            //Return a bool if the element being "hovered" on passes the criteria
            public bool AllowElement(Element element)
            {
                //Check to see if the Element's category is 'Schedule Graphics' and then it can be selected
                if (element.Category.Name == "Schedule Graphics")
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
