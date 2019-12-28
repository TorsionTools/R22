using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace Revit_2020_Add_In.Ribbon
{
    class AppRibbon
    {
        internal static void AddRibbonPanel(UIControlledApplication application)
        {
            //Tab Name that will display in Revit
            string TabName = "My Tab";
            
            //Create the Ribbon Tab
            application.CreateRibbonTab(TabName);

            //Get the assembly path to execute commands
            string AssemblyPath = Assembly.GetExecutingAssembly().Location;

            //Create an Image to display on the buttons
            BitmapImage ButtonImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/Button100x100.png"));

            //Create a Panel within the Tab
            RibbonPanel RibbonPanelOne = application.CreateRibbonPanel(TabName, "PANEL 1");
            RibbonPanel RibbonPanelTwo = application.CreateRibbonPanel(TabName, "PANEL 2");

            //Create Push Button Data to create the Push button from
            PushButtonData pbdTestButton = new PushButtonData("cmdTestButton", "Button Name", AssemblyPath, "Revit_2020_Add_In.Commands.HelloWorld");
            PushButtonData pbdSheetSelection = new PushButtonData("cmdSheetSelection", "Sheet" + Environment.NewLine + "Selector", AssemblyPath, "Revit_2020_Add_In.Commands.SheetSelection");

            //Create a Push Button from the Push Button Data
            PushButton pbTestButton = RibbonPanelOne.AddItem(pbdTestButton) as PushButton;
            PushButton pbSheetSelection = RibbonPanelTwo.AddItem(pbdSheetSelection) as PushButton;

            //Set Button Image
            pbTestButton.LargeImage = ButtonImage;

            //Set Button Tool Tip
            pbTestButton.ToolTip = "Tell the user what your button does here";
            pbSheetSelection.ToolTip = "Select from all of the Sheets in the project";

            //Set Button Long description which is the text that flys out when you hover on a button longer
            pbTestButton.LongDescription = "Give the user more information about how they need to use the button features";
            pbSheetSelection.LongDescription = "The Sheet Selection Form can be used in multiple ways to allow the user to select one or multiple sheets in the project and perform additiona actions on the sheets returned.";
        }
    }
}
