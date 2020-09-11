using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using View = Autodesk.Revit.DB.View;


namespace Revit_2020_Add_In.Commands
{
    //This Attribute sets how transactions are handled. The Manual Transaction Mode allows us to control any 
    //transactions individually, but roll the entire function back if we return any Result other than Succeded
    [Transaction(TransactionMode.Manual)]
    //This class will call the View Legend Copy form copy legends from a Linked Document
    class LinkedViewUpdate : IExternalCommand
    {
        //Create a Class variable for a Document
        Document doc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            //Set the Class variable to the Executing Document
            doc = uiapp.ActiveUIDocument.Document;
            //Wrap the main functions in a Try block to catch Exceptions and help prevent Revit from crashing
            try
            {
                //initialize a new SortedList to hold all Revit Links
                SortedList<string, Document> links = new SortedList<string, Document>();
                //Filtered Element Collector to get all Revit Link Types in the Document
                using (FilteredElementCollector rvtLinks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
                {
                    //Check to make sure there is at least one link
                    if (rvtLinks.ToElements().Count > 0)
                    {
                        //Loop Each Revit Link Type
                        foreach (RevitLinkType rvtLink in rvtLinks.ToElements())
                        {
                            //Check to see if the Link is currently Loaded 
                            if (rvtLink.GetLinkedFileStatus() == LinkedFileStatus.Loaded)
                            {
                                //Create a Filter Rule and Elemenet Parameter Filter for the Revit Link Instance Collector to Match the Type
                                FilterRule rule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_ID_PARAM),rvtLink.get_Parameter(BuiltInParameter.ID_PARAM).AsElementId());
                                ElementParameterFilter filter = new ElementParameterFilter(rule);
                                //In order to get the Document, we have to get the first or default Revit Link Instance that is of the Revit Link Type
                                RevitLinkInstance link = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).WherePasses(filter).First() as RevitLinkInstance;
                                //Add the Link Name and Link Document to the SortedList
                                links.Add(rvtLink.Name, link.GetLinkDocument());
                            }
                        }
                    }
                    //Aler the user that the current Document doesn't have any links
                    else
                    {
                        TaskDialog.Show("No Revit Links", "No Links are Loaded in the current Model");
                        //Stops the Method and returns a Cancelled result
                        return Result.Cancelled;
                    }
                }

