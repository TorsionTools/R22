using System.ComponentModel;
using PropertyChanged;

namespace TorsionTools.WPF
{
	//Using Fody.PropertyChanged Nuget to facilitate property changed on public properties
	[AddINotifyPropertyChangedInterface]
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

		public void OnPropertyChanged(string name)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}
}
