using System.Windows;

namespace TorsionTools.WPF
{
	/// <summary>
	/// Interaction logic for ParameterMappingWPF.xaml
	/// </summary>
	public partial class ParameterMappingWPF : Window
	{
		ParamMapViewModel vm = new ParamMapViewModel();
		public ParameterMappingWPF()
		{
			InitializeComponent();
			DataContext = vm;
		}

		private void btnUpdateMapping_Click(object sender, RoutedEventArgs e)
		{
			StaticVariables.ParamMappings = vm.ParamMap;
			Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Title += " (" + StaticVariables.Version + ")";
			vm.ParamMap = StaticVariables.ParamMappings;
		}
	}
}
