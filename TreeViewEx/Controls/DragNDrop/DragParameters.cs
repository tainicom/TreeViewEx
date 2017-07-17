
namespace tainicom.TreeViewEx.DragNDrop
{
    public class DragParameters
    {
        public readonly TreeViewExItem DragItem;
        public object DraggedObject;

        public DragParameters(TreeViewExItem dragItem)
        {
            this.DragItem = dragItem;
        }
    }
}
