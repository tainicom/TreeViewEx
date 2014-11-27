using System;
using System.Collections.Generic;
using System.Text;

namespace BindingWithControllerSample.CommandMgr.Commands
{
    /// <summary>
    /// Execute/Undo more than one command in a single step.
    /// Useful for compiling simple commands into more complex one.
    /// </summary>
    class CommandGroupCmd : CommandBase
    {
        CommandBase[] commandGroup;

        public CommandGroupCmd(IReceiver receiver, CommandBase[] commandGroup): base(receiver)
        {
            this.commandGroup = commandGroup;
        }

        public override void Execute()
        {
            // execute all commands in a row
            for(int i=0;i<commandGroup.Length;i++)
                commandGroup[i].Execute();
        }
        
        public override void Undo()
        {
            // undo all commands in reverse order
            for (int i = (commandGroup.Length-1); i >=0; i--)
                commandGroup[i].Undo();
        }



    }
}
