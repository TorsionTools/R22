using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace TorsionTools.Commands
{
	//This allows us to create transactions within our code, but also allows us to roll the entire command back if the Result is Failed or Cancelled
	[Transaction(TransactionMode.Manual)]
    //Change the Class Name to something other than 'TEMPLATE'
    class ParamMapping : IExternalCommand
    {
        //This line has to be here in order for the command to execute in the current Revit context
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the Current Session / Project from Revit
            UIApplication uiapp = commandData.Application;

            //Get the Current Document from the Current Session
            Document doc = uiapp.ActiveUIDocument.Document;

			//Create a new instance of the WPF form
			WPF.ParameterMappingWPF form = new WPF.ParameterMappingWPF();

			//Use the Main Windo Handle and Window Helper to make the Revit window the Owner. THis will
			//prevent task dialogs from going behind the main window or losing focus
			Helpers.JtWindowHandle rvtwin = new Helpers.JtWindowHandle(commandData.Application.MainWindowHandle);
			//THis class specifically helps WPF coordinate with Win Form properties
			System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(form)
			{
				Owner = rvtwin.Handle
			};

			//Show the form as a dialog and check the return value
			if(form.ShowDialog().Value)
			{
				//Let Revit know it executed successfully. This is also how you can roll back the entire feature.
				return Result.Succeeded;
			}
			else
			{
				//Let Revit know it was cancelled and all changes will be automatically rolled back if any were made.
				return Result.Cancelled;
			}
		}
    }
}
