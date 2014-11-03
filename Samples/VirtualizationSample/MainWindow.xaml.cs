namespace TreeViewEx.VirtualizationSample
{
    #region

    using System.Windows.Interop;

    using W7StyleSample.Model;
    using System.Diagnostics;
    using System.Windows;
    using System;
    using System.Windows.Threading;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors
        Node node0;
        public MainWindow()
        {
            InitializeComponent();

            node0 = new Node { Name = "element" };
            const int firstLevelCount = 200;
            const int thirdLevelCount = 100;
            for (int i = 0; i < firstLevelCount; i++)//
            {
                var node1 = new Node { Name = string.Format("element_{0}", i + 1) };
                node0.Children.Add(node1);

                var node11 = new Node { Name = string.Format("element_{0}_1", i + 1) };
                node1.Children.Add(node11);
                for (int j = 0; j < thirdLevelCount; j++)
                {
                    var node11x = new Node { Name = string.Format("element_{0}_1_{1}", i + 1, j + 1) };
                    node11.Children.Add(node11x);
                }

                var node12 = new Node { Name = string.Format("element_{0}_2", i + 1) };
                node1.Children.Add(node12);
                for (int j = 0; j < thirdLevelCount; j++)
                {
                    var node12x = new Node { Name = string.Format("element_{0}_2_{1}", i + 1, j + 1) };
                    node12.Children.Add(node12x);
                }

                var node13 = new Node { Name = string.Format("element_{0}_3", i + 1) };
                node1.Children.Add(node13);
                for (int j = 0; j < thirdLevelCount; j++)
                {
                    var node13x = new Node { Name = string.Format("element_{0}_3_{1}", i + 1, j + 1) };
                    node13.Children.Add(node13x);
                }
            }

            GC.Collect();
            MemoryAfterLoad = GC.GetTotalMemory(true);
        }

        #endregion

        public static readonly DependencyProperty TimeToLoadProperty = DependencyProperty.Register(
           "TimeToLoad", typeof(long), typeof(MainWindow), new PropertyMetadata(default(long)));

        public long TimeToLoad
        {
            get
            {
                return (long)GetValue(TimeToLoadProperty);
            }
            set
            {
                SetValue(TimeToLoadProperty, value);
            }
        }

        public static readonly DependencyProperty MemoryAfterLoadProperty = DependencyProperty.Register(
           "MemoryAfterLoad", typeof(long), typeof(MainWindow), new PropertyMetadata(default(long)));

        public long MemoryAfterLoad
        {
            get
            {
                return (long)GetValue(MemoryAfterLoadProperty);
            }
            set
            {
                SetValue(MemoryAfterLoadProperty, value);
            }
        }

        private Stopwatch sw;
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            DataContext = node0;
            ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);
        }

        void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            ComponentDispatcher.ThreadIdle -= new EventHandler(ComponentDispatcher_ThreadIdle);
            sw.Stop();
            TimeToLoad = sw.ElapsedMilliseconds;
            MemoryAfterLoad = GC.GetTotalMemory(true);
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            TimeToLoad = 0;
            GC.Collect();
            MemoryAfterLoad = GC.GetTotalMemory(true);
        }
    }
}