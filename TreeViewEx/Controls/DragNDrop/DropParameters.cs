
using System.Windows;
namespace tainicom.TreeViewEx.DragNDrop
{
    public class DropParameters
    {
        public readonly TreeViewExItem DropToItem;
        public readonly IDataObject Data;
        public readonly int Index;

        public DropParameters(TreeViewExItem dropToItem, IDataObject data): this(dropToItem, data, -1)
        {
        }

        public DropParameters(TreeViewExItem dropToItem, IDataObject data, int index)
        {
            this.DropToItem = dropToItem;
            this.Data = data;
            this.Index = index;
        }
                
    }
}
