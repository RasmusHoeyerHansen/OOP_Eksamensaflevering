using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.Model
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Constructor_CorrectData_NoException()
        {
            Product p1 = new Product("Dav1", 10m, true, true);
            Product p2= new Product("Dav2", 0m, false, true);
            Product p3 = new Product("Dav3", 10000m, true, false);
            Product p4 = new Product("Dav4", 10m, false, false);

            Assert.IsNotNull(p1);
            Assert.IsNotNull(p2);
            Assert.IsNotNull(p3);
            Assert.IsNotNull(p4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeAmount_ThrowsArgumentOutOfRange()
        {
            Product p1 = new Product("Dav1", -10m);
        }

        [TestMethod]
        public void Constructor_availableOnCredit_FalseOnNoSpecification()
        {
            Product p1 = new Product("Dav1", 10m);
            Assert.IsFalse(p1.CanBeBoughtOnCredit);
        }

        [TestMethod]
        public void Constructor_isActiveOnCreation_FalseOnNoSpecification()
        {
            Product p1 = new Product("Dav1", 10m);
            Assert.IsTrue(p1.IsActive);
        }
    }
}
