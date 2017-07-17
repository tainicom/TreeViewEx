namespace BindingWithControllerSample
{
    #region

    using BindingWithControllerSample.CommandMgr;
    using BindingWithControllerSample.CommandMgr.Commands;
    using BindingWithControllerSample.Model;
    using BindingWithControllerSample.ViewModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using tainicom.TreeViewEx;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors

        MainViewModel viewModel;

        Node first1;
        Node first2;
        Node first11;
        
        public MainWindow()
        {
            InitializeComponent();
            
            viewModel = new MainViewModel();

            first1 = new Node { Name = "element1" };
            first2 = new Node { Name = "element2" };
            first11 = new Node { Name = "element11" };
            var first12 = new Node { Name = "element12" };
            var first13 = new Node { Name = "element13" };
            var first14 = new Node { Name = "element14" };
            var first15 = new Node { Name = "element15" };
            var first131 = new Node { Name = "element131" };
            var first132 = new Node { Name = "element132" };

            viewModel.Children.Add(first1);
            viewModel.Children.Add(first2);
            first1.Children.Add(first11);
            first1.Children.Add(first12);
            first1.Children.Add(first13);
            first1.Children.Add(first14);
            first1.Children.Add(first15);
            first13.Children.Add(first131);
            first13.Children.Add(first132);
             for (int i = 0; i < 10; i++)
             {
                first13.Children.Add(new Node { Name = "element13" + (i) });
             }

            DataContext = viewModel;

            leftTree.OnSelecting += treeViewEx_OnSelecting;
            rightTree.OnSelecting += treeViewEx_OnSelecting;
            
        }
        
        void treeViewEx_OnSelecting(object sender, SelectionChangedCancelEventArgs e)
        {
            TreeViewEx treeViewEx = (TreeViewEx)sender;
            CommandController controller = viewModel.Controller;

            e.Cancel = true;

            List<CommandBase> cmdList = new List<CommandBase>();
            foreach (object itemToUnselect in e.ItemsToUnSelect)
            {
                //if (!treeViewEx.SelectedItems.Contains(itemToUnselect)) continue;
                cmdList.Add(new DeselectItemCmd(viewModel, (Node)itemToUnselect));
            }
            foreach (object itemToSelect in e.ItemsToSelect)
            {
                //if (treeViewEx.SelectedItems.Contains(itemToSelect)) continue;
                cmdList.Add(new SelectItemCmd(viewModel, (Node)itemToSelect));

                //TreeViewExItem tvei = leftTree.GetTreeViewItemFor(itemToSelect);
                //if (tvei != null) tvei.BringIntoView(new Rect(1, 1, 1, 1));

            }
            if (cmdList.Count > 0) controller.AddAndExecute(new CommandGroupCmd(viewModel, cmdList.ToArray()));
            
            lblUndoStack.Content = controller.UndoCount;
            lblRedoStack.Content = controller.RedoCount;

            return;
        }

        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled) return;

            bool alt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            // DO - UNDO
            CommandController controller = viewModel.Controller;
            if (!alt && ctrl && !shift && e.Key == Key.Z)
                controller.UndoCommand();
            if (!alt && ctrl && !shift && e.Key == Key.Y)
                controller.ExecuteCommand();

            lblUndoStack.Content = controller.UndoCount;
            lblRedoStack.Content = controller.RedoCount;

            //if (!alt && !ctrl && !shift)
            //{
            //    leftTree.SelectNextName(e.Key);
            //}
                        
            return;
        }

    }
}