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
            BitmapImage SheetFindReplaceImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/FindReplace100x100.png"));
            BitmapImage SheetCapitalizeImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/ToUpper100x100.png"));
            BitmapImage SheetSelectionImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/SheetSelection100x100.png"));
            BitmapImage SheetTitleblockKeyPlanImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/SheetTitleblockKeyPlan100x100.png"));
            BitmapImage ToolsWarningsImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/Warning100x100.png"));
            BitmapImage ToolsElemOfCategoryImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/ComponentInfo100x100.png"));
            BitmapImage ToolsViewLegendCopyImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/ViewLegendCopy100x100.png"));
            BitmapImage ToolsLinkedViewsImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/LinkedView100x100.png"));
            BitmapImage ToolsLinkedViewsUpdateImage = new BitmapImage(new Uri("pack://application:,,,/Revit 2020 Add-In;component/Resources/LinkedViewUpdate100x100.png"));

            //Create a Panel within the Tab
            RibbonPanel RibbonPanelOne = application.CreateRibbonPanel(TabName, "PANEL 1");
            RibbonPanel RibbonPanelSheets = application.CreateRibbonPanel(TabName, "Sheets");
            RibbonPanel RibbonPanelTools = application.CreateRibbonPanel(TabName, "Tools");

            //Create Push Button Data to create the Push button from
            PushButtonData pbdTestButton = new PushButtonData("cmdTestButton", "Button Name", AssemblyPath, "Revit_2020_Add_In.Commands.HelloWorld");

            PushButtonData pbdSheetSelection = new PushButtonData("cmdSheetSelection", "Sheet\nSelector", AssemblyPath, "Revit_2020_Add_In.Commands.SheetSelection");
            PushButtonData pbdSheetFindReplace = new PushButtonData("cmdSheetFindReplace", "Find Replace", AssemblyPath, "Revit_2020_Add_In.Commands.SheetFindReplace");
            PushButtonData pbdSheetNameCapitalize = new PushButtonData("cmdSheetNameCapitalize", "Capitalize\nName", AssemblyPath, "Revit_2020_Add_In.Commands.SheetNameCapitalize");
            PushButtonData pbdSheetTitleblockKeyPlan = new PushButtonData("cmdSheetTitleblockKeyPlan", "Key Plan\nVisibility", AssemblyPath, "Revit_2020_Add_In.Commands.SheetTitleblockKeyPlan");

            PushButtonData pbdToolsWarnings = new PushButtonData("cmdToolsWarnings", "Warnings", AssemblyPath, "Revit_2020_Add_In.Commands.Warnings");
            PushButtonData pbdToolsElemOfCategory = new PushButtonData("cmdToolsElemOfCategory", "Family Instances\nof Category", AssemblyPath, "Revit_2020_Add_In.Commands.ElementsOfCategory");
            PushButtonData pbdToolsViewLegendCopy = new PushButtonData("cmdToolsViewLegendCopy", "Copy\nLegends", AssemblyPath, "Revit_2020_Add_In.Commands.ViewLegendCopy");
            PushButtonData pbdToolsLinkedViews = new PushButtonData("cmdToolsLinkedViews", "Linked\nViews", AssemblyPath, "Revit_2020_Add_In.Commands.LinkedViews");
            PushButtonData pbdToolsLinkedViewsUpdate = new PushButtonData("cmdToolsLinkedViewsUpdate", "Update\nViews", AssemblyPath, "Revit_2020_Add_In.Commands.LinkedViewUpdate");

            //Create a Push Button from the Push Button Data
            PushButton pbTestButton = RibbonPanelOne.AddItem(pbdTestButton) as PushButton;

            PushButton pbSheetSelection = RibbonPanelSheets.AddItem(pbdSheetSelection) as PushButton;
            PushButton pbSheetNameCapitalize = RibbonPanelSheets.AddItem(pbdSheetNameCapitalize) as PushButton;
            PushButton pbSheetFindReplace = RibbonPanelSheets.AddItem(pbdSheetFindReplace) as PushButton;
            PushButton pbSheetTitleblockKeyPlan = RibbonPanelSheets.AddItem(pbdSheetTitleblockKeyPlan) as PushButton;

            PushButton pbToolsWarnings = RibbonPanelTools.AddItem(pbdToolsWarnings) as PushButton;
            PushButton pbToolsElemOfCategory = RibbonPanelTools.AddItem(pbdToolsElemOfCategory) as PushButton;
            PushButton pbToolsViewLegendCopy = RibbonPanelTools.AddItem(pbdToolsViewLegendCopy) as PushButton;
            
            //If you are going to use PushButtonData objects for Pulldown or Split buttons, you have to set these properties BEFORE adding them
            pbdToolsLinkedViews.LargeImage = ToolsLinkedViewsImage;
            pbdToolsLinkedViewsUpdate.LargeImage = ToolsLinkedViewsUpdateImage;
            pbdToolsLinkedViews.ToolTip = "Create Drafting Views based on Views in a Linked Model for reference";
            pbdToolsLinkedViewsUpdate.ToolTip = "Update Linked View information referenced from a Linked Model";
            pbdToolsLinkedViews.LongDescription = "Verify the current Revit Model has the following three parameters under Phasing paramter group and View category:\nLinked View - Yes/No\nLinked View GUID - Text\nLink Name - Text ";

            //Pull Down buttons allow you to stack similar or grouped buttons into a stack that you can expand down and select
            PulldownButtonData pdbdLinkedViews = new PulldownButtonData("pullDownLinkView", "Link\nViews");
            PulldownButton pdbLinkedViews = RibbonPanelTools.AddItem(pdbdLinkedViews) as PulldownButton;
            pdbLinkedViews.LargeImage = ToolsLinkedViewsImage;
            pdbLinkedViews.AddPushButton(pbdToolsLinkedViews);
            pdbLinkedViews.AddPushButton(pbdToolsLinkedViewsUpdate);

            //Set Button Image
            pbTestButton.LargeImage = ButtonImage;

            pbSheetNameCapitalize.LargeImage = SheetCapitalizeImage;
            pbSheetFindReplace.LargeImage = SheetFindReplaceImage;
            pbSheetSelection.LargeImage = SheetSelectionImage;
            pbSheetTitleblockKeyPlan.LargeImage = SheetTitleblockKeyPlanImage;

            pbToolsWarnings.LargeImage = ToolsWarningsImage;
            pbToolsViewLegendCopy.LargeImage = ToolsViewLegendCopyImage;
            pbToolsElemOfCategory.LargeImage = ToolsElemOfCategoryImage;
            

            //Set Button Tool Tips
            pbTestButton.ToolTip = "Tell the user what your button does here";
            pbSheetSelection.ToolTip = "Select from all of the Sheets in the Model";
            pbSheetFindReplace.ToolTip = "Find and Replace values in Sheet Name or Number";
            pbSheetNameCapitalize.ToolTip = "Capitalize the Name of all Sheets in the Model";
            pbSheetTitleblockKeyPlan.ToolTip = "Set Yes / No parameters of a Titleblock type based on search criteria of the Sheet Name or Sheet Number";

            pbToolsWarnings.ToolTip = "Display and isolate Warnings in the Document";
            pbToolsElemOfCategory.ToolTip = "Get all Elements of selected Category";
            pbToolsViewLegendCopy.ToolTip = "Copy one or more Legend Views from a Linked Document";

            //Set Button Long description which is the text that flys out when you hover on a button longer
            pbTestButton.LongDescription = "Give the user more information about how they need to use the button features";
            pbToolsViewLegendCopy.LongDescription = "Select the Linked Document from which you want to copy the Legend from. Then select from the available Legend views. Then press the Copy button to copy the legends into the current Document.";
            pbSheetSelection.LongDescription = "The Sheet Selection Form can be used in multiple ways to allow the user to select one or multiple sheets in the project and perform additiona actions on the sheets returned.";
            
        }
    }
}
