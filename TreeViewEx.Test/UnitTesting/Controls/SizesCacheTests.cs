using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using FluentAssertions;

namespace TreeViewEx.Test.Model.UnitTesting.Controls.SizesCacheTests
{
    [TestClass]
    public class SizesCacheTests
    {
        [TestMethod]
        public void AddInEmptyCache()
        {
            var expected = 43.0;
            var target = new SizesCache();
            target.AddOrChange(0, expected);

            Assert.AreEqual(expected, target.GetEstimate(0));
        }

        [TestMethod]
        public void AddInFilledCache()
        {
            var target = new SizesCache();
            target.AddOrChange(0, 43);
            target.AddOrChange(0, 46);
            target.AddOrChange(0, 46);

            Assert.AreEqual(46, target.GetEstimate(0));
        }

        [TestMethod]
        public void Change()
        {
            var target = new SizesCache();
            target.AddOrChange(0, 43);
            target.AddOrChange(0, 46);
            target.AddOrChange(0, 46);
            target.AddOrChange(0, 43);
            target.AddOrChange(0, 43);

            Assert.AreEqual(43, target.GetEstimate(0));
        }

        [TestMethod]
        public void ChangeWithMaxItems()
        {
            var target = new SizesCache();
            target.AddOrChange(0, 1);
            target.AddOrChange(0, 1);
            target.AddOrChange(0, 3);
            target.AddOrChange(0, 3);
            target.AddOrChange(0, 5);
            target.AddOrChange(0, 7);
            target.AddOrChange(0, 7);
            target.AddOrChange(0, 9);
            target.AddOrChange(0, 9);
            target.AddOrChange(0, 11);
            target.AddOrChange(0, 11);
            target.AddOrChange(0, 11);

            Assert.AreEqual(11, target.GetEstimate(0));
        }

        [TestMethod]
        public void CleanUpEmptyCache()
        {
            var target = new SizesCache();
            target.Invoking(x => x.CleanUp(0)).ShouldNotThrow();
        }

        [TestMethod]
        public void CleanUpFilledCacheNormalCase()
        {
            var target = new SizesCache();
            target.AddOrChange(0, 40);
            target.AddOrChange(0, 41);
            target.AddOrChange(0, 42);
            target.AddOrChange(0, 43);

            target.CleanUp(0);
            Assert.AreEqual(0, target.GetEstimate(0));
        }
    }
}
