namespace TreeViewEx.Test.Model
{
	#region

	using System.Windows.Automation;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using TreeViewEx.Test.Model.Helper;
	using System.Threading;

	#endregion

    [TestClass]
    [DeploymentItem("SimpleSample.exe")]
    [DeploymentItem("TreeViewEx.dll")]
	public class SelectionTests
	{
		#region Constants and Fields

		private const string FileName = "SimpleSample.exe";

		private const string ProcessName = "W7StyleSample";

		#endregion

		#region Public Properties

		public TestContext TestContext { get; set; }

		#endregion

		#region Public Methods

		[TestMethod]
		public void SelectElement1()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Select();
				Assert.IsTrue(sst.Element1.IsSelected);
			}
		}

		[TestMethod]
		public void SelectElement11()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Expand();
				sst.Element11.Select();
				Assert.IsTrue(sst.Element11.IsSelected);
			}
		}

		[TestMethod]
		public void SelectElement11ByClickOnIt()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Expand();
				sst.Element11.Select();
				Assert.IsTrue(sst.Element11.IsSelected);
			}
		}

        [TestMethod]
        public void ShiftRoot()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);
                sst.Element1.Expand();
                Mouse.ShiftClick(sst.Element11);
                Mouse.ShiftClick(sst.Element13);
                Mouse.ShiftClick(sst.Element15);

                Assert.IsTrue(sst.Element11.IsSelected);
                Assert.IsTrue(sst.Element12.IsSelected);
                Assert.IsTrue(sst.Element13.IsSelected);
                Assert.IsTrue(sst.Element14.IsSelected);
                Assert.IsTrue(sst.Element15.IsSelected);
            }
        }

        [TestMethod]
        public void SelectWithShiftArrows()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);
                sst.Element1.Select();
                Keyboard.Right();
                Keyboard.Down();
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after down");
                Assert.IsTrue(sst.Element11.IsFocused, "Element11 has not focus after down");

                Keyboard.ShiftDown();
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after second down");
                Assert.IsTrue(sst.Element12.IsSelected, "Element12 not selected after second down");
                Assert.IsTrue(sst.Element12.IsFocused, "Element12 has not focus after second down");

                Keyboard.Down();
                Assert.IsFalse(sst.Element11.IsSelected, "Element11 selected after second down");
                Assert.IsFalse(sst.Element12.IsSelected, "Element12 selected after second down");
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after down");
                Assert.IsTrue(sst.Element13.IsFocused, "Element13 has not focus after down");

                Keyboard.ShiftUp();
                Keyboard.ShiftUp();
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 selected after up");
                Assert.IsTrue(sst.Element12.IsSelected, "Element12 selected after up");
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after up");
                Assert.IsTrue(sst.Element11.IsFocused, "Element13 has not focus after up");


                Keyboard.ShiftDown();
                Keyboard.ShiftDown();
                Keyboard.ShiftDown();
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after fourth down");
                Assert.IsTrue(sst.Element14.IsSelected, "Element14 not selected after fourth down");
                Assert.IsTrue(sst.Element14.IsFocused, "Element14 has not focus after fourth down");

                Keyboard.ShiftUp();
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after up");
                Assert.IsTrue(sst.Element13.IsFocused, "Element13 has not focus after up");
                Assert.IsFalse(sst.Element14.IsSelected, "Element14 not selected after up");
            }
        }

		[TestMethod]
		public void DeSelectElementWithControlAndClick()
		{
			using (TreeApplication app = new TreeApplication("SimpleSample"))
			{
				SimpleSampleTree sst = new SimpleSampleTree(app);
				sst.Element1.Expand();
				Mouse.CtrlClick(sst.Element11);
				Thread.Sleep(100);
				Mouse.CtrlClick(sst.Element11);
				Assert.IsFalse(sst.Element11.IsSelected);
				// we cannot test for focus, because the automation peer sets focus by itself
				// Assert.IsFalse(sst.Element11.IsFocused);
			}
		}
		#endregion
	}
}