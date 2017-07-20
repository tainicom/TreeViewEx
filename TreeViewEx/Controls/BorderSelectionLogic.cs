// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BorderSelectionLogic.cs" company="Slompf Industries">
//   Copyright (c) Slompf Industries 2006 - 2012
// </copyright>
// <summary>
//   Defines the BorderSelectionLogic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace tainicom.TreeViewEx
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    #endregion

    internal class BorderSelectionLogic : InputSubscriberBase
    {
        #region Constants and Fields

        private readonly IEnumerable<TreeViewExItem> items;

        private BorderSelectionAdorner border;
                
        private Point? selectPosition;
        private MouseButton dragButton;
        bool beginSelect = false;

        #endregion

        #region Constructors and Destructors

        public BorderSelectionLogic(TreeViewEx treeView, IEnumerable<TreeViewExItem> items)
        {
            this.items = items;
            TreeView = treeView;
        }

        #endregion

        #region Methods

        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            // begin click
            if (!selectPosition.HasValue && !TreeView.IsVirtualizing) // TODO: Virtual Elements still don't work with Selection.
            {
                TreeViewExItem item = GetTreeViewItemUnderMouse(e.GetPosition(TreeView));
                if (item != null)
                {
                    var contentPresenter = item.Template.FindName("content", item) as ContentPresenter;
                    if (!contentPresenter.IsMouseOver)
                    {
                        selectPosition = GetMousePositionRelativeToContent();
                        dragButton = e.ChangedButton;
                    }
                }
            }
        }

        internal override void OnMouseMove(MouseEventArgs e)
        {
            if (selectPosition.HasValue)
            {            
                if (!beginSelect)
                {
                    // detect select
                    var dragDiff = selectPosition.Value - e.GetPosition(TreeView);
                    if ((Math.Abs(dragDiff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(dragDiff.Y) > SystemParameters.MinimumVerticalDragDistance))
                    {
                        // begin select
                        border = new BorderSelectionAdorner(TreeView);
                        beginSelect = true;
                    }
                }

                if (beginSelect)
                    HandleInput(e);
            }
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (selectPosition.HasValue && e.ChangedButton == dragButton)
            {
                if (beginSelect)
                {
                    border.Visibility = Visibility.Collapsed;
                    border.Dispose();
                    border = null;
                    
                    beginSelect = false;
                }

                selectPosition = null;
            }
        }

        internal override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            HandleInput(e);
        }

        private void HandleInput(RoutedEventArgs e)
        {
            if (beginSelect)
            {
                List<object> itemsToSelect = new List<object>();
                List<object> itemsToUnSelect = new List<object>();

                // if the mouse position or the start point is outside the window, we trim it inside
                Point currentPoint = TrimPointToVisibleArea(GetMousePositionRelativeToContent());
                Point trimmedStartPoint = TrimPointToVisibleArea(selectPosition.Value);
                
                Rect selectionRect = new Rect(currentPoint, trimmedStartPoint);
                border.UpdatePosition(selectionRect);
                
                if (!SelectionMultiple.IsControlKeyDown)
                {
                    foreach (var item in TreeView.SelectedItems)
                    {
                        var treeViewItem = TreeView.GetTreeViewItemFor(item);
                        Rect itemRect = GetPositionOf(treeViewItem);

                        if (!selectionRect.IntersectsWith(itemRect))
                            itemsToUnSelect.Add(item);
                    }
                }

                foreach (var item in items)
                {
                    if (!item.IsVisible || item.IsEditing)
                    {
                        continue;
                    }

                    Rect itemRect = GetPositionOf(item);

                    if (selectionRect.IntersectsWith(itemRect))
                    {
                        if (!TreeView.SelectedItems.Contains(item.DataContext))
                            itemsToSelect.Add(item.DataContext);
                    }
                    else
                    {
                        if (!SelectionMultiple.IsControlKeyDown && TreeView.SelectedItems.Contains(item.DataContext))
                            itemsToUnSelect.Add(item.DataContext);
                    }
                }
                ((SelectionMultiple)TreeView.Selection).SelectByRectangle(itemsToSelect, itemsToUnSelect);
            }
        }

        private Point TrimPointToVisibleArea(Point point)
        {
            return
               new Point(
                  Math.Max(
                     Math.Min(TreeView.ActualWidth + TreeView.ScrollViewer.ContentHorizontalOffset, point.X),
                     +TreeView.ScrollViewer.ContentHorizontalOffset),
                  Math.Max(
                     Math.Min(TreeView.ActualHeight + TreeView.ScrollViewer.ContentVerticalOffset, point.Y),
                     TreeView.ScrollViewer.ContentVerticalOffset));
        }

        private Rect GetPositionOf(TreeViewExItem treeViewItem)
        {
            FrameworkElement item = (FrameworkElement)treeViewItem.Template.FindName("border", treeViewItem);
            if (item == null)
            {
                throw new InvalidOperationException("Could not get content of item");
            }

            Point p = item.TransformToAncestor(TreeView).Transform(new Point());
            double itemLeft = p.X + TreeView.ScrollViewer.ContentHorizontalOffset;
            double itemTop = p.Y + TreeView.ScrollViewer.ContentVerticalOffset;

            return new Rect(itemLeft, itemTop, item.ActualWidth, item.ActualHeight);
        }
        #endregion
    }
}