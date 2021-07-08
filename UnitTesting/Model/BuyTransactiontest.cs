using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.Model
{
    [TestClass]
    public class BuyTransactionTest
    {
        User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
        Product product = new Product("ProductName", 500);

        [TestMethod]
        public void Constructor_ValidData_NoException()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            Product product = new Product("ProductName", 500);

            BuyTransaction trans = new BuyTransaction(product.Price, user, product);

            Assert.IsNotNull(trans);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ProductIsNull_ThrowsArgumentNull()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);

            BuyTransaction trans = new BuyTransaction(product.Price, user, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_UserIsNull_ThrowsArgumentNull()
        {
            Product product = new Product("ProductName", 500);

            BuyTransaction trans = new BuyTransaction(product.Price, null, product);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_AmountIsNegative_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            Product product = new Product("ProductName", 500);

            BuyTransaction trans = new BuyTransaction(-1m, user, product);
        }

        [TestMethod]
        public void Execute_CallSubstractsToBalance_GivesUserNegativeBalance()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            Product product = new Product("ProductName", 500);
            BuyTransaction trans = new BuyTransaction(product.Price, user, product);
            var before = user.Balance;

            trans.Execute();

            decimal after = user.Balance;
            decimal expected = before - product.Price;

            Assert.AreEqual(expected, after);
        }
    }
}
