using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using View = Autodesk.Revit.DB.View;


namespace Revit_2020_Add_In.Commands
{
    [Transaction(TransactionMode.Manual)]
    class LinkedViewUpdate : IExternalCommand
    {
        Document doc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            doc = uiapp.ActiveUIDocument.Document;
            try
            {
                //Get all Linked Revit Models
                SortedList<string, Document> links = new SortedList<string, Document>();
                using (FilteredElementCollector rvtLinks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)))
                {
                    if (rvtLinks.ToElements().Count > 0)
                    {
                        foreach (RevitLinkType rvtLink in rvtLinks.ToElements())
                        {
                            if (rvtLink.GetLinkedFileStatus() == LinkedFileStatus.Loaded)
                            {
                                RevitLinkInstance link = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).Where(x => x.GetTypeId() == rvtLink.Id).First() as RevitLinkInstance;
                                links.Add(rvtLink.Name, link.GetLinkDocument());
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("No Revit Links","No Links are Loaded in the current Model");
                        return Result.Cancelled;
                    }
                }
                int itemCount = 0;

                //Check for any Views with Linked View Yes/No Parameter
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Update Linked View");
                    foreach (Viewport vp in Helpers.Collectors.ByCategoryNotElementType(doc, BuiltInCategory.OST_Viewports))
                    {
                        if (doc.GetElement(vp.ViewId) is View view)
                        {
                            if (view.ViewType == ViewType.DraftingView)
                            {
                                if (view.LookupParameter("Linked View") is Parameter lView)
                                {
                                    if (lView.AsInteger() == 1)
                                    {
                                        if (view.LookupParameter("Linked View GUID") is Parameter lGUID && view.LookupParameter("Link Name") is Parameter lName)
                                        {
                                            Document linkDoc = null;
                                            if (links.TryGetValue(lName.AsString(), out linkDoc))
                                            {
                                                ViewSheet vs = (ViewSheet)doc.GetElement(vp.SheetId);
                                                //Find the Viewport in the Linked Model using GUID and update / remove as needed
                                                if (linkDoc.GetElement(lGUID.AsString()) is Viewport linkVP)
                                                {
                                                    string linkDetail = linkVP.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).AsString();
                                                    string linkSheet = linkVP.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString();
                                                    string linkViewName = linkDoc.GetElement(linkVP.ViewId).Name;
                                                    string viewDetail = vp.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).AsString();
                                                    Parameter vpDetail = vp.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                                                    XYZ labelMin = linkVP.GetLabelOutline().MinimumPoint;

                                                    if (view.Name != linkViewName)
                                                    {
                                                        view.Name = linkViewName;
                                                    }

                                                    if (vs.SheetNumber == linkSheet)
                                                    {
                                                        if (vpDetail.AsString() != linkDetail)
                                                        {
                                                            vpDetail.Set(linkDetail);
                                                            itemCount++;
                                                        }

                                                        //Get the location of the Viewport and Viewport Label to update Viewort Location
                                                        if (vp.GetBoxCenter() != linkVP.GetBoxCenter())
                                                        {
                                                            vp.SetBoxCenter(labelMin);
                                                            ElementTransformUtils.MoveElement(doc, vp.Id, labelMin - vp.GetLabelOutline().MinimumPoint);
                                                            itemCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            vs.DeleteViewport(vp);
                                                            Viewport newVP = Viewport.Create(doc, CheckSheet(linkSheet, vs).Id, view.Id, labelMin);
                                                            ElementTransformUtils.MoveElement(doc, vp.Id, labelMin - vp.GetLabelOutline().MinimumPoint);
                                                            newVP.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).Set(linkDetail);
                                                            itemCount++;
                                                        }
                                                        catch
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (TaskDialog.Show("View Not Found","Linked View '" + view.Name + "' could not be found." + Environment.NewLine + "Would you like to delete the View?",TaskDialogCommonButtons.Yes|TaskDialogCommonButtons.No,TaskDialogResult.Yes) == TaskDialogResult.Yes)
                                                    {
                                                        doc.Delete(view.Id);
                                                    }
                                                    else
                                                    {
                                                        vs.DeleteViewport(vp);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                TaskDialog.Show("Missing Revit Link","Link '" + lName.AsString() + "' could not be found.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    TaskDialog.Show("Missing Parameters", "Linked View parameters are missing from this Document.");
                                    return Result.Failed;
                                }
                            }
                        }
                    }
                    t.Commit();
                    return Result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error",ex.ToString());
                return Result.Failed;
            }
        }

        //Check to see if Sheet exists
        private ViewSheet CheckSheet(string _sheetNumber, ViewSheet _vs)
        {
            try
            {
                ViewSheet sheet = null;
                ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.SHEET_NUMBER));
                FilterStringRuleEvaluator fsr = new FilterStringEquals();
                FilterRule fRule = new FilterStringRule(pvp, fsr, _sheetNumber, true);
                ElementParameterFilter filter = new ElementParameterFilter(fRule);

                if (new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WherePasses(filter).FirstOrDefault() is ViewSheet vs)
                {
                    sheet = vs;
                }
                else
                {
                    FamilyInstance tb = new FilteredElementCollector(doc, _vs.Id).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_TitleBlocks).FirstOrDefault() as FamilyInstance;
                    sheet = ViewSheet.Create(doc, tb.Symbol.Id);
                    sheet.Name = "DO NOT PRINT";
                    sheet.SheetNumber = _sheetNumber;
                    sheet.LookupParameter("Appears In Sheet List").Set(0);
                }
                return sheet;
            }
            catch
            {
                return null;
            }
        }
    }
}
