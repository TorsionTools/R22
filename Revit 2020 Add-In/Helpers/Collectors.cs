using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Revit_2020_Add_In.Helpers
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
    }
}
