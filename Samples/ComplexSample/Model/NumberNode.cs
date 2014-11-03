using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

namespace TreeViewEx.ComplexSample.Model
{
	class NumberNode:NodeBase, INotifyPropertyChanged
	{
		public int Number { get; set; }
		
		public ICommand CountUp { get { return new RelayCommand(CountUpAction); } }

		public ICommand CountDown { get { return new RelayCommand(CountDownAction); } }

		public override string Tip
		{
			get { return "You can press F2 to edit the number or right click to open the context menu."; }
		}

		private void CountDownAction()
		{
			Number--;
			RaisePropertyChanged("Number");
		}

		private void CountUpAction()
		{
			Number++;
			RaisePropertyChanged("Number");
		}

		public override string ToString()
		{
			return "NumberNode: " + Number;
		}

		private void RaisePropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
