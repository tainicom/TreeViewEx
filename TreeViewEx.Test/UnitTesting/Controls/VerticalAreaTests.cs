using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TreeViewEx.Test.Model.UnitTesting.Controls.SizesCacheTests
{
    [TestClass]
    public class VerticalAreaTests
    {
        [TestMethod]
        public void IsWithin()
        {
            var va = new VerticalArea { Top = 100, Bottom = 200 };
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 100, Bottom = 200 }));
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 110, Bottom = 200 }));
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 100, Bottom = 150 }));
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 150, Bottom = 250 }));
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 0, Bottom = 150 }));
            Assert.IsTrue(va.IsWithin(new VerticalArea { Top = 0, Bottom = 300 }));


            Assert.IsFalse(va.IsWithin(new VerticalArea { Top = 0, Bottom = 10 }));
            Assert.IsFalse(va.IsWithin(new VerticalArea { Top = 250, Bottom = 300 }));
        }
    }
}
