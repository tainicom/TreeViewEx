using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BindingWithControllerSample.Model;
using BindingWithControllerSample.ViewModel;

namespace BindingWithControllerSample.CommandMgr.Commands
{
    class DeselectItemCmd : CommandBase
    {
        Node gc;

        public DeselectItemCmd(MainViewModel receiver, Node gc): base(receiver)
        {            
            this.gc = gc;
        }

        public override void Execute()
        {
            ((MainViewModel)receiver).__DeselectItem(gc);
        }


        public override void Undo()
        {
            ((MainViewModel)receiver).__SelectItem(gc);
        }
    }
}