                //Create a new Transaction within a using group to make any updates if needed
                using (Transaction t = new Transaction(doc))
                {
                    //Start the Transaction and give it a Name for the Undo / Redo Menu
                    t.Start("Update Linked View");
                    //Loop through All viewports in the current document
                    foreach (Viewport vp in Helpers.Collectors.ByCategoryNotElementType(doc, BuiltInCategory.OST_Viewports))
                    {
                        //Get the View from the Viewport using the ViewId property
                        if (doc.GetElement(vp.ViewId) is View view)
                        {
                            //Filter to only Drafting View types which are what the LinkedViews Method creates
                            if (view.ViewType == ViewType.DraftingView)
                            {
                                //Check to see if the Linked View Yes/No Parameter exists get it
                                if (view.LookupParameter("Linked View") is Parameter lView)
                                {
                                    //Check to see if the Linked View parameter Yes/No parameter is Checked Yes
                                    if (lView.AsInteger() == 1)
                                    {
                                        //Check if the Linked View GUID and Link Name parameters exists and get them as variables
                                        if (view.LookupParameter("Linked View GUID") is Parameter lGUID && view.LookupParameter("Link Name") is Parameter lName)
                                        {
                                            //Create a Document variable to hold the Link Document from the SortedList
                                            Document linkDoc = null;
                                            //Check to see if the Link Name exists in the SortedList and get the Document out if so
                                            if (links.TryGetValue(lName.AsString(), out linkDoc))
                                            {
                                                //Use the Viewport to get the Sheet from the SheetId property
                                                ViewSheet vs = (ViewSheet)doc.GetElement(vp.SheetId);
                                                //Find the Viewport in the Linked Model using GUID and get the Viewport element
                                                if (linkDoc.GetElement(lGUID.AsString()) is Viewport linkVP)
                                                {
                                                    //Get Detail number and Sheet Numbers from the Linked Viewport
                                                    string linkDetail = linkVP.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).AsString();
                                                    string linkSheet = linkVP.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString();
                                                    //Get the Linked View name
                                                    string linkViewName = linkDoc.GetElement(linkVP.ViewId).Name;
                                                    //Get the Viewport Number and Viewport Number Paramter from the current Viewport
                                                    string viewDetail = vp.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).AsString();
                                                    Parameter vpDetail = vp.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                                                    //Get the Minimum (lower left) point of the Viewport Label (Title) of the Current Viewport
                                                    XYZ labelMin = linkVP.GetLabelOutline().MinimumPoint;

                                                    //Check to see if the View Name has changed and update it if needed
                                                    if (view.Name != linkViewName)
                                                    {
                                                        view.Name = linkViewName;
                                                    }

                                                    //Check to see if the Viewport is still on the corret Sheet Number
                                                    if (vs.SheetNumber == linkSheet)
                                                    {
                                                        if (vpDetail.AsString() != linkDetail)
                                                        {
                                                            vpDetail.Set(linkDetail);
                                                        }

                                                        //Get the location of the Viewport and Viewport Label to update Viewort Location
                                                        if (vp.GetBoxCenter() != linkVP.GetBoxCenter())
                                                        {
                                                            //Move the Viewport to the Label Minimum Point
                                                            vp.SetBoxCenter(labelMin);
                                                            //Move the viewport a relative amount from the Viewport Label to the Minimum point of the Viewport
                                                            ElementTransformUtils.MoveElement(doc, vp.Id, labelMin - vp.GetLabelOutline().MinimumPoint);
                                                        }
                                                    }
                                                    //If the Linked Viewport Sheet Number is different
                                                    else
                                                    {
                                                        try
                                                        {
                                                            //Remove the Viewport from the sheet it is on
                                                            vs.DeleteViewport(vp);
                                                            //Move the viewport to the new sheet number
                                                            Viewport newVP = Viewport.Create(doc, CheckSheet(linkSheet, vs).Id, view.Id, labelMin);
                                                            //Move it to the correct location based on the Linked View location
                                                            ElementTransformUtils.MoveElement(doc, vp.Id, labelMin - vp.GetLabelOutline().MinimumPoint);
                                                            //Set the Viewport Detail Number to match the Linked Viewport
                                                            newVP.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).Set(linkDetail);
                                                        }
                                                        catch
                                                        {
                                                            //If there is an exception, just keep moving
                                                            continue;
                                                        }
                                                    }
                                                }
                                                //If the View cannot be found in the link, the User is asked if they want to delete the view from the project. If they Keep it, the viewport will be removed from the Sheet
                                                else
                                                {
                                                    if (TaskDialog.Show("View Not Found", "Linked View '" + view.Name + "' could not be found." + Environment.NewLine + "Would you like to delete the View?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogResult.Yes) == TaskDialogResult.Yes)
                                                    {
                                                        //Delete the View from the project
                                                        doc.Delete(view.Id);
                                                    }
                                                    else
                                                    {
                                                        //Remove the Viewport from the Sheet
                                                        vs.DeleteViewport(vp);
                                                    }
                                                }
                                            }
                                            //Alert the User that the Link Name doesn't match any current Links in the Proeject
                                            else
                                            {
                                                TaskDialog.Show("Missing Revit Link", "Link '" + lName.AsString() + "' could not be found.");
                                            }
                                        }
                                    }
                                }
                                //ALert the user if the Linked View Yes/No parameter is missing from the project
                                else
                                {
                                    TaskDialog.Show("Missing Parameters", "Linked View parameters are missing from this Document.");
                                    //Method cannot continue if the parameters are missing so return a Failed result
                                    return Result.Failed;
                                }
                            }
                        }
                    }
                    //Commit the Transaction to Keep any Changes
                    t.Commit();
                    //Return a Succeeded Result to keep the changes
                    return Result.Succeeded;
                }
            }
            //Catch any Exceptions encountered and provide the user with the Details
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString());
                //Method cannot continue so return a Failed result
                return Result.Failed;
            }
        }

        //Check to see if Sheet exists using the unique Sheet Number 
        private ViewSheet CheckSheet(string _sheetNumber, ViewSheet _vs)
        {
            try
            {
                //Create an Empty ViewSheet object to return if sheet is found
                ViewSheet sheet = null;
                //Use Parameter Value Provider, Filter Rules, and Element Filters to narrow the results of the Element Collector in the most efficient way
                ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NUMBER));
                FilterStringRuleEvaluator fsr = new FilterStringEquals();
                //Check that the PVP (Sheet Number) is equal to the Sheet Number passed to the Method
                FilterRule fRule = new FilterStringRule(pvp, fsr, _sheetNumber, true);
                //This is the total Filter to use on the Collector
                ElementParameterFilter filter = new ElementParameterFilter(fRule);

                //Use an Elemeent Collector to get the sheet with the Sheet Number passed to the Method
                if (new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter).FirstOrDefault() is ViewSheet vs)
                {
                    //Return the ViewSheet Obeject if there is one that matches in the current document
                    sheet = vs;
                }
                //If there is not a Sheet with the Sheet Number passed, create one
                else
                {
                    //Get the Titleblock Type from the sheet it was on and use for the new Sheet
                    FamilyInstance tb = new FilteredElementCollector(doc, _vs.Id).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_TitleBlocks).FirstOrDefault() as FamilyInstance;
                    //Create a new Sheet with the Titleblock
                    sheet = ViewSheet.Create(doc, tb.Symbol.Id);
                    //Set the Sheet Name, Number, and Appears In Sheet List parameters 
                    sheet.Name = "DO NOT PRINT";
                    sheet.SheetNumber = _sheetNumber;
                    sheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED).Set(0);
                }
                //Return either the existing Sheet or the New Sheet
                return sheet;
            }
            catch
            {
                //Return a null sheet if there is an error
                return null;
            }
        }
    }
}
