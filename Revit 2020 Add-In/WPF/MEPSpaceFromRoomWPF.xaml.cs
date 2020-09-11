using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TorsionTools.WPF
{
    /// <summary>
    /// Interaction logic for MEPSpaceFromRoomWPF.xaml
    /// </summary>
    public partial class MEPSpaceFromRoomWPF : Window
    {
        //Class level varaible to be used for multiple methods
        Document doc;
        Document linkDoc;
        List<ElementIdName> LinkedRooms = new List<ElementIdName>();
        Dictionary<string, RevitLinkInstance> linkInstances = new Dictionary<string, RevitLinkInstance>();

        public MEPSpaceFromRoomWPF(Document _doc)
        {
            InitializeComponent();
            //Set the class level Document variable from the Document passed to the form
            doc = _doc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Create a DataTable to hold the Revit Link information
            DataTable dtLinkedDocuments = new DataTable();

            dtLinkedDocuments.Columns.Add(new DataColumn("Name", typeof(string)));
            dtLinkedDocuments.Columns.Add(new DataColumn("Document", typeof(Document)));
            //Add a row inthe DataTable for when no Link is Selected and give it a null value

            //Get all of the Link Instances in the Current Document
            using (FilteredElementCollector LinkInstancesCollector = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)))
            {
                //Check to make sure there is at least one Link Instance
                if (LinkInstancesCollector.ToElements().Count > 0)
                {
                    //Get each Revit Link Instance since here may be more than one
                    foreach (RevitLinkInstance LinkedInstance in LinkInstancesCollector.ToElements())
                    {
                        //Get the Revit Link Type from the instance to check and see if it is loaded, which it should be if there is an instance
                        RevitLinkType linkType = doc.GetElement(LinkedInstance.GetTypeId()) as RevitLinkType;
                        
                        //We only want to check Links that are currently Loaded
                        if (linkType.GetLinkedFileStatus() == LinkedFileStatus.Loaded && !linkType.IsNestedLink)
                        {
                            //Iterate through the Link Instances in the Document and match the first one with the Link Type. This is because you need the Link Instance to get the Link Document, not the Link Type
                            //RevitLinkInstance LinkedDocumentInstance = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).Where(x => x.GetTypeId() == LinkedInstance.Id).First() as RevitLinkInstance;
                            //Add the Link Name and Link Instance to the Dictionary if it doesn't already exist
                            if (!linkInstances.ContainsKey(linkType.Name + " - " + LinkedInstance.Name))
                            {
                                linkInstances.Add(linkType.Name + " - " + LinkedInstance.Name, LinkedInstance);
                            }
                            else
                            {
                                //Tell the User that the Link Type Name and Instance Name combination already exist
                                TaskDialog.Show("Duplicate Link Instance Name", "More than one Link Instance has the same name. Only one instance has been added.");
                                //Continue through the for loop
                                continue;
                            }
                        }
                    }
                }
                //Tell the user when there aren't any linked document and close the form
                else
                {
                    TaskDialog.Show("No Revit Link", "Document does not contain any Revit Links.");
                    DialogResult = false;
                    Close();
                }
            }

            //Use the Dictionary as the items source of the Combobox and set the Display and value Path
            ComboBoxLinks.ItemsSource = linkInstances.ToList();
            ComboBoxLinks.SelectedValuePath = "Value";
            ComboBoxLinks.DisplayMemberPath = "Key";
        }

        private void ComboBoxRevitLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check to make sure the Selected value is not null, which is the Value for the row we added
            if (ComboBoxLinks.SelectedValue != null)
            {
                //Get the Link Document by casting the Selected Value of the Combo Box to a Document
                RevitLinkInstance rvtLinkInstance = (RevitLinkInstance)ComboBoxLinks.SelectedValue;
                //Get the Revit Link Type to check Type Parameter
                RevitLinkType rvtLinkType = doc.GetElement(rvtLinkInstance.GetTypeId()) as RevitLinkType;
                //Check to see if the Link Type is Room Bounding
                if (rvtLinkType.LookupParameter("Room Bounding").AsInteger() == 0)
                {
                    //If it is not room bounding, ask the user if they want to proceed or not
                    if (TaskDialog.Show("Linked Model Room Bounding", "The Revit Link " + rvtLinkType.Name + " is not Room Bounding.\n\nWould you like to make it Room Bounding and proceed with creating Spaces?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogResult.No) == TaskDialogResult.Yes)
                    {
                        //Use a transaction and set the Link Type parameter to be room bounding
                        using (Transaction trans = new Transaction(doc))
                        {
                            trans.Start("Change Link Room Bounding");
                            rvtLinkType.LookupParameter("Room Bounding").Set(1);
                            trans.Commit();
                        }
                        //If you user says yes, get all rooms in the linked model via GetLinkedRoom method by passing the Linked Document
                        GetLinkedRooms(rvtLinkInstance.GetLinkDocument());
                    }
                    else
                    {
                        //If the user says no, close the form
                        DialogResult = false;
                        Close();
                    }
                }
                else
                {
                    //If the link is already room bounding, get all rooms in the linked model via GetLinkedRoom method by passing the Linked Document
                    GetLinkedRooms(rvtLinkInstance.GetLinkDocument());
                }
            }
        }

        private void GetLinkedRooms(Document _linkDoc)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                linkDoc = _linkDoc;
                //Clear the Items in the List View so they aren't added to existing ones when the Link is changed
                ListViewLinkedRooms.Items.Clear();

                //Use a Filtered Element Collector to get all of the Rooms in the Linked Document
                using (FilteredElementCollector LinkRooms = new FilteredElementCollector(linkDoc).OfCategory(BuiltInCategory.OST_Rooms))
                {
                    //Loop through each Room in the Element Collector
                    foreach (Room LinkRoom in LinkRooms.ToElements())
                    {
                        //Check to make sure the room is placed and not just in the model
                        if (LinkRoom.Location != null && LinkRoom.Area > 0)
                        {
                            {
                                //Create a new instance of the ElementIdName class to store the linked Room information
                                LinkedRooms.Add(new ElementIdName() { Name = LinkRoom.Name, ElemId = LinkRoom.Id, Check = false });
                            }
                        }
                    }
                }
            }
            //Display a message that an error occured when trying to change the Link Selection
            catch (Exception ex)
            {
                TaskDialog.Show("Selection Change Error", ex.ToString());
            }
            //Set the items source of the ListView to the DataTable
            ListViewLinkedRooms.ItemsSource = LinkedRooms;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewLinkedRooms.SelectedItems.Cast<ElementIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ElementIdName Room in TempViews)
            {
                //Try and find the item in the main Item list and get that object
                //We can't directly change this List because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ElementIdName item = LinkedRooms.Find(x => x.ElemId == Room.ElemId);
                //set the bool Check variable to true or checked
                item.Check = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Cast the selected items in the ListView to a List of the ElementIdName class
            List<ElementIdName> TempViews = ListViewLinkedRooms.SelectedItems.Cast<ElementIdName>().ToList();
            //iterate through each of the ViewSheets items in that list
            foreach (ElementIdName Room in TempViews)
            {
                //Try and find the item in the main item list and get that object
                //We can't directly change this List because we have linked it to the Listview
                //and we are using NotifyPropertyChanged events so that is why we need temp items
                ElementIdName item = LinkedRooms.Find(x => x.ElemId == Room.ElemId);
                //set the bool Check variable to true or checked
                item.Check = false;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnCreateSpaces_Click(object sender, RoutedEventArgs e)
        {
            //Use a Try block to keep any errors from crashing Revit
            try
            {
                //Integer variable to count the number of Spaces Created
                int count = 0;

                //Create a new Dictionary to hold Levels and Level Names, then populate with Element Collector
                Dictionary<string, Level> levels = new Dictionary<string, Level>();
                using (FilteredElementCollector fecLevels = new FilteredElementCollector(doc).OfClass(typeof(Level)))
                {
                    foreach (Level level in fecLevels.ToElements())
                    {
                        levels.Add(level.Name, level);
                    }
                }

                //Use a bool switch if there are levels not found
                bool missingLevels = false;
                int missingCount = 0;

                //Get the Link Document by casting the Selected Value of the Combo Box to a Document
                RevitLinkInstance rvtLinkInstance = (RevitLinkInstance)ComboBoxLinks.SelectedValue;
                //Get the Transform for the Link Instance to position the spaces relative to any Transformation of the link instance
                Transform LinkTransform = rvtLinkInstance.GetTotalTransform();

                //Use a Transaction for the Document you are creating spaces in
                using (Transaction trans = new Transaction(doc))
                {
                    //Start the transaction and give it a name for the Undo / Redo list
                    trans.Start("Create Spaces from Rooms");
                    //Loop through each of the Checked Items (rooms) in the List view to copy
                    foreach (ElementIdName Room in LinkedRooms)
                    {
                        if (Room.Check)
                        {
                            //Cast the Room from the ListView and Linked Document
                            Room LinkedRoom = (Room)linkDoc.GetElement(Room.ElemId);
                            //Get the location of the Linked Room to place the Space
                            LocationPoint roomLocationPoint = LinkedRoom.Location as LocationPoint;
                            //Try and get the level from the current document with the same name as the Linked Document
                            if (levels.TryGetValue(LinkedRoom.Level.Name, out Level level))
                            {
                                //If a level with the same name is found, create the new Space from that level and transformed room location
                                Space space = doc.Create.NewSpace(level, new UV(LinkTransform.OfPoint(roomLocationPoint.Point).X, LinkTransform.OfPoint(roomLocationPoint.Point).Y));
                                //Set the Name and Number of the space to match the room it cam from.
                                space.Name = LinkedRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                                space.Number = LinkedRoom.Number;
                                count++;
                            }
                            else
                            {
                                //If the level can't be found, change the bool switch
                                missingLevels = true;
                                missingCount++;
                            }
                        }
                    }
                    //Commit the transaction to save the changed to the document
                    trans.Commit();
                }
                //Tell the user how many spaces were created or missed
                if (!missingLevels)
                {
                    TaskDialog.Show("Create Spaces", "Created " + count + " Spaces");
                }
                else
                {
                    TaskDialog.Show("Create Spaces", "Created " + count + " Spaces\n\nCorresponding Level not found for " + missingCount + " Spaces");
                }

                //Set the Dialog Result so that the calling Method resturns the correct result and the changes stick
                DialogResult = true;
                //Close the form
                Close();
            }
            //Display a message that an error occured when trying to create Spaces, set the Dialog Result, and close the form.
            catch (Exception ex)
            {
                TaskDialog.Show("Create Spaces Error", ex.ToString());
                DialogResult = false;
                Close();
            }
        }

        private void ChkSelectAllRooms_Checked(object sender, RoutedEventArgs e)
        {
            //Cast the sending Checkbox to check the Check status
            CheckBox chkbox = sender as CheckBox;
            //Check to see if it is Checked
            if (chkbox.IsChecked == true)
            {
                //If it is checked, iterate each instance of the ElementIdName class and set the Check property to true
                foreach (ElementIdName eid in LinkedRooms)
                {
                    eid.Check = true;
                }
            }
            else
            {
                //If it is not checked, iterate each instance of the ElementIdName class and set the Check property to false
                foreach (ElementIdName eid in LinkedRooms)
                {
                    eid.Check = false;
                }
            }
        }
    }
}
