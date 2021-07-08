using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.Model
{
    [TestClass]
    public class InsertTransactionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullUserThrowsException()
        {
            InsertCashTransaction trans = new InsertCashTransaction(10, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void 
        amount_NegativeAmount_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            InsertCashTransaction trans = new InsertCashTransaction(-0.001m, user);
        }

        [TestMethod]
        public void Constructor_Succes_CreatesWithDateTime()
        {
            User user = new User("Rasmus", "Hansen", "rumle", "somemail@mail.com", 1000);
            InsertCashTransaction trans = new InsertCashTransaction(10, user);
            Assert.IsNotNull(trans);
            Assert.IsNotNull(trans.TransactionDate);
        }

        [TestMethod]
        public void Constructor_Succes_CreatesWithinOneMinute()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            InsertCashTransaction trans = new InsertCashTransaction(10, user);
            Assert.IsNotNull(trans);

            TimeSpan timeDifference = trans.TransactionDate - DateTime.Now;
            Assert.IsTrue(timeDifference.Minutes <= 1);
        }

        [TestMethod]
        public void Execute_CallAddsToBalance()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            InsertCashTransaction trans = new InsertCashTransaction(1000, user);
            decimal before = user.Balance;

            trans.Execute();

            decimal after = user.Balance;
            decimal expected = before + 1000m;

            Assert.AreEqual(expected, after);
        }
    }
}
