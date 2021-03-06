﻿using System;
using System.ComponentModel;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IContainer = Autofac.IContainer;

namespace CraigLib.TimeAndDate.Test
{
    [TestClass]
    public class TimeAndDateMonthSpanTests
    {
        private readonly IContainer _container;
        public TimeAndDateMonthSpanTests()
        {
            var diHelper = new DependencyInjectionHelper();
            _container = diHelper.Container;
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesSameMonth()
        {
            //Arrange
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2014, 10, 15);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(1, helper.NumberMonthsBetweenDates(d1, d2));
            }
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesDifferentMonthSameYear()
        {
            //Arrange
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2014, 11, 15);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(2, helper.NumberMonthsBetweenDates(d1, d2));
            }
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesDifferentMonthDifferentYear()
        {
            //Arrange
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 11, 15);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(13, helper.NumberMonthsBetweenDates(d1, d2));
            }
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesSameMonthDifferentYear()
        {
            //Arrange
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 10, 15);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(12, helper.NumberMonthsBetweenDates(d1, d2));
            }
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDatesBackwardsOrder()
        {
            //Arrange
            var d1 = new DateTime(2014, 10, 1);
            var d2 = new DateTime(2015, 10, 15);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(12, helper.NumberMonthsBetweenDates(d2, d1));
            }
        }

        [TestMethod]
        public void TestNumberMonthsBetweenDates1MonthSpansAYear()
        {
            //Arrange
            var d1 = new DateTime(2014, 12, 1);
            var d2 = new DateTime(2015, 1, 1);
            using (var scope = _container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<ITimeAndDateHelper>();
                //Act -> Assert
                Assert.AreEqual(1, helper.NumberMonthsBetweenDates(d1, d2));
            }
        }
    }
}
