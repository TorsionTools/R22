namespace TorsionTools
{
	internal static class Map
	{
		/// <summary>
		/// Search for Parameter Mappings from the resource XML file
		/// </summary>
		/// <param name="name"><see cref="string"/> value for the code behind Name of the parameter in the XML file</param>
		/// <returns><see cref="string"/> value for the Project Parameter name</returns>
		internal static string Find(string name)
		{
			//Match a Name to the Parameter in the ParameterMapping.xml file
			if(StaticVariables.ParamMappings.Find(x => x.Name == name) is WPF.MapParam param)
			{
				return param.Name;
			}
			else
			{
				return null;
			}
		}
	}
}
