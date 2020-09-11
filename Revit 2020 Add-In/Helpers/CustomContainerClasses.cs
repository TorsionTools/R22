using Autodesk.Revit.DB;
using System.ComponentModel;

namespace TorsionTools
{
    //This class is used to Update the listview in real time when checking boxes which is really
    //change the value of the Check variable and representing it as a CheckBox
    internal class ElementIdName : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _Check;
        private string _Name;
        private ElementId _ElementId;

        public bool Check
        {
            get { return _Check; }
            set
            {
                _Check = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        public ElementId ElemId
        {
            get { return _ElementId; }
            set
            {
                _ElementId = value;
                NotifyPropertyChanged();
            }
        }

        //Makes sure the variable is not null, then fires a change event to update list view graphics
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //This class is used to Update the listview in real time when checking boxes which is really
    //change the value of the Check variable and representing it as a CheckBox
    internal class ViewSheetsIdName : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _Check;
        private string _SheetName;
        private ElementId _SheetId;

        public bool Check
        {
            get { return _Check; }
            set
            {
                _Check = value;
                NotifyPropertyChanged();
            }
        }

        public string SheetName
        {
            get { return _SheetName; }
            set
            {
                _SheetName = value;
                NotifyPropertyChanged();
            }
        }

        public ElementId SheetId
        {
            get { return _SheetId; }
            set
            {
                _SheetId = value;
                NotifyPropertyChanged();
            }
        }

        //Makes sure the variable is not null, then fires a change event to update list view graphics
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    //This class is used to Update the listview in real time when checking boxes which is really
    //change the value of the Check variable and representing it as a CheckBox
    internal class ViewSheetsIdNumberName : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ElementId _SheetId;
        private string _SheetNumber;
        private string _SheetName;
        private bool _Check;

        public ElementId SheetId
        {
            get { return _SheetId; }
            set
            {
                _SheetId = value;
                NotifyPropertyChanged();
            }
        }
        
        public string SheetNumber
        {
            get { return _SheetNumber; }
            set
            {
                _SheetNumber = value;
                NotifyPropertyChanged();
            }
        }

        public string SheetName
        {
            get { return _SheetName; }
            set
            {
                _SheetName = value;
                NotifyPropertyChanged();
            }
        }

        public bool Check
        {
            get { return _Check; }
            set
            {
                _Check = value;
                NotifyPropertyChanged();
            }
        }

        //Makes sure the variable is not null, then fires a change event to update list view graphics
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
