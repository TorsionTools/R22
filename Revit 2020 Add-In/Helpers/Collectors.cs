using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace TorsionTools.Helpers
{
    class Collectors
    {
        internal static IList<Element> ByCategory(Document doc, BuiltInCategory category)
        {
            IList<Element> elements;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category))
            {
                elements = fec.ToElements();
            }
            return elements;
        }

        internal static IList<Element> ByCategoryElementType(Document doc, BuiltInCategory category)
        {
            IList<Element> elements;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category).WhereElementIsElementType())
            {
                elements = fec.ToElements();
            }
            return elements;
        }

        internal static IList<Element> ByCategoryNotElementType(Document doc, BuiltInCategory category)
        {
            IList<Element> elements;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category).WhereElementIsNotElementType())
            {
                elements = fec.ToElements();
            }
            return elements;
        }

        internal static IList<Element> ByCategoryNotElementTypeByView(Document doc, ElementId viewId, BuiltInCategory category)
        {
            IList<Element> elements;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc, viewId).OfCategory(category).WhereElementIsNotElementType())
            {
                elements = fec.ToElements();
            }
            return elements;
        }

        internal static ICollection<ElementId> IdsByCategory(Document doc, BuiltInCategory category)
        {
            ICollection<ElementId> elementIds;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category))
            {
                elementIds = fec.ToElementIds();
            }
            return elementIds;
        }

        internal static ElementId GetViewTypeIdByViewType(Document doc, ViewFamily viewFamily, string typeName)
        {
            ElementId elementId;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)))
            {
                elementId = fec.Cast<ViewFamilyType>().Where(x => x.ViewFamily == viewFamily && x.Name == typeName).FirstOrDefault().Id;
            }
            return elementId;
        }

        internal static IList<ElementType> ViewportTypes(Document doc)
        {
            FilterRule rule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId((int)BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "Viewport", false);
            ElementParameterFilter filter = new ElementParameterFilter(rule);
            IList<ElementType> viewportTypes;
            using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(filter))
            {
                viewportTypes = fec.Cast<ElementType>().ToList();
            }
            return viewportTypes;
        }

        //This method will check to see if there are any Schedules in the specified document with the same name
        internal static bool CheckSchedule(Document _doc, string _name)
        {
            ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.VIEW_NAME));
            FilterStringRuleEvaluator fsr = new FilterStringEquals();
            FilterRule fRule = new FilterStringRule(pvp, fsr, _name, true);
            ElementParameterFilter filter = new ElementParameterFilter(fRule);

            if (new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Schedules).WherePasses(filter).FirstOrDefault() is ViewSchedule vs)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //This method will check to see if there are any Legends in the specified document with the same name
        internal static bool CheckLegend(Document _doc, string _name)
        {
            ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(BuiltInParameter.VIEW_NAME));
            FilterStringRuleEvaluator fsr = new FilterStringEquals();
            FilterRule fRule = new FilterStringRule(pvp, fsr, _name, true);
            ElementParameterFilter filter = new ElementParameterFilter(fRule);

            if (new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Views).WherePasses(filter).FirstOrDefault() is View lg)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
