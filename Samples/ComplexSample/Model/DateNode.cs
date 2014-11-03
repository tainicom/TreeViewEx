using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeViewEx.ComplexSample.Model
{
	class DateNode:NodeBase
	{
		public DateTime Date { get; set; }

		public override string ToString()
		{
			return "DateNode: " + Date.ToShortDateString();
		}

		public override string Tip
		{
			get { return "You can press F2 to edit the date."; }
		}
	}
}
