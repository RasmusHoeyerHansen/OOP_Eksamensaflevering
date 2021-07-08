using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.Model
{
    [TestClass]
    public class SeasonalProductTests
    {
        [TestMethod]
        public void Constructor_CorrectData_NoException()
        {
            var future = DateTime.Now + new TimeSpan(19, 1, 1, 1);
            SeasonalProduct p1 = new SeasonalProduct("Dav1", 10m, DateTime.Now, future);
            SeasonalProduct p2 = new SeasonalProduct("Dav2", 10m, true, true, DateTime.Now, future);
            SeasonalProduct p3 = new SeasonalProduct("Dav2", 10m, false, true, DateTime.Now, future);
            SeasonalProduct p4 = new SeasonalProduct("Dav2", 10m, true, false, DateTime.Now, future);
            SeasonalProduct p5 = new SeasonalProduct("Dav2", 10m, false, false, DateTime.Now, future);


            Assert.IsNotNull(p1);
            Assert.IsNotNull(p2);
            Assert.IsNotNull(p3);
            Assert.IsNotNull(p4);
            Assert.IsNotNull(p5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EndTimeInPast_ThrowsArgumentOutOfrange()
        {
            var future = DateTime.Now + new TimeSpan(19, 1, 1, 1);
            SeasonalProduct p1 = new SeasonalProduct("Dav1", 10m, future, DateTime.Now);
        }
    }
}
