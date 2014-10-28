using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CraigLib.TimeAndDate.Test
{
    [TestClass]
    public class TimeAndDateMonthSpanTests
    {
        [TestMethod]
        public void TestNumberMonthsBetweenDatesSameMonth()
        {
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2014, 10, 15);
            Assert.AreEqual(1, TimeAndDateHelper.NumberMonthsBetweenDates(d1, d2));
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesDifferentMonthSameYear()
        {
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2014, 11, 15);
            Assert.AreEqual(2, TimeAndDateHelper.NumberMonthsBetweenDates(d1, d2));
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesDifferentMonthDifferentYear()
        {
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 11, 15);
            Assert.AreEqual(13, TimeAndDateHelper.NumberMonthsBetweenDates(d1, d2));
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesSameMonthDifferentYear()
        {
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 10, 15);
            Assert.AreEqual(12, TimeAndDateHelper.NumberMonthsBetweenDates(d1, d2));
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesBackwardsOrder()
        {
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 10, 15);
            Assert.AreEqual(12, TimeAndDateHelper.NumberMonthsBetweenDates(d2, d1));
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDates1MonthSpansAYear()
        {
            var d1 = new DateTime(2014, 12, 1);
            var d2 = new DateTime(2015, 1, 1);
            Assert.AreEqual(1, TimeAndDateHelper.NumberMonthsBetweenDates(d1, d2));
        }
    }
}
