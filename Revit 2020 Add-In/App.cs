using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;

namespace Revit_2020_Add_In
{
    class App : IExternalApplication
    {
        //This is the Function that tells the Revit application to do something when Revit starts.
        public Result OnStartup(UIControlledApplication RevitApplication)
        {
            //Create the Ribbon Panel and Tabs when the Add-In is started.
            Ribbon.AppRibbon.AddRibbonPanel(RevitApplication);

            //Register the Dynamic Updaters
            RegisterUpdaters(RevitApplication);

            //Add an event handler that will do something when the Document has finished Synchronizing
            RevitApplication.ControlledApplication.DocumentSynchronizedWithCentral += new EventHandler<Autodesk.Revit.DB.Events.DocumentSynchronizedWithCentralEventArgs>(Application_DocumentSynchronized);

            //Store the UI Controlled Application for Disabling and Enabling Updaters
            Helpers.StaticVariables.revitApplication = RevitApplication;

            //Let Revit know it was successfully executed
            return Result.Succeeded;
        }

        //This is the Function that tells the Revit application to do something when Revit closes.
        public Result OnShutdown(UIControlledApplication RevitApplication)
        {
            //Unregister the Dynmic Updaters
            UnregisterUpdaters(RevitApplication);

            //Remove the Event Handler for Document Synchronized
            RevitApplication.ControlledApplication.DocumentSynchronizedWithCentral -= Application_DocumentSynchronized;

            //Let Revit know it was successfully executed
            return Result.Succeeded;
        }

        //Do somethind when a Document is opening
        private void Application_DocumentOpening(object sender, Autodesk.Revit.DB.Events.DocumentOpeningEventArgs args)
        {

        }

        //Do Something when the Document has finished opening
        private void Application_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs args)
        {

        }

        //Do something when the Document is Closing
        private void Application_DocumentClosing(object sender, Autodesk.Revit.DB.Events.DocumentClosingEventArgs args)
        {

        }

        //Do something when the Document has finished Synchronizing
        private void Application_DocumentSynchronized(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizedWithCentralEventArgs args)
        {

        }

        //Create and Register Dynamic Model Updaters with the Current Session of Revit
        private Result RegisterUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(viewSheetUpdater, true);
                ElementClassFilter viewSheetFilter = new ElementClassFilter(typeof(ViewSheet));
                UpdaterRegistry.AddTrigger(viewSheetUpdater.GetUpdaterId(), viewSheetFilter, Element.GetChangeTypeElementAddition());
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Registering Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Unregister Dynamic Model Updaters with the Current Session of Revit
        private Result UnregisterUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                UpdaterRegistry.UnregisterUpdater(viewSheetUpdater.GetUpdaterId());
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Unregistering Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Enable Dynamic Model Updaters with the Current Session of Revit. This is to be used when one or more Updaters were Disabled programatically
        internal static Result EnableUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                UpdaterRegistry.EnableUpdater(viewSheetUpdater.GetUpdaterId());
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Enabling Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Disable Dynamic Model Updaters with the Current Session of Revit. This is usefull when another tool may be stopped or interupted by an Updater
        internal static Result DisableUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                Updaters.ViewSheetUpdater sheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                UpdaterRegistry.DisableUpdater(sheetUpdater.GetUpdaterId());
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Result.Failed;
            }
        }
    }
}
