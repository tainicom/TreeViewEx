namespace TreeViewEx.ComplexSample
{
	#region

	using ComplexSample.Model;

	#endregion

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Constructors and Destructors

		public MainWindow()
		{

			DataContext = new Vm();

			InitializeComponent();
		}

		#endregion

		private void TreeViewEx_OnSelecting(object sender, System.Windows.Controls.SelectionChangedCancelEventArgs e)
		{
			foreach (NodeBase item in e.ItemsToSelect)
			{
				if (item.Name.ToLower().Contains("not selectable"))
				{
					e.Cancel = true;
					MessageLabel.Content = "Cannot select item " + item;
					break;
				}
			}
		}
	}
}