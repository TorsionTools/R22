using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace TorsionTools.Helpers
{
	class Collectors
	{
		internal static IList<Element> ByCategory(Document doc, BuiltInCategory category)
		{
			IList<Element> elements;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category))
			{
				elements = fec.ToElements();
			}
			return elements;
		}

		internal static IList<Element> ByCategoryElementType(Document doc, BuiltInCategory category)
		{
			IList<Element> elements;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category).WhereElementIsElementType())
			{
				elements = fec.ToElements();
			}
			return elements;
		}

		internal static IList<Element> ByCategoryNotElementType(Document doc, BuiltInCategory category)
		{
			IList<Element> elements;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category).WhereElementIsNotElementType())
			{
				elements = fec.ToElements();
			}
			return elements;
		}

		internal static IList<Element> ByCategoryNotElementTypeByView(Document doc, ElementId viewId, BuiltInCategory category)
		{
			IList<Element> elements;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc, viewId).OfCategory(category).WhereElementIsNotElementType())
			{
				elements = fec.ToElements();
			}
			return elements;
		}

		internal static ICollection<ElementId> IdsByCategory(Document doc, BuiltInCategory category)
		{
			ICollection<ElementId> elementIds;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory(category))
			{
				elementIds = fec.ToElementIds();
			}
			return elementIds;
		}

		internal static ElementId GetViewTypeIdByViewType(Document doc, ViewFamily viewFamily, string typeName)
		{
			ElementId elementId;
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)))
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
			using(FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(filter))
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

			if(new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Schedules).WherePasses(filter).FirstOrDefault() is ViewSchedule vs)
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

			if(new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Views).WherePasses(filter).FirstOrDefault() is View lg)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		/// <summary>
		/// Read an xml file to provide <see cref="MapParam"/> Parameter Mapping for 
		/// parameters changes in the document and not Code behind	for Parameter Lookups
		/// </summary>
		/// <param name="_parent">Top level in the xml file with multiple Descendants</param>
		/// <param name="_desc">The Decendents within the specified Parent</param>
		/// <param name="_child1">The child of the Descendant by name for code Parameter Name</param>
		/// <param name="_child2">The child of the Descendant by name for code 
		/// Parameter Value in the Document</param>
		/// <param name="_child3">The Child of the Descendant by name for the Description 
		/// of the Parameter</param>
		/// <returns><see cref="List{T}"/> where T is <see cref="MapParam"/></returns>
		internal static List<WPF.MapParam> GetParameterMappings(string _parent, string _desc, string 
			_child1, string _child2, string _child3 = "Description")
		{
			try
			{
				//List of MapParam elements to store the values retrieved from the XML file
				List<WPF.MapParam> Params = new List<WPF.MapParam>();
				//Load the XML document from the path relative to the Executing Assembly or this application
				XDocument xDoc = XDocument.Load(Path.Combine(Path.GetDirectoryName(
					Assembly.GetExecutingAssembly().Location),@"Resources\ParameterMappings.xml"));
				//Loop through each descendant of the Parent
				foreach(XElement Elem in xDoc.Descendants(_parent))
				{
					//Loop through each descendant of the first descendant
					foreach(XElement ElemDesc in Elem.Descendants(_desc))
					{
						//Get the child element of each 2nd descendant and the associated value
						Params.Add(new WPF.MapParam() { Name = ElemDesc.Element(_child1).Value, 
							Model = ElemDesc.Element(_child2).Value, 
							Description = ElemDesc.Element(_child3).Value });
					}
				}
				//Return the List of MapParam objects
				return Params;
			}
			catch(Exception ex)
			{
				TaskDialog.Show("Parameter Mapping Error", ex.ToString());
				return null;
			}
		}
	}
}
