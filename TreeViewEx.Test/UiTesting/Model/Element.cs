using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using TreeViewEx.Test.Model.Helper;

namespace TreeViewEx.Test.Model
{
	class Element
	{
		public Element(AutomationElement automationElement)
		{
            if (automationElement == null)
            {
                throw new ArgumentNullException("automationElement");
            }

			Ae = automationElement;
		}

		internal AutomationElement Ae { get; private set; }

		public void Expand()
		{
			ExpandCollapsePattern expandPattern = Ae.GetPattern<ExpandCollapsePattern>();
			expandPattern.Expand();
		}

		public void Collapse()
		{
			ExpandCollapsePattern expandPattern = Ae.GetPattern<ExpandCollapsePattern>();
			expandPattern.Collapse();
		}

		public void Select()
		{
			SelectionItemPattern pattern = Ae.GetPattern<SelectionItemPattern>();
			pattern.Select();
		}

		public bool IsSelected
		{
			get
			{
				SelectionItemPattern pattern = Ae.GetPattern<SelectionItemPattern>();
				return pattern.Current.IsSelected;
			}
		}

		public bool IsExpanded
		{
			get
			{
				ExpandCollapsePattern pattern = Ae.GetPattern<ExpandCollapsePattern>();
				return pattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
			}
		}

		public bool IsFocused
		{
			get
			{
				//throw new InvalidOperationException(
				//   "Focus of item cannot be requested, because the focus is set by UIAutomation.";
				return Convert.ToBoolean(GetValue("node;IsFocused"));
			}
		}

		public string GetValue(string id)
		{
			ValuePattern pattern = Ae.GetPattern<ValuePattern>();
			pattern.SetValue(id);
			return pattern.Current.Value;
		}
	}
}
