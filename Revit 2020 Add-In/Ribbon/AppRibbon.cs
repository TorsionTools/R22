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
            string TabName = "Your Tab";
            
            //Create the Ribbon Tab
            application.CreateRibbonTab(TabName);

            //Get the assembly path to execute commands
            string AssemblyPath = Assembly.GetExecutingAssembly().Location;

            //Create an Image to display on the buttons
            BitmapImage ButtonImage = new BitmapImage(new Uri("pack://application:,,,/Revit_2020_Add-In;component/Resources/Button100x100.png"));

            //Create a Panel within the Tab
            RibbonPanel RibbonPanelTest = application.CreateRibbonPanel(TabName, "TEST");

            //Create Push Button Data to creat the Push button from
            PushButtonData pbdTestButton = new PushButtonData("cmdTestButton", "Button Name", AssemblyPath, "Revit_2020_Add_In.Commands.Test");

            //Create a Push Button from the Push Button Data
            PushButton pbTestButton = RibbonPanelTest.AddItem(pbdTestButton) as PushButton;

            //Set Button Image
            pbTestButton.LargeImage = ButtonImage;

            //Set Button Tool Tip
            pbTestButton.ToolTip = "Tell the user what your button does here";

            //Set Button Long description which is the text that flys out when you hover on a button longer
            pbTestButton.LongDescription = "Give the user more information about how they need to use the button features";
        }
    }
}
