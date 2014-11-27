using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingWithControllerSample.CommandMgr
{

    abstract internal class CommandBase
    {
        internal IReceiver receiver;

        public CommandBase(IReceiver receiver)
        {
            this.receiver = receiver;
        }

        public abstract void Execute();
        public abstract void Undo();

    }



}
