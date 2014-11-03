namespace W7StyleSample.Model
{
	#region

	using System.Collections.ObjectModel;
	using System.Windows;

	#endregion

	/// <summary>
	/// Model for testing
	/// </summary>
	public class Node
	{
		#region Constructors and Destructors

		public Node()
		{
			Children = new ObservableCollection<Node>();
			IsVisible = true;
		}

		#endregion

		#region Public Properties

		public ObservableCollection<Node> Children { get; set; }

		public string Name { get; set; }

		public Visibility Visibility { get; set; }
		public bool IsVisible { get; set; }
		#endregion

		#region Public Methods

		public override string ToString()
		{
			return Name;
		}

		#endregion
	}
}