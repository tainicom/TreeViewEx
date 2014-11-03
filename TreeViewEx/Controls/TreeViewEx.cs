// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Defines the TreeViewEx type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace System.Windows.Controls
{
    #region

    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls.DragNDrop;
    using System.Windows.Input;
    using System.Windows.Media;

    #endregion

    public class TreeViewEx : ItemsControl
    {
        #region Constants and Fields
        // the space where items will be realized if virtualization is enabled. This is set by virtualizingtreepanel.
        internal VerticalArea realizationSpace = new VerticalArea();
        internal SizesCache cachedSizes = new SizesCache();

        public static readonly DependencyProperty DragTemplateProperty = DependencyProperty.Register(
           "DragTemplate", typeof(DataTemplate), typeof(TreeViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty InsertTemplateProperty = DependencyProperty.Register(
           "InsertTemplate", typeof(DataTemplate), typeof(TreeViewEx), new PropertyMetadata(null));

        public static DependencyProperty InsertionMarkerBrushProperty =
           DependencyProperty.Register(
              "InsertionMarkerBrush",
              typeof(Brush),
              typeof(TreeViewEx),
              new FrameworkPropertyMetadata(Brushes.Black, null));

        public static readonly DependencyProperty IsDragNDropEnabledProperty =
           DependencyProperty.Register(
              "IsDragNDropEnabled", typeof(bool), typeof(TreeViewEx), new PropertyMetadata(false));

        public static DependencyProperty BackgroundSelectionRectangleProperty =
           DependencyProperty.Register(
              "BackgroundSelectionRectangle",
              typeof(Brush),
              typeof(TreeViewEx),
              new FrameworkPropertyMetadata(Brushes.LightBlue, null));

        public static DependencyProperty BorderBrushSelectionRectangleProperty =
           DependencyProperty.Register(
              "BorderBrushSelectionRectangle",
              typeof(Brush),
              typeof(TreeViewEx),
              new FrameworkPropertyMetadata(Brushes.Blue, null));

        public static DependencyProperty LastSelectedItemProperty =
           DependencyProperty.Register("LastSelectedItem", typeof(object), typeof(TreeViewEx), new FrameworkPropertyMetadata(null, OnLastSelectedItemPropertyChanged));

        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
           "SelectedItems",
           typeof(IList),
           typeof(TreeViewEx),
           new FrameworkPropertyMetadata(
              null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsPropertyChanged));
        
        public static readonly DependencyProperty IsVirtualizingProperty =
            DependencyProperty.Register("IsVirtualizing", typeof(bool), typeof(TreeViewEx), new PropertyMetadata(false));
        

        internal TreeViewExAutomationPeer automationPeer;

        private DragNDropController dragNDropController;

        private InputEventRouter inputEventRouter;

        private bool isInitialized;

        private ScrollViewer scroller;

        #endregion

        #region Constructors and Destructors

        static TreeViewEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewEx), new FrameworkPropertyMetadata(typeof(TreeViewEx)));

            FrameworkElementFactory vPanel = new FrameworkElementFactory(typeof(VirtualizingTreePanel));
            vPanel.SetValue(VirtualizingTreePanel.IsItemsHostProperty, true);
            ItemsPanelTemplate vPanelTemplate = new ItemsPanelTemplate();
            vPanelTemplate.VisualTree = vPanel;
            ItemsPanelProperty.OverrideMetadata(typeof(TreeViewEx), new FrameworkPropertyMetadata(vPanelTemplate));

            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TreeViewEx), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TreeViewEx), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
        }

        public TreeViewEx()
        {
            SelectedItems = new ObservableCollection<object>();

            var depPropDescriptorIsDragNDropEnabled = DependencyPropertyDescriptor.FromProperty(IsDragNDropEnabledProperty, typeof(TreeViewEx));
            depPropDescriptorIsDragNDropEnabled.AddValueChanged(this, OnIsDragNDropEnabledChanged);
        }

        #endregion

        #region Public Events

        public event EventHandler<SelectionChangedCancelEventArgs> OnSelecting;

        #endregion

        #region Properties

        public bool IsVirtualizing
        {
            get { return (bool)GetValue(IsVirtualizingProperty); }
            set { SetValue(IsVirtualizingProperty, value); }
        }

        public Brush BackgroundSelectionRectangle
        {
            get
            {
                return (Brush)GetValue(BackgroundSelectionRectangleProperty);
            }

            set
            {
                SetValue(BackgroundSelectionRectangleProperty, value);
            }
        }

        public Brush BorderBrushSelectionRectangle
        {
            get
            {
                return (Brush)GetValue(BorderBrushSelectionRectangleProperty);
            }

            set
            {
                SetValue(BorderBrushSelectionRectangleProperty, value);
            }
        }

        public Brush InsertionMarkerBrush
        {
            get
            {
                return (Brush)GetValue(InsertionMarkerBrushProperty);
            }

            set
            {
                SetValue(InsertionMarkerBrushProperty, value);
            }
        }

        public DataTemplate DragTemplate
        {
            get
            {
                return (DataTemplate)GetValue(DragTemplateProperty);
            }

            set
            {
                SetValue(DragTemplateProperty, value);
            }
        }

        public DataTemplate InsertTemplate
        {
            get
            {
                return (DataTemplate)GetValue(InsertTemplateProperty);
            }

            set
            {
                SetValue(InsertTemplateProperty, value);
            }
        }

        public bool IsDragNDropEnabled
        {
            get
            {
                return (bool)GetValue(IsDragNDropEnabledProperty);
            }

            set
            {
                SetValue(IsDragNDropEnabledProperty, value);
            }
        }

        /// <summary>
        ///   Gets the last selected item.
        /// </summary>
        public object LastSelectedItem
        {
            get
            {
                return GetValue(LastSelectedItemProperty);
            }

            private set
            {
                SetValue(LastSelectedItemProperty, value);
            }
        }

        internal IsEditingManager IsEditingManager { get; set; }

        /// <summary>
        ///   Gets or sets a list of selected items and can be bound to another list. If the source list implements <see
        ///    cref="INotifyPropertyChanged" /> the changes are automatically taken over.
        /// </summary>
        public IList SelectedItems
        {
            get
            {
                return (IList)GetValue(SelectedItemsProperty);
            }

            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        internal DragNDropController DragNDropController
        {
            get
            {
                return dragNDropController;
            }
        }

        internal ScrollViewer ScrollViewer
        {
            get
            {
                if (scroller == null)
                {
                    scroller = (ScrollViewer)Template.FindName("scroller", this);
                }

                return scroller;
            }
        }

        internal ISelectionStrategy Selection { get; private set; }

        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!isInitialized)
            {
                Loaded += OnLoaded;
                Unloaded += OnUnLoaded;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var autoScroller = new AutoScroller();
            dragNDropController = new DragNDropController(autoScroller);
            var selectionMultiple = new SelectionMultiple(this);
            Selection = selectionMultiple;
            IsEditingManager = new IsEditingManager(this);

            inputEventRouter = new InputEventRouter(this);
            inputEventRouter.Add(IsEditingManager);
            inputEventRouter.Add(autoScroller);
            inputEventRouter.Add(dragNDropController);
            inputEventRouter.Add(selectionMultiple.BorderSelectionLogic);
            inputEventRouter.Add(selectionMultiple);
            isInitialized = true;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            if (inputEventRouter != null)
            {
                inputEventRouter.Dispose();
                inputEventRouter = null;
            }

            if (dragNDropController != null)
            {
                dragNDropController.Dispose();
                dragNDropController = null;
            }

            Selection = null;
        }

        internal bool CheckSelectionAllowed(object item, bool isItemAdded)
        {
            if (isItemAdded)
            {
                return CheckSelectionAllowed(new List<object> { item }, new List<object>());
            }

            return CheckSelectionAllowed(new List<object>(), new List<object> { item });
        }

        internal bool CheckSelectionAllowed(IEnumerable<object> itemsToSelect, IEnumerable<object> itemsToUnselect)
        {
            if (OnSelecting != null)
            {
                var e = new SelectionChangedCancelEventArgs(itemsToSelect, itemsToUnselect);
                foreach (var method in OnSelecting.GetInvocationList())
                {
                    method.Method.Invoke(method.Target, new object[] { this, e });

                    // stop iteration if one subscriber wants to cancel
                    if (e.Cancel)
                    {
                        return false;
                    }
                }

                return true;
            }

            return true;
        }

        internal IEnumerable<TreeViewExItem> GetChildren(TreeViewExItem item)
        {
            if (item == null) yield break;
            for (int i = 0; i < item.Items.Count; i++)
			{
                TreeViewExItem child = item.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewExItem;
                if (child != null) yield return child;
			}
        }

        internal TreeViewExItem GetNextItem(TreeViewExItem item, List<TreeViewExItem> items)
        {
            int indexOfCurrent = items.IndexOf(item);

            for (int i = indexOfCurrent + 1; i < items.Count; i++)
            {
                if (items[i].IsVisible)
                {
                    return items[i];
                }
            }

            return null;
        }

        internal IEnumerable<TreeViewExItem> GetNodesToSelectBetween(TreeViewExItem firstNode, TreeViewExItem lastNode)
        {
            var allNodes = TreeViewElementFinder.FindAll(this, false).ToList();
            var firstIndex = allNodes.IndexOf(firstNode);
            var lastIndex = allNodes.IndexOf(lastNode);

            if (firstIndex >= allNodes.Count)
            {
                throw new InvalidOperationException(
                   "First node index " + firstIndex + "greater or equal than count " + allNodes.Count + ".");
            }

            if (lastIndex >= allNodes.Count)
            {
                throw new InvalidOperationException(
                   "Last node index " + lastIndex + " greater or equal than count " + allNodes.Count + ".");
            }

            var nodesToSelect = new List<TreeViewExItem>();

            if (lastIndex == firstIndex)
            {
                return new List<TreeViewExItem> { firstNode };
            }

            if (lastIndex > firstIndex)
            {
                for (int i = firstIndex; i <= lastIndex; i++)
                {
                    if (allNodes[i].IsVisible)
                    {
                        nodesToSelect.Add(allNodes[i]);
                    }
                }
            }
            else
            {
                for (int i = firstIndex; i >= lastIndex; i--)
                {
                    if (allNodes[i].IsVisible)
                    {
                        nodesToSelect.Add(allNodes[i]);
                    }
                }
            }

            return nodesToSelect;
        }

        /// <summary>
        /// Send down the IsVirtualizing property if it's set on this element.
        /// </summary>
        /// <param name="element">
        /// <param name="item">
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            TreeViewExItem.IsVirtualizingPropagationHelper(this, element);
        }

        internal TreeViewExItem GetPreviousItem(TreeViewExItem item, List<TreeViewExItem> items)
        {
            int indexOfCurrent = items.IndexOf(item);
            for (int i = indexOfCurrent - 1; i >= 0; i--)
            {
                if (items[i].IsVisible)
                {
                    return items[i];
                }
            }

            return null;
        }

        internal TreeViewExItem GetTreeViewItemFor(object item)
        {
            foreach (var treeViewExItem in TreeViewElementFinder.FindAll(this, false))
            {
                if (item == treeViewExItem.DataContext)
                {
                    return treeViewExItem;
                }
            }

            return null;
        }

        internal IEnumerable<TreeViewExItem> GetTreeViewItemsFor(IEnumerable objects)
        {
            if (objects == null)
            {
                yield break;
            }
            var items = objects.Cast<object>().ToList();
            foreach (var treeViewExItem in TreeViewElementFinder.FindAll(this, false))
            {
                if (items.Contains(treeViewExItem.DataContext))
                {
                    yield return treeViewExItem;
                }
            }

        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewExItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeViewExItem;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            if (automationPeer == null)
            {
                automationPeer = new TreeViewExAutomationPeer(this);
            }

            return automationPeer;
        }
        
        private static void OnLastSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TreeViewEx)) throw new InvalidOperationException("Setting LastSelectedItem is externally is not allowed. Please use OneWay-Binding.");
        }

        private static void OnSelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewEx treeView = (TreeViewEx)d;
            if (e.OldValue != null)
            {
                INotifyCollectionChanged collection = e.OldValue as INotifyCollectionChanged;
                if (collection != null)
                {
                    collection.CollectionChanged -= treeView.OnSelectedItemsChanged;
                }
            }

            if (e.NewValue != null)
            {
                INotifyCollectionChanged collection = e.NewValue as INotifyCollectionChanged;
                if (collection != null)
                {
                    collection.CollectionChanged += treeView.OnSelectedItemsChanged;
                }
            }
        }

        private void OnIsDragNDropEnabledChanged(object sender, EventArgs e)
        {
            dragNDropController.Enabled = IsDragNDropEnabled;
        }

        private void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    object last = null;
                    foreach (var item in GetTreeViewItemsFor(e.NewItems))
                    {
                        item.IsSelected = true;

                        last = item.DataContext;
                    }

                    LastSelectedItem = last;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in GetTreeViewItemsFor(e.OldItems))
                    {
                        item.IsSelected = false;
                        if (item.DataContext == LastSelectedItem)
                        {
                            if (SelectedItems.Count > 0)
                            {
                                LastSelectedItem = SelectedItems[SelectedItems.Count - 1];
                            }
                            else
                            {
                                LastSelectedItem = null;
                            }
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in TreeViewElementFinder.FindAll(this, false))
                    {
                        if (item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                    }

                    LastSelectedItem = null;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     This method is invoked when the Items property changes.
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    if (Selection != null) // happens during unload
                        Selection.ClearObsoleteItems(e.OldItems.Cast<object>());
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
        #endregion
    }
}