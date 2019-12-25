using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Revit_2020_Add_In
{
    class App :IExternalApplication
    {
        //This is the Function that tells the Revit application to do something when Revit starts.
        public Result OnStartup(UIControlledApplication RevitApplication)
        {
            //Create the Ribbon Panel and Tabs when the Add-In is started.
            Ribbon.AppRibbon.AddRibbonPanel(RevitApplication);

            //Do something when Revit opens
            TaskDialog.Show("Opening", "Revit is Opening!");

            //Let Revit know it was successfully executed
            return Result.Succeeded;
        }

        //This is the Function that tells the Revit application to do something when Revit closes.
        public Result OnShutdown(UIControlledApplication application)
        {
            //Do something here when Revit closes
            TaskDialog.Show("Closing", "Revit is Closing!");

            //Let Revit know it was successfully executed
            return Result.Succeeded;
        }
    }
}
