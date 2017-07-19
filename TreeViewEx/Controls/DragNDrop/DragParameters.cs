using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace tainicom.TreeViewEx.DragNDrop
{
    public class CanDragParameters
    {
        public readonly IEnumerable<TreeViewExItem> Items;
        public readonly Point Position;
        public readonly MouseButton Button;

        internal CanDragParameters(IEnumerable<TreeViewExItem> draggableItems, Point dragPosition, MouseButton mouseButton)
        {
            this.Items = draggableItems;
            this.Position = dragPosition;
            this.Button = mouseButton;
        }
    }

    public class DragParameters
    {
        public readonly IDataObject Data = new DataObject();
        public readonly IEnumerable<TreeViewExItem> Items;
        public readonly Point Position;
        public readonly MouseButton Button;

        public DragDropEffects AllowedEffects = DragDropEffects.All;

        internal DragParameters(IEnumerable<TreeViewExItem> draggableItems, Point dragPosition, MouseButton mouseButton)
        {
            this.Items = draggableItems;
            this.Position = dragPosition;
            this.Button = mouseButton;
        }
    }
}
