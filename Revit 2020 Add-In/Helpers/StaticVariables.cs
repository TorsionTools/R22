using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Autodesk.Revit.UI;

namespace TorsionTools
{
	class StaticVariables
	{
		//A global variable to hold the UI Controlled Application in
		public static UIControlledApplication revitApplication { get; set; }
		//A list of custom MapParam class objects to store the Parameter Mappings
		internal static List<WPF.MapParam> ParamMappings { get; set; } = null;
		//Retrieve the current version of the addplcation to append to Window Titles
		internal static string Version
		{
			get
			{
				return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
			}
		}
	}
}
