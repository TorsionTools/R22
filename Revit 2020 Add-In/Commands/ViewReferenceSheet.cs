using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Linq;

namespace TorsionTools.Commands
{
    //This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
    [Transaction(TransactionMode.Manual)]
    class ViewReferenceSheet : IExternalCommand
    {
        //Create public variable to store the current Document
        public static Document doc { get; set; }
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Document from the Current Session
            doc = commandData.Application.ActiveUIDocument.Document;
            //Get the User Interface Document from the current Document
            UIDocument uiDoc = new UIDocument(doc);
            //Create a Selection variable for picking one or more things from the model
            Selection picked = uiDoc.Selection;
            //Initialize a new instance of the ViewSelectionFilter Class to control what can be picked
            ISelectionFilter filter = new ViewSelectionFilter();
            //Use a Try block in case the user "Cancels" the selection it does not throw an Exception
            try
            {
                //Use the PickObject method to select one item. Include the Object Type and Selection Filter
                Reference selection = picked.PickObject(ObjectType.Element, filter, "Select Viewport on Sheet");
                //Check to see if the selection is inded a Viewport
                if (doc.GetElement(selection) is Autodesk.Revit.DB.Viewport ViewPort)
                {
                    //Get the View from the Viewport using the ViewId property
                    Autodesk.Revit.DB.View view = doc.GetElement(ViewPort.ViewId) as Autodesk.Revit.DB.View;
                    //Check to see if the View has a valid Referencing Sheet property
                    if (CheckSheet(view.get_Parameter(BuiltInParameter.VIEW_REFERENCING_SHEET).AsString()) is ViewSheet sheet)
                    {
                        //If the View has a Referencing Sheet property, make that sheet the Active View
                        uiDoc.ActiveView = sheet;
                    }
                }
                //Let Revit know it executed successfully. This is also how you can roll back the entire feature.
                return Result.Succeeded;
            }
            //Catch any exceptions in the Try block
            catch (Exception ex)
            {
                //Check to see if the Exception is an ArgumentNullException becuase the PickObject will throw this if the user "Escepes" the selection process
                if (ex.GetType() != typeof(Autodesk.Revit.Exceptions.ArgumentNullException))
                {
                    //If it is any other type of excpetion display a TaskDialog with the information
                    TaskDialog.Show("Selection Error", ex.ToString(), TaskDialogCommonButtons.Ok, TaskDialogResult.Ok);
                    //Return a failed result
                    return Result.Failed;
                }
                else
                {
                    //If the user cancels the selection, return a Cancalled result
                    return Result.Cancelled;
                }
            }
        }

        //This Method will return the ViewSheet by Sheet Number if it exists
        private ViewSheet CheckSheet(string _sheetNumber)
        {
            try
            {
                //Create the variables needed for an Element Filter to search for ONLY the one sheet
                ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NUMBER));
                FilterStringRuleEvaluator fsr = new FilterStringEquals();
                FilterRule fRule = new FilterStringRule(pvp, fsr, _sheetNumber, true);
                ElementParameterFilter filter = new ElementParameterFilter(fRule);
                //Use a Collector to search for only the One sheet with the supplied sheet number and if a ViewSheet is found, return it
                if (new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter).FirstOrDefault() is ViewSheet Sheet)
                {
                    return Sheet;
                }
                //If no sheet is found, return null
                return null;
            }
            catch
            {
                //If an exception is thrown in the Try block, return null
                return null;
            }
        }

        //The ISelectionFilter Interface provides a class to filter selections based two methods. AllowElement and AllowReference
        private class ViewSelectionFilter : ISelectionFilter
        {
            //Use this Method to determine if the method is allowed or not and return a bool
            public bool AllowElement(Element element)
            {
                if (element is Viewport vp)
                {
                    if (doc.GetElement(vp.ViewId) is Autodesk.Revit.DB.View view)
                    {
                        if (view.ViewType != ViewType.Legend)
                            return true;
                    }
                }
                return false;
            }

            //Use this to determine if refrence elements like points, lines, or planes can be selected
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
    }
}
