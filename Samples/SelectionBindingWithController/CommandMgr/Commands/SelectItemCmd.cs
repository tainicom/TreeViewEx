using System;
using System.Collections.Generic;
using System.Text;
using BindingWithControllerSample.Model;
using BindingWithControllerSample.ViewModel;

namespace BindingWithControllerSample.CommandMgr.Commands
{
    class SelectItemCmd:CommandBase
    {
        Node gc;

        public SelectItemCmd(MainViewModel receiver, Node gc): base(receiver)
        {            
            this.gc = gc;
        }

        public override void Execute()
        {
            ((MainViewModel)receiver).__SelectItem(gc);
        }


        public override void Undo()
        {
            ((MainViewModel)receiver).__DeselectItem(gc);
        }

    }
}
