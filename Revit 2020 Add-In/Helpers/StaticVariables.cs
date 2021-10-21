using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace TorsionTools
{
	class StaticVariables
	{
		public static UIControlledApplication revitApplication { get; set; }
		//A list of custom MapParam class objects to store the Parameter Mappings
		internal static List<MapParam> ParamMappings { get; set; } = null;
	}
}
