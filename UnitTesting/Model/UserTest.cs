using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAndDal;
using ModelAndDal.Model.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddToBalance_NegativeAmount_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com",10000);
            user.AddToBalance(-1);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void AddToBalance_AddsCorrectly_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 10000);
            decimal before = user.Balance;
            user.AddToBalance(100);

            decimal expected = before + 100;
            Assert.AreEqual(expected, user.Balance);
        }

        [TestMethod]
        public void RemoveFromBalance_SubstractsCorrectly_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 10000);
            decimal before = user.Balance;
            user.RemoveFromBalance(100);

            decimal expected = before - 100;
            Assert.AreEqual(expected, user.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidMail_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemailÆØÅ@ma.m-gt12il.com", 10000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_nullmail_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", null, 10000);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_nullUserName_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", "hansen", null, "somemail@mail.com", 10000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_nullLastName_ThrowsArgumentOutOfRange()
        {
            User user = new User("rasmus", null,"rumle", "somemail@mail.com", 10000);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_nullFirstName_ThrowsArgumentException()
        {
            User user = new User(null, "hansen", "rumle", "somemail@mail.com", 10000);
        }

        [TestMethod]
        public void Equals_UsersAreIdentifiedByUsernameOnly()
        {
            User user = new User("ghf", "hansen", "rumle", "somemail@mail123.com", 1011000);
            User user1 = new User("NAME", "hdsadsaen", "rumle", "somemail@mail321.com", 10000);

            Assert.IsTrue(user.Equals(user1));
        }

        [TestMethod]
        public void ToString_Contains_NameLastNameAndEmail()
        {
            User user = new User("ghf", "hansen", "rumle", "somemail@mail123.com", 101100);

            Assert.IsNotNull(user);
            string result = user.ToString();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(user.FirstName));
            Assert.IsTrue(result.Contains(user.LastName));
            Assert.IsTrue(result.Contains(user.Email));
        }

        [TestMethod]
        public void Constructor_WithId_Succes()
        {
            User user = new User("ghf", "hansen", "rumle", "somemail@mail123.com", 101100);
            Assert.IsNotNull(user);
        }
    }

}
