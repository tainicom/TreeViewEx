using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeViewEx.ComplexSample.Model
{
	class SimpleNode : NodeBase
	{
		private string name;
		public SimpleNode(string name)
		{
			this.name = name;
		}


		public override string ToString()
		{
			return "SimpleNode: " + name;
		}

		public override string Tip
		{
			get { return string.Empty; }
		}
	}
}
