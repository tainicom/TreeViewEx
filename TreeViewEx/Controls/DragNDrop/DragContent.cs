using System.Collections.Generic;
using System.ComponentModel;

namespace tainicom.TreeViewEx.DragNDrop
{
    public class DragContent : INotifyPropertyChanged
    {
        private bool canDrop;
        private bool canInsert;
        private int insertIndex;
        private List<object> draggedItems;

        internal DragContent()
        {
            draggedItems = new List<object>();
        }

        public void Add(object draggedItem)
        {
            draggedItems.Add(draggedItem);
            RaisePropertyChanged("Count");
        }

        public bool CanInsert
        {
            get { return canInsert; }
            set
            {
                if (canInsert != value)
                {
                    canInsert = value;
                    RaisePropertyChanged("CanInsert");
                }
            }
        }
        
        public IEnumerable<object> Items { get { return draggedItems; } }

        public int Count { get { return draggedItems.Count; } }

        public bool CanDrop
        {
            get { return canDrop; }
            set
            {
                if (canDrop != value)
                {
                    canDrop = value;
                    RaisePropertyChanged("CanDrop");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
                
    }
}
