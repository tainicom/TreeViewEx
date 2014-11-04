using System;
using System.Collections.Generic;
using System.Text;

namespace BindingWithControllerSample.CommandMgr
{
    internal class CommandController
    {
        private Stack<CommandBase> _commandStack;
        private Stack<CommandBase> _undoStack;

        public CommandController()
        {
            _commandStack = new Stack<CommandBase>();
            _undoStack = new Stack<CommandBase>();
        }

        public void ExecuteCommand()
        {
            if (_commandStack.Count == 0) return;

            //Trace.WriteLine("EXECUTING COMMAND");
            
            CommandBase command = _commandStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }

        public void UndoCommand()
        {
            if (_undoStack.Count == 0) return;

            //Trace.WriteLine("REVERSING COMMAND");

            CommandBase command = _undoStack.Pop();
            command.Undo();
            _commandStack.Push(command);
        }

        public void AddAndExecute(CommandBase command)
        {
            _commandStack.Clear();
            _commandStack.Push(command);
            ExecuteCommand();
        }

        public void ClearHistory()
        {
            _undoStack.Clear();
            _commandStack.Clear();
        }

        public int RedoCount { get { return _commandStack.Count; } }
        public int UndoCount { get { return _undoStack.Count; } }

    }
}
