namespace DragNDropSample.Model
{
    #region

    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Input;
    using tainicom.TreeViewEx.DragNDrop;
    using System.Linq;
    #endregion

    /// <summary>
    /// Model for testing
    /// </summary>
    public class Node
    {
        #region Constructors and Destructors

        public Node()
        {
            Children = new ObservableCollection<Node>();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Node> Children { get; set; }

        public string Name { get; set; }
        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion


        public bool AllowDrag { get; set; }

        public bool AllowDrop { get; set; }

        public bool AllowInsert { get; set; }
        
    }
}