using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using TreeViewEx.ComplexSample.Model;

namespace TreeViewEx.ComplexSample
{
	class TreeItemStyleSelector : StyleSelector
	{
		public override Style SelectStyle(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (item != null)
			{
				if (item is SimpleNode)
				{
					return element.FindResource("simpleNodeStyle") as Style;
				}

				if (item is DateNode)
				{
					return element.FindResource("dateNodeStyle") as Style;
				}

				if (item is NumberNode)
				{
					return element.FindResource("numberNodeStyle") as Style;
				}
			}

			return base.SelectStyle(item, container);
		}
	}
}
