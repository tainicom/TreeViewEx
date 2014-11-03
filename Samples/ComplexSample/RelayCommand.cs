using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TreeViewEx.ComplexSample
{
	class RelayCommand : ICommand
	{
		Action action;

		public RelayCommand(Action action)
		{
			this.action = action;
		}

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			action();
		}

		#endregion
	}
}
