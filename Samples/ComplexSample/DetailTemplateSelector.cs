using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TreeViewEx.ComplexSample.Model;
using System.Windows;

namespace TreeViewEx.ComplexSample
{
	class DetailTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (item != null && item is NodeBase)
			{
				return element.FindResource("detailTemplate") as DataTemplate;
			}

			return element.FindResource("noNodeTemplate") as DataTemplate;
		}
	}
}
