namespace tainicom.TreeViewEx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Logic for the multiple selection
    /// </summary>
    internal class SelectionMultiple : InputSubscriberBase, ISelectionStrategy
    {
        #region Private fields and constructor
        private readonly TreeViewEx treeViewEx;

        private object lastShiftRoot;

        public SelectionMultiple(TreeViewEx treeViewEx)
        {
            this.treeViewEx = treeViewEx;

            BorderSelectionLogic = new BorderSelectionLogic(treeViewEx, TreeViewElementFinder.FindAll(treeViewEx, false));
        }
        #endregion

        #region Properties

        internal BorderSelectionLogic BorderSelectionLogic { get; private set; }

        internal static bool IsControlKeyDown
        {
            get
            {
                return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            }
        }

        private static bool IsShiftKeyDown
        {
            get
            {
                return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            }
        }

        #endregion

        #region Private helper methods

        private TreeViewExItem GetFocusedItem()
        {
            foreach (var item in TreeViewElementFinder.FindAll(treeViewEx, false))
            {
                if (item.IsFocused) return item;
            }

            return null;
        }
        #endregion

        #region Private modify selection methods

        private void ToggleItem(TreeViewExItem item)
        {
            if (treeViewEx.SelectedItems.Contains(item.DataContext))
            {
                ModifySelection(null, item.DataContext);
            }
            else
            {
                ModifySelection(item.DataContext, null);
            }
        }

        private bool ModifySelection(object itemToSelect, List<object> itemsToUnselect)
        {
            List<object> itemsToSelect = new List<object>();
            itemsToSelect.Add(itemToSelect);

            if (itemsToUnselect == null) itemsToUnselect = new List<object>();

            return ModifySelection(itemsToSelect, itemsToUnselect);
        }

        private bool ModifySelection(List<object> itemsToSelect, object itemToUnselect)
        {
            if (itemsToSelect == null) itemsToSelect = new List<object>();

            List<object> itemsToUnselect = new List<object>();
            itemsToUnselect.Add(itemToUnselect);

            return ModifySelection(itemsToSelect, itemsToUnselect);
        }

        private bool ModifySelection(List<object> itemsToSelect, List<object> itemsToUnselect)
        {
            //clean up any duplicate or unnecessery input
            OptimizeModifySelection(itemsToSelect, itemsToUnselect);

            //check if there's anything to do.
            if (itemsToSelect.Count == 0 && itemsToUnselect.Count == 0)
            {
                return false;
            }

            // notify listeners what is about to change.
            // Let them cancel and/or handle the selection list themself
            bool allowed = treeViewEx.CheckSelectionAllowed(itemsToSelect, itemsToUnselect);
            if (!allowed) return false;

            // Unselect and then select items
            foreach (object itemToUnSelect in itemsToUnselect)
            {
                treeViewEx.SelectedItems.Remove(itemToUnSelect);
            }

            foreach (object itemToSelect in itemsToSelect)
            {
                treeViewEx.SelectedItems.Add(itemToSelect);
            }

            object lastSelectedItem = itemsToSelect.LastOrDefault();

            if (itemsToUnselect.Contains(lastShiftRoot)) lastShiftRoot = null;
            if (!(TreeView.SelectedItems.Contains(lastShiftRoot) && IsShiftKeyDown)) lastShiftRoot = lastSelectedItem;

            return true;
        }

        private void OptimizeModifySelection(List<object> itemsToSelect, List<object> itemsToUnselect)
        {
            // check for items in both lists and remove them in unselect list
            List<object> biggerList;
            List<object> smallerList;
            if (itemsToSelect.Count > itemsToUnselect.Count)
            {
                biggerList = itemsToSelect;
                smallerList = itemsToUnselect;
            }
            else
            {
                smallerList = itemsToUnselect;
                biggerList = itemsToSelect;
            }

            List<object> temporaryList = new List<object>();
            foreach (object item in biggerList)
            {
                if (smallerList.Contains(item))
                {
                    temporaryList.Add(item);
                }
            }

            foreach (var item in temporaryList)
            {
                itemsToUnselect.Remove(item);
            }

            // check for itemsToSelect allready in treeViewEx.SelectedItems
            temporaryList.Clear();
            foreach (object item in itemsToSelect)
            {
                if (treeViewEx.SelectedItems.Contains(item))
                {
                    temporaryList.Add(item);
                }
            }

            foreach (var item in temporaryList)
            {
                itemsToSelect.Remove(item);
            }

            // check for itemsToUnSelect not in treeViewEx.SelectedItems
            temporaryList.Clear();
            foreach (object item in itemsToUnselect)
            {
                if (!treeViewEx.SelectedItems.Contains(item))
                {
                    temporaryList.Add(item);
                }
            }

            foreach (var item in temporaryList)
            {
                itemsToUnselect.Remove(item);
            }
        }

        private void SelectSingleItem(TreeViewExItem item)
        {

            // selection with SHIFT is not working in virtualized mode. Thats because the Items are not visible.
            // Therefor the children cannot be found/selected.
            if (IsShiftKeyDown && treeViewEx.SelectedItems.Count > 0 && !treeViewEx.IsVirtualizing)
            {
                SelectWithShift(item);
            }
            else if (IsControlKeyDown)
            {
                ToggleItem(item);
            }
            else
            {
                List<object> itemsToUnSelect = treeViewEx.SelectedItems.Cast<object>().ToList();
                if (itemsToUnSelect.Contains(item.DataContext))
                {
                    itemsToUnSelect.Remove(item.DataContext);
                }
                ModifySelection(item.DataContext, itemsToUnSelect);
            }

        }
        #endregion

        #region Overrides InputSubscriberBase

        private TreeViewExItem selectItem = null;
        private Point selectPosition;
        private MouseButton selectButton;

        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            TreeViewExItem item = GetTreeViewItemUnderMouse(e.GetPosition(treeViewEx));
            if (item == null) return;
            if (e.ChangedButton != MouseButton.Left && !(e.ChangedButton == MouseButton.Right && item.ContextMenu != null)) return;            
            if (item.IsEditing) return;

            // ToggleItem or SelectWithShift
            if (IsControlKeyDown || (IsShiftKeyDown))
            {
                SelectSingleItem(item);
                FocusHelper.Focus(item);
                return;
            }

            // begin click
            if (selectItem == null)
            {
                selectItem = item;
                selectPosition = e.GetPosition(TreeView);
                selectButton = e.ChangedButton;
            }
        }

        internal override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (selectItem != null)
            {
                // detect drag
                var dragDiff = selectPosition - e.GetPosition(TreeView);
                if ((Math.Abs(dragDiff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(dragDiff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // abort click
                    selectItem = null;
                }
            }
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // click
            if (selectItem != null && e.ChangedButton == selectButton)
            {
                // select item
                SelectSingleItem(selectItem);
                FocusHelper.Focus(selectItem);

                // end click
                selectItem = null;
            }
        }

        private void SelectWithShift(TreeViewExItem item)
        {
            object firstSelectedItem;
            if (lastShiftRoot != null)
            {
                firstSelectedItem = lastShiftRoot;
            }
            else
            {
                // Get the first item in the SelectedItems that is also bound to the Tree.
                firstSelectedItem = treeViewEx.SelectedItems.Cast<object>().FirstOrDefault((x) => { return treeViewEx.GetTreeViewItemFor(x) != null; });
                }

            if (firstSelectedItem != null)
                {
            TreeViewExItem shiftRootItem = treeViewEx.GetTreeViewItemsFor(new List<object> { firstSelectedItem }).First();

            List<object> itemsToSelect = treeViewEx.GetNodesToSelectBetween(shiftRootItem, item).Select(x => x.DataContext).ToList();
            List<object> itemsToUnSelect = treeViewEx.SelectedItems.Cast<object>().ToList();

            ModifySelection(itemsToSelect, itemsToUnSelect);
            }
            else
            {   // Fall-back to sigle selection
                List<object> itemsToUnSelect = treeViewEx.SelectedItems.Cast<object>().ToList();
                if (itemsToUnSelect.Contains(item.DataContext))
                    itemsToUnSelect.Remove(item.DataContext);
                ModifySelection(item.DataContext, itemsToUnSelect);
            }
        }
        #endregion

        #region Methods called by BorderSelection

        internal void UnSelectByRectangle(TreeViewExItem item)
        {
            if (!treeViewEx.CheckSelectionAllowed(item.DataContext, false)) return;

            treeViewEx.SelectedItems.Remove(item.DataContext);
            if (item.DataContext == lastShiftRoot)
            {
                lastShiftRoot = null;
            }
        }

        internal void SelectByRectangle(List<object> itemsToSelect, List<object> itemsToUnselect)
        {
            if (itemsToSelect == null) itemsToSelect = new List<object>();
            if (itemsToUnselect == null) itemsToUnselect = new List<object>();

            ModifySelection(itemsToSelect, itemsToUnselect);
        }

        #endregion

        #region ISelectionStrategy Members

        public void SelectFromUiAutomation(TreeViewExItem item)
        {
            SelectSingleItem(item);

            FocusHelper.Focus(item);
        }

        public void SelectFromProperty(TreeViewExItem item, bool isSelected)
        {
            // we do not check if selection is allowed, because selecting on that way is no user action.
            // Hopefully the programmer knows what he does...
            if (isSelected)
            {
                treeViewEx.SelectedItems.Add(item.DataContext);
                lastShiftRoot = item.DataContext;
                FocusHelper.Focus(item);
            }
            else
            {
                treeViewEx.SelectedItems.Remove(item.DataContext);
            }
        }

        public void SelectFirst()
        {
            TreeViewExItem item = TreeViewElementFinder.FindFirst(treeViewEx, true);
            if (item != null)
            {
                SelectSingleItem(item);
            }

            FocusHelper.Focus(item);
        }

        public void SelectLast()
        {
            TreeViewExItem item = TreeViewElementFinder.FindLast(treeViewEx, true);
            if (item != null)
            {
                SelectSingleItem(item);
            }

            FocusHelper.Focus(item);
        }

        public void SelectNextFromKey()
        {
            TreeViewExItem item = GetFocusedItem();
            item = TreeViewElementFinder.FindNext(item, true);
            if (item == null) return;

            // if ctrl is pressed just focus it, so it can be selected by space. Otherwise select it.
            if (!IsControlKeyDown)
            {
                SelectSingleItem(item);
            }

            FocusHelper.Focus(item);
        }

        public void SelectPreviousFromKey()
        {
            List<TreeViewExItem> items = TreeViewElementFinder.FindAll(treeViewEx, true).ToList();
            TreeViewExItem item = GetFocusedItem();
            item = treeViewEx.GetPreviousItem(item, items);
            if (item == null) return;

            // if ctrl is pressed just focus it, so it can be selected by space. Otherwise select it.
            if (!IsControlKeyDown)
            {
                SelectSingleItem(item);
            }

            FocusHelper.Focus(item);
        }

        public void SelectCurrentBySpace()
        {
            TreeViewExItem item = GetFocusedItem();
            SelectSingleItem(item);
            FocusHelper.Focus(item);
        }

        public void ClearObsoleteItems(IEnumerable items)
        {
            foreach (var itemToUnSelect in items)
            {
                if (itemToUnSelect == lastShiftRoot)
                    lastShiftRoot = null;
                if (!treeViewEx.SelectedItems.IsReadOnly && treeViewEx.SelectedItems.Contains(itemToUnSelect))
                    treeViewEx.SelectedItems.Remove(itemToUnSelect);
            }
        }

        #endregion
    }
}
