namespace TreeViewEx.Test.Model
{
	#region
	using System.Windows.Automation;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using TreeViewEx.Test.Model.Helper;

	#endregion

	[TestClass]
    [DeploymentItem("SimpleSample.exe")]
    [DeploymentItem("TreeViewEx.dll")]
	public class ExpandCollapseTests
	{
		#region Public Properties

		public TestContext TestContext { get; set; }

		#endregion

		#region Public Methods

		[TestMethod]
		public void Collapse()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Expand();
				sst.Element1.Collapse();

				Assert.IsFalse(sst.Element1.IsExpanded);
			}
		}

		[TestMethod]
		public void Expand()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Expand();

				Assert.IsTrue(sst.Element1.IsExpanded);
			}
		}

		#endregion
	}
}