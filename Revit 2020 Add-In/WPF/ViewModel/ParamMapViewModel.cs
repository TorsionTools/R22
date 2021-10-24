using System.Collections.Generic;
using System.ComponentModel;

namespace TorsionTools.WPF
{	
	/// <summary>
	/// Main View Model for the Parameter Mapping Window
	/// </summary>
	internal class ParamMapViewModel : BaseViewModel
	{	
		//List of MapParam classes to hold in the information gathered from the XML and linked to DataGrid
		public List<MapParam> ParamMap { get; set; }
	}
	/// <summary>
	/// This is a Container class for the Parameter element class for Mapping project parameters to the code
	/// </summary>
	public class MapParam : INotifyPropertyChanged
	{
		//The Property Changed event is used for ViewModel integration 
		public event PropertyChangedEventHandler PropertyChanged;
		//The private property for the Project Parameter
		private string _Model;
		//The public property for the Name of the parameter in the code behind
		public string Name { get; set; }
		//The publi property for the Project Parameter with the NotifyPropertyChanged event attached
		public string Model
		{
			get => _Model;
			set
			{
				_Model = value;
				NotifyPropertyChanged();
			}
		}
		//The public property for the description of where the parameter is used in the code
		public string Description { get; set; }
		//The method that fires the property changed event to update ViewModel bindings when used
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}