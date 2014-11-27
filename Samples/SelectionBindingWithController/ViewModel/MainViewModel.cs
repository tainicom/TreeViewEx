using BindingWithControllerSample.CommandMgr;
using BindingWithControllerSample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BindingWithControllerSample.ViewModel
{
    public class MainViewModel:IReceiver
    {
        CommandController _controller;
        
        public ObservableCollection<Node> Children { get; set; }
        internal CommandController Controller { get { return _controller; } }

        public int RedoCount { get { return _controller.RedoCount; } }
        public int UndoCount { get { return _controller.UndoCount; } }


        public MainViewModel()
		{
            _controller = new CommandController();
			Children = new ObservableCollection<Node>();
		}

        ObservableCollection<Node> _selectedItems = new ObservableCollection<Node>();
        public ObservableCollection<Node> SelectedItems
        {
            get { return _selectedItems; }
        }
        ReadOnlyObservableCollection<Node> _readonlySelectedItems = null;
        public ReadOnlyObservableCollection<Node> ReadOnlySelectedItems
        {
            get
            {
                if (_readonlySelectedItems == null)
                    _readonlySelectedItems = new ReadOnlyObservableCollection<Node>(_selectedItems);
                return _readonlySelectedItems;
            }
        }


        #region Reciever Selection 

        internal void __SelectItem(Node gc)
        {
            if (!_selectedItems.Contains(gc))
            {
                _selectedItems.Add(gc);
                //OnSelectionChanged(EventArgs.Empty);
            }
        }

        internal void __DeselectItem(Node gc)
        {
            if (_selectedItems.Contains(gc))
            {
                _selectedItems.Remove(gc);
                //OnSelectionChanged(EventArgs.Empty);
            }
        }

        internal Node[] __DeselectAllItems()
        {
            //make a copy of currert selections and return it.
            Node[] prevSelectedItems = new Node[_selectedItems.Count];
            _selectedItems.CopyTo(prevSelectedItems, 0);

            _selectedItems.Clear();
            //OnSelectionChanged(EventArgs.Empty);

            return prevSelectedItems;
        }

        #endregion



    }
}
