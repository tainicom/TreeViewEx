using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeViewEx.Test.Model.Helper;
using System.Windows.Automation;

namespace TreeViewEx.Test.Model
{
    class Tree
    {
        AutomationElement treeAutomationElement;
        Element element1;
        Element element11;
        Element element12;
        Element element13;
        Element element14;
        Element element15;
        Element element2;

        public Tree(AutomationElement treeAutomationElement)
        {
            this.treeAutomationElement = treeAutomationElement;
        }

        public Element Element1 { get { return new Element(treeAutomationElement.FindFirstDescendant(ControlType.TreeItem)); } }
        public Element Element11 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element11")); } }
        public Element Element12 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element12")); } }
        public Element Element13 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element13")); } }
        public Element Element14 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element14")); } }
        public Element Element15 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element15")); } }
        public Element Element2 { get { return new Element(treeAutomationElement.FindDescendantByName(ControlType.TreeItem, "element2")); } }
    }
}
