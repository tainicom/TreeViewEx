namespace W7StyleSample.Model
{
	#region

	using System.Collections.ObjectModel;

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
            IsExpandedValue = true;
		}

		#endregion

		#region Public Properties

		public ObservableCollection<Node> Children { get; set; }
        string name;
        public string Name { get { return name; } set { name = value; } }
        public bool IsExpandedValue { get; set; }
		#endregion

		#region Public Methods

		public override string ToString()
		{
			return Name;
		}

		#endregion
	}
}