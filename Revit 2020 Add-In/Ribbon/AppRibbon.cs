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
            string TabName = "Mott MacDonald Add-In";
            
            //Create the Ribbon Tab
            application.CreateRibbonTab(TabName);

            //Get the assembly path to execute commands
            string AssemblyPath = Assembly.GetExecutingAssembly().Location;

            //Create an Image to display on the buttons
            BitmapImage ButtonImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/Button100x100.png"));
            BitmapImage SheetFindReplaceImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/FindReplace100x100.png"));
            BitmapImage SheetCapitalizeImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/ToUpper100x100.png"));
            BitmapImage SheetSelectionImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/SheetSelection100x100.png"));
            BitmapImage SheetTitleblockKeyPlanImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/SheetTitleblockKeyPlan100x100.png"));


            //Create a Panel within the Tab
            RibbonPanel RibbonPanelOne = application.CreateRibbonPanel(TabName, "PANEL 1");
            RibbonPanel RibbonPanelSheets = application.CreateRibbonPanel(TabName, "Sheets");

            //Create Push Button Data to create the Push button from
            PushButtonData pbdTestButton = new PushButtonData("cmdTestButton", "Button Name", AssemblyPath, "Revit_2020_Add_In.Commands.HelloWorld");
            PushButtonData pbdSheetSelection = new PushButtonData("cmdSheetSelection", "Sheet\nSelector", AssemblyPath, "Revit_2020_Add_In.Commands.SheetSelection");
            PushButtonData pbdSheetFindReplace = new PushButtonData("cmdSheetFindReplace", "Find Replace", AssemblyPath, "Revit_2020_Add_In.Commands.SheetFindReplace");
            PushButtonData pbdSheetNameCapitalize = new PushButtonData("cmdSheetNameCapitalize", "Capitalize\nName", AssemblyPath, "Revit_2020_Add_In.Commands.SheetNameCapitalize");
            PushButtonData pbdSheetTitleblockKeyPlan = new PushButtonData("cmdSheetTitleblockKeyPlan", "Key Plan\nVisibility", AssemblyPath, "Revit_2020_Add_In.Commands.SheetTitleblockKeyPlan");

            //Create a Push Button from the Push Button Data
            PushButton pbTestButton = RibbonPanelOne.AddItem(pbdTestButton) as PushButton;
            PushButton pbSheetSelection = RibbonPanelSheets.AddItem(pbdSheetSelection) as PushButton;
            PushButton pbSheetNameCapitalize = RibbonPanelSheets.AddItem(pbdSheetNameCapitalize) as PushButton;
            PushButton pbSheetFindReplace = RibbonPanelSheets.AddItem(pbdSheetFindReplace) as PushButton;
            PushButton pbSheetTitleblockKeyPlan = RibbonPanelSheets.AddItem(pbdSheetTitleblockKeyPlan) as PushButton;

            //Set Button Image
            pbTestButton.LargeImage = ButtonImage;
            pbSheetNameCapitalize.LargeImage = SheetCapitalizeImage;
            pbSheetFindReplace.LargeImage = SheetFindReplaceImage;
            pbSheetSelection.LargeImage = SheetSelectionImage;
            pbSheetTitleblockKeyPlan.LargeImage = SheetTitleblockKeyPlanImage;

            //Set Button Tool Tip
            pbTestButton.ToolTip = "Tell the user what your button does here";
            pbSheetSelection.ToolTip = "Select from all of the Sheets in the Model";
            pbSheetFindReplace.ToolTip = "Find and Replace values in Sheet Name or Number";
            pbSheetNameCapitalize.ToolTip = "Capitalize the Name of all Sheets in the Model";
            pbSheetTitleblockKeyPlan.ToolTip = "Set Yes / No parameters of a Titleblock type based on search criteria of the Sheet Name or Sheet Number";

            //Set Button Long description which is the text that flys out when you hover on a button longer
            pbTestButton.LongDescription = "Give the user more information about how they need to use the button features";
            pbSheetSelection.LongDescription = "The Sheet Selection Form can be used in multiple ways to allow the user to select one or multiple sheets in the project and perform additiona actions on the sheets returned.";
        }
    }
}
