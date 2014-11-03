namespace TreeViewEx.ComplexSample.Model
{
	#region

	using System.Collections.ObjectModel;

	#endregion

	/// <summary>
	/// Model for testing
	/// </summary>
	public abstract class NodeBase
	{
		#region Constructors and Destructors

		public NodeBase()
		{
			Children = new ObservableCollection<NodeBase>();
			IsVisible = true;
			IsEditable = true;
		}

		#endregion

		#region Public Properties

		public string Name { get { return ToString(); } }

		public bool IsSelected { get; set; }

		public bool IsVisible { get; set; }

		public bool IsEditable { get; set; }

		public bool IsExpanded { get; set; }

		public ObservableCollection<NodeBase> Children { get; set; }

		public abstract string Tip { get; }
		#endregion

		#region Public Methods

		public override abstract string ToString();

		#endregion
	}
}