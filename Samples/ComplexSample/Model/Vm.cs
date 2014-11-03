using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TreeViewEx.ComplexSample.Model
{
	class Vm
	{
		public Vm()
		{
			var first1 = new SimpleNode ("element1");
			var first2 = new SimpleNode ("element2");
			var first11 = new DateNode { Date = DateTime.Now };
			var first12 = new NumberNode { Number = 728 };
			var first13 = new SimpleNode ("element13");
			var first14 = new SimpleNode ("element14");
			var first15 = new SimpleNode ("element15 (Not selectable)");
			var first131 = new SimpleNode ("element131");
			var first132 = new SimpleNode ("element132");

			// add to tree
			NodeTree = new List<NodeBase>();
			NodeTree.Add(first1);
			NodeTree.Add(first2);
			first1.Children.Add(first11);
			first1.Children.Add(first12);
			first1.Children.Add(first13);
			first1.Children.Add(first14);
			first1.Children.Add(first15);
			first13.Children.Add(first131);
			first13.Children.Add(first132);

			// add to list
			NodeList = new List<NodeBase>();
			NodeList.Add(first1);
			NodeList.Add(first11);
			NodeList.Add(first12);
			NodeList.Add(first13);
			NodeList.Add(first131);
			NodeList.Add(first132);
			NodeList.Add(first14);
			NodeList.Add(first15);
			NodeList.Add(first2);
		}

		public List<NodeBase> NodeTree { get; set; }
		public List<NodeBase> NodeList { get; set; }
	}
}
