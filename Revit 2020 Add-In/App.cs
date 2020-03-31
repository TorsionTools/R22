using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Windows.Forms;

namespace Revit_2020_Add_In
{
    class App : IExternalApplication
    {
        Document doc;
        UIDocument uiDoc;
        bool idleCheck;

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

            //Command Bindings allow you to interecept Revit Commands like Import CAD or In Place components to block or warn people not to use them
            //Add a Command Binding for CAD Import
            AddCommandBindings(RevitApplication, "ID_FILE_IMPORT");

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

        //This Idling method is used to add the binding back to the command if the user proceeds with disabling it
        private void FileImport_Idling(object sender, IdlingEventArgs args)
        {
            //Here we will the bool switch to make sure the first execution does not reset the binding
            //but will run on the second after we have set the bool to false
            if(!idleCheck)
            {
                //Reset the command binding so that the next time the button is pressed it will do the same thing
                //The "Name" could be a variable so that this method is more flexible
                AddCommandBindings(Helpers.StaticVariables.revitApplication, "ID_FILE_IMPORT");
                //We want to remove this Idling event so we use the "-=" to stop it from executing continously
                Helpers.StaticVariables.revitApplication.Idling -= FileImport_Idling;
                //The return here is just to make sure we escape the method after adding the binding back.
                //In this simple case it is not needed, but more complex idlers it could be necessary.
                return;
            }
            //Once the Idling event has fired once, the bool switch will be set to false
            idleCheck = false;
        }

        //The following Method will bind the Revit Command to the indicated Method. In this case it is the DisableCommand Method
        private Result AddCommandBindings(UIControlledApplication uiApp, string name)
        {
            //Get the RevitCommandId from the Name of the Command passed on Start Up
            RevitCommandId rCommandId = RevitCommandId.LookupCommandId(name);
            //Make sure the Command can have a binding, and that another application hasn't already bound to it.
            //Command can only have One binding for all add-ins
            if (rCommandId.CanHaveBinding && !rCommandId.HasBinding)
            {
                //Set the Method that the bound command will execute when a user "presses" the button on the Ribbon
                uiApp.CreateAddInCommandBinding(rCommandId).Executed += new EventHandler<ExecutedEventArgs>(this.DisableCommand);
            }
            else
            {
                //Tell the User that the Command cannot be bound
                TaskDialog.Show(name+" Binding","The "+name+" Command can not have Binding or is already Bound");
            }

            return Result.Succeeded;
        }

        //This method just asks the user to verify this is what they want to do. It then attaches to an Idling event to add the binding back
        private void DisableCommand(object sender, ExecutedEventArgs args)
        {
            if (TaskDialog.Show("Contine?","The use of this Command is not recommended.\nDo you want to proceed anyway?",TaskDialogCommonButtons.Yes|TaskDialogCommonButtons.No,TaskDialogResult.No)== TaskDialogResult.Yes)
            {
                //Get the active Document
                doc = args.ActiveDocument;
                //Get the UIDocument based on the current Doc and set the Class Variable
                uiDoc = new UIDocument(doc);
                //Remove the binding on the command so it can be called without prompting the user again
                uiDoc.Application.RemoveAddInCommandBinding(args.CommandId);
                //Set up an Idling event to add the command binding back once the command has been called 
                Helpers.StaticVariables.revitApplication.Idling += new EventHandler<IdlingEventArgs>(FileImport_Idling);
                //The Idling event will execute one time between the completion of this Metho and the calling of the post command
                //so we use the following bool as a switch to catch that and make sure the command in not boudn BEFORE we post the command
                idleCheck = true;
                //Continue the command by programmatically calling the same command the user did
                uiDoc.Application.PostCommand(args.CommandId);
            }
        }

        //reate and Register Dynamic Model Updaters with the Current Session of Revit. It should only need to be registered one time per session, and affects
        //all documents opened there after
        private Result RegisterUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                //Create a new instance of the ViewSheetUpdater Class and Pass the ActiveAddInId for this Application
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                //Register the Updater for this Session of Revit. The IsOption bool at the end allows the modifications of the updater to persist even if another user
                //doesnt have the same add-in (For paid / propriatary add-in like Autodesk Subcription itsm)
                UpdaterRegistry.RegisterUpdater(viewSheetUpdater, true);
                //Filter the items being "watched" to just ViewSheets so we use a class filter
                ElementClassFilter viewSheetFilter = new ElementClassFilter(typeof(ViewSheet));
                //Add a trigger to the Updater so Revit knows When to execute the Updater. Here it is set for Element Addition
                //which is when a new sheet is created.
                UpdaterRegistry.AddTrigger(viewSheetUpdater.GetUpdaterId(), viewSheetFilter, Element.GetChangeTypeElementAddition());
                //This Method is set up as a Result becuase you may need to Unregister and Re-Register the updaters for other portions of the App and
                //it is good to know if it all worked or not
                return Result.Succeeded;
            }

            //Catch any exceptions and present them to the User
            catch (Exception ex)
            {
                TaskDialog.Show("Error Registering Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Unregister Dynamic Model Updaters with the Current Session of Revit to clean things up on Exit
        private Result UnregisterUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                //Create a new instance of the ViewSheetUpdater Class and Pass the ActiveAddInId for this Application
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                //Unregister the Updater from the Updater Registry using the UpdaterId
                UpdaterRegistry.UnregisterUpdater(viewSheetUpdater.GetUpdaterId());
                //Return the result
                return Result.Succeeded;
            }
            //Catch any exceptions and present them to the User
            catch (Exception ex)
            {
                TaskDialog.Show("Error Unregistering Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Enable Dynamic Model Updaters within the Current Session of Revit. This is to be used when one or more Updaters were Disabled programatically
        internal static Result EnableUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                //Create a new instance of the ViewSheetUpdater Class and Pass the ActiveAddInId for this Application
                Updaters.ViewSheetUpdater viewSheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                //Enable the updater once it is registered, or if it has been disabled by another Method or Class programatically
                UpdaterRegistry.EnableUpdater(viewSheetUpdater.GetUpdaterId());
                //Return the result
                return Result.Succeeded;
            }
            //Catch any exceptions and present them to the User
            catch (Exception ex)
            {
                TaskDialog.Show("Error Enabling Updaters", ex.ToString());
                return Result.Failed;
            }
        }

        //Disable Dynamic Model Updaters within the Current Session of Revit. This is useful when another tool may be stopped or interupted by an Updater
        internal static Result DisableUpdaters(UIControlledApplication RevitApplication)
        {
            try
            {
                //Create a new instance of the ViewSheetUpdater Class and Pass the ActiveAddInId for this Application
                Updaters.ViewSheetUpdater sheetUpdater = new Updaters.ViewSheetUpdater(RevitApplication.ActiveAddInId);
                //Disable the updater. It will not execute again until it is Enabled
                UpdaterRegistry.DisableUpdater(sheetUpdater.GetUpdaterId());
                //Return the Result
                return Result.Succeeded;
            }
            //Catch any exceptions and present them to the User
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Result.Failed;
            }
        }
    }
}
