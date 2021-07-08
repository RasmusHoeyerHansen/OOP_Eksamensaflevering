using ModelAndDal;
using ModelAndDal.Model.Users;
using System;
using System.Collections.Generic;

namespace Controller
{
    public interface IStregsystem
    {
        public int MultiBuy(uint numberOfProducts, Product product, User user);
        public IEnumerable<Product> ActiveProducts { get; }
        public InsertCashTransaction AddCreditsToAccount(User user, int amount);
        public BuyTransaction BuyProduct(User user, Product product);
        public Product GetProductByID(int id);
        public IEnumerable<ITransaction> GetTransactions(User user, int count);
        public IEnumerable<User> GetUsers(Func<User, bool> predicate);
        public User GetUserByUsername(string username);

        event UserBalanceNotification UserBalanceWarning;

        public void DeactivateProduct(Product product);
        void ActivateProduct(Product product);
        void ActivateOnCredit(Product product);
        void DeactivateOnCredit(Product product);
    }
}