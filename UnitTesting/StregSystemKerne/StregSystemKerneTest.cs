using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using ModelAndDal;
using ModelAndDal.Model.Users;
using System.Linq;

namespace UnitTesting.StregSystemKerne
{
    [TestClass]
    public class StregSystemKerneTest
    {
        [TestMethod]
        public void Constructor_Correctdata_NoException()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            Assert.IsNotNull(system);
        }

        [TestMethod]
        public void Constructor_NoNullPropertiesOnInitalisation()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            Assert.IsNotNull(system.Users);
            Assert.IsNotNull(system.ActiveProducts);
            Assert.IsNotNull(system.Transactions);
        }

        [TestMethod]
        public void AddCreditToUser_AddsCreditToBalance()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 10000);
            var before = user.Balance;
            var amount = 100;
            var expected = user.Balance + amount;

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.AddCreditsToAccount(user, amount);

            Assert.AreEqual(user.Balance, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddCreditToUser_NegativeAmount_ThrowsArgOutOfRange()
        {
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 10000);
            var amount = -100;

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.AddCreditsToAccount(user, amount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCreditToUser_NullUser_ThrowsArgNull()
        {
            User user = null;
            var amount = 100;

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.AddCreditsToAccount(user, amount);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotActiveException))]
        public void BuyProduct_InactiveProductThrows_CustomException()
        {
            Product product = new Product("Dav1", 10m, true, false);
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 10000);
            BuyTransaction trans = new BuyTransaction(product.Price, user, product);

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            var res = system.BuyProduct(user, product);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientCreditsException))]
        public void BuyProduct_NotEnoughMoney_ThrowInssuficient_Exception()
        {
            Product product = new Product("Dav1", 1000m, false, true);
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 0);
            BuyTransaction trans = new BuyTransaction(product.Price, user, product);

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            var res = system.BuyProduct(user, product);
        }

        [TestMethod]
        public void BuyProduct_CanBuyWithProductAvailableOnCredit()
        {
            Product product = new Product("Dav1", 1000m, true, true);
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 0);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            var res = system.BuyProduct(user, product);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void BuyProduct_PositiveAmount_RemovesFromBalance()
        {
            var price = 100;
            Product product = new Product("Dav1", price, true, true);
            User user = new User("rasmus", "hansen", "rumle", "somemail@mail.com", 1000);
            var before = user.Balance;
            BuyTransaction trans = new BuyTransaction(product.Price, user, product);

            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            var res = system.BuyProduct(user, product);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Amount, price);
            Assert.AreEqual(user.Balance, before - price);
        }


        [TestMethod]
        public void DeactivateOnCredit_Succes()
        {
            Product product = new Product("Dav1", 1000m, true, true);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.DeactivateOnCredit(product);
            Assert.IsFalse(product.CanBeBoughtOnCredit);
        }

        [TestMethod]
        public void DeactivateProduct_Succes()
        {
            Product product = new Product("Dav1", 1000m, true, true);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.DeactivateProduct(product);
            Assert.IsFalse(product.IsActive);

        }

        [TestMethod]
        [ExpectedException(typeof(ProductAlreadyDeactivatedException))]
        public void DeactivateProduct_AlreadyDeactive_ThrowsException()
        {
            Product product = new Product("Dav1", 1000m, true, false);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.DeactivateProduct(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductAlreadyActivatedException))]
        public void ActivateProduct_AlreadyDeactive_ThrowsException()
        {
            Product product = new Product("Dav1", 1000m, true, true);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.ActivateProduct(product);
        }

        [TestMethod]
        public void ActivateProduct_Success()
        {
            Product product = new Product("Dav1", 1000m, true, false);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.ActivateProduct(product);
            Assert.IsTrue(product.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductAlreadyDeactivatedException))]
        public void DeactivateOnCredit_AlreadyDeactivated_Exception()
        {
            Product product = new Product("Dav1", 1000m, false, true);
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();

            system.DeactivateOnCredit(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void ActivateProduct_Null_ThrowsNull()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.ActivateProduct(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void DeactivateProduct_Null_ThrowsNull()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.DeactivateProduct(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void DeactivateOnCredit_Null_ThrowsNull()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.DeactivateOnCredit(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void ActivateOnCredit_Null_ThrowsNull()
        {
            Controller.StregSystemKerne system = new Controller.StregSystemKerne();
            system.ActivateOnCredit(null);
        }

    }
}
