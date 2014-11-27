using System;
using System.Collections.Generic;
using System.Text;
using BindingWithControllerSample.Model;
using BindingWithControllerSample.ViewModel;

namespace BindingWithControllerSample.CommandMgr.Commands
{
    class DeselectAllItemsCmd : CommandBase
    {
        Node[] selectedItems;

        public DeselectAllItemsCmd(MainViewModel receiver): base(receiver)
        {
        }

        public override void Execute()
        {
            selectedItems = ((MainViewModel)receiver).__DeselectAllItems();
        }
        
        public override void Undo()
        {
            foreach (Node gc in selectedItems)
                ((MainViewModel)receiver).__SelectItem(gc);
        }
    }
}
