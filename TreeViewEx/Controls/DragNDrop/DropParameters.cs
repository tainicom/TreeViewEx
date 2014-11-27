
namespace System.Windows.Controls.DragNDrop
{
    public class DropParameters
    {
        public readonly TreeViewExItem DropToItem;
        public readonly IDataObject DropData;
        public readonly int Index;

        public DropParameters(TreeViewExItem dropToItem, IDataObject dropData): this(dropToItem, dropData, -1)
        {
        }

        public DropParameters(TreeViewExItem dropToItem, IDataObject dropData, int index)
        {
            this.DropToItem = dropToItem;
            this.DropData = dropData;
            this.Index = index;
        }
                
    }
}
