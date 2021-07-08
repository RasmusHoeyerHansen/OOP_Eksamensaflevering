using ModelAndDal;
using ModelAndDal.Model.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UI;

namespace Controller
{
    public class StregSystemKerne : IStregsystem
    {
        public event UserBalanceNotification UserBalanceWarning;

        // Files to read data from.
        private const string _productFileName = "products.csv";
        private const string _userFileName = "users.csv";
        private const string _insertTransFileName = "insertTransactions.csv";
        private const string _buyTransFileName = "buyTransactions.csv";

        // Changing from list to IEnumerable gives unexpected problems when
        // parsing csv data in LoadFile
        public List<User> Users { get; } = new List<User>();
        private List<Product> Products { get; } = new List<Product>();
        public IEnumerable<Product> ActiveProducts { get => Products.Where(p => p.IsActive); }
        public List<ITransaction> Transactions { get; } = new List<ITransaction>();

        private readonly string dataLocationPath = Path.Combine(Environment.CurrentDirectory.Replace("CLI", "ModelAndDal"), "Data");

        public StregSystemKerne()
        {
            LoadData();
            SubscribeToModelEvents();
        }

        /// <summary>
        /// Subscribe to all User in Users' UserBalanceWarning.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            foreach (User user in Users)
            {
                user.UserBalanceWarning += (user, amount) => UserBalanceWarning?.Invoke(user, amount);
            }
        }

        /// <summary>
        /// Load data from the CSV files using paths defined as private constants.
        /// </summary>
        private void LoadData()
        {
            // Read from ...bin/CLI/bin/debug/netcoreApp.../Data/*FILENAME.csv*
            Console.WriteLine("loading data .. " + dataLocationPath);
            string usersCsvPath = Path.Combine(dataLocationPath, _userFileName);
            string productsCsvPath = Path.Combine(dataLocationPath, _productFileName);
            string buyTransactionsCsvPath = Path.Combine(dataLocationPath, _buyTransFileName);
            string insertCashTransactionCsvPath = Path.Combine(dataLocationPath, _insertTransFileName);

            LoadFromCsvFile(new CsvDataReader<CsvUserData>(','), usersCsvPath,
                csvData => Users.Add((User)csvData));

            LoadFromCsvFile(new CsvDataReader<CsvProductData>(';'), productsCsvPath,
                (  (csvData) => Products.Add((Product)csvData)) );
        }

        /// <summary>
        /// LoadFromCsvFile using CsvDataReader<T> that implements ICsvData. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <param name="path"></param>
        /// <param name="callbackForEachItem"></param>
        private void LoadFromCsvFile<T>(CsvDataReader<T> dataReader, string path, Action<T> callbackForEachItem)
            where T : ICsvData, new()
        {
            IEnumerable<T> csvData = dataReader.ReadFile(path);

            foreach (T item in csvData)
            {
                callbackForEachItem?.Invoke(item);
            }
        }

        /// <summary>
        /// Creates InsertCashTransaction and performs it.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns>The transaction performed.</returns>
        public InsertCashTransaction AddCreditsToAccount(User user, int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount) + " must not be less than 0");
            }
            else if (user == null)
            {
                throw new ArgumentNullException(nameof(user) + " must not be null");

            }
            InsertCashTransaction transaction = new InsertCashTransaction( (decimal)amount, user);
            return (InsertCashTransaction)PerformTransaction(transaction);
        }

        /// <summary>
        /// Creates BuyTransaction and performs it.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="product"></param>
        /// <returns>The transaction performed.</returns>
        public BuyTransaction BuyProduct(User user, Product product)
        {
            if (product == null)
                throw new ProductNotFoundException();

            if (!product.IsActive)
            {
                throw new ProductNotActiveException(product.Name + "is not active. Buy something else.");
            }

            if (user.Balance < product.Price && !product.CanBeBoughtOnCredit)
            {
                string message = string.Format("{0} tried to buy {1} for {2} but only had {3} on their account." +
                    " Don't let them gamble with an economy like this!", user.Username, product.Name, product.Price, user.Balance);
                throw new InsufficientCreditsException(message);
            }

            BuyTransaction buyTransaction = new BuyTransaction(product.Price, user, product);

            return (BuyTransaction)PerformTransaction(buyTransaction);
        }

        /// <summary>
        /// Gets the product with ID equal to inputted int.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductByID(int id)
        {
            Product p = Products.Find(p => p.Id == id);
            if (p == null)
                new ProductNotFoundException("No such entity");

            return p;
        }

        /// <summary>
        /// Get the first count number of the the users' transactions.
        /// transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns>IEnumerable of the transactions.</returns>
        public IEnumerable<ITransaction> GetTransactions(User user, int count)
        {
            return (IEnumerable<ITransaction>)Transactions
                .Where(t => t.User.Equals(user))
                .OrderBy(t => t.TransactionDate)
                .Take(count);
        }

        /// <summary>
        /// Get the user specified by username.
        /// Users are identified only by username per assignment!
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUserByUsername(string username)
        {
            User user = Users.ToList().Find(u => u.Username == username);

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            return user;
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Get all users satisfying the predicate</returns>
        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            IEnumerable<User> users = Users.ToList().FindAll(u => predicate(u));

            return users;
        }

        /// <summary>
        /// Performs transactions by calling the Execute method specified in ITransaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>the transaction performed.</returns>
        private ITransaction PerformTransaction(ITransaction transaction)
        {
            transaction.Execute();
            transaction.WriteContentToCsvFile();
            Transactions.Add(transaction);

            return transaction;
        }


        //  TODO: Perform the transactions. Make sure all products 
        //  gets bought so you can't spend more money than you have


        /// <summary>
        /// Creates multiple BuyTransaction and returns them.
        /// </summary>
        /// <param name="numberOfProducts"></param>
        /// <param name="product"></param>
        /// <param name="user"></param>
        /// <returns>The list of transactions to buy.</returns>
        public int MultiBuy(uint numberOfProducts, Product product, User user)
        {
            List<BuyTransaction> transactions = new List<BuyTransaction>();

            for (int i = 0; i < numberOfProducts; i++)
            {
                 transactions.Add(BuyProduct(user, product));
            }

            return transactions.Count;
        }

        /// <summary>
        /// Set a products isActive property to true.
        /// Throws ProductAlreadyActivatedException if already true
        /// </summary>
        /// <param name="p"></param>
        public void ActivateProduct(Product p)
        {
            if (p == null)
            {
                throw new ProductNotFoundException();
            }
            else if (p.IsActive == false)
            {
                p.IsActive = true;
                ActiveProducts.ToList().Remove(p);
                return;
            }

            throw new ProductAlreadyActivatedException(p.Name + " is already active.");
        }

        /// <summary>
        /// Set a products isActive property to false.
        /// Throws ProductAlreadyDeativatedException if already false
        /// </summary>
        /// <param name="p"></param>
        public void DeactivateProduct(Product p)
        {
            
            if (p == null)
            {
                throw new ProductNotFoundException();
            }
            else if (p.IsActive == true)
            {
                p.IsActive = false;
                ActiveProducts.ToList().Remove(p);
                return;
            }

            throw new ProductAlreadyDeactivatedException(p.Name + " is already deactive.");
        }

        /// <summary>
        /// Set a products CanBeBoughtOnCredit property to true.
        /// Throws ProductAlreadyDeativatedException if already true
        /// </summary>
        /// <param name="p"></param>
        public void ActivateOnCredit(Product p)
        {
            if (p == null)
            {
                throw new ProductNotFoundException();
            }
            else if (p.CanBeBoughtOnCredit == false)
            {
                p.CanBeBoughtOnCredit = true;
                ActiveProducts.ToList().Remove(p);
                return;
            }

            throw new ProductAlreadyActivatedException(p.Name + "  is already active.");
        }

        /// <summary>
        /// Set a products CanBeBoughtOnCredit property to false.
        /// Throws ProductAlreadyDeativatedException if already false
        /// </summary>
        /// <param name="p"></param>
        public void DeactivateOnCredit(Product p)
        { 
            if (p == null)
            {
                throw new ProductNotFoundException();
            }
            else if (p.CanBeBoughtOnCredit == true)
            {
                p.CanBeBoughtOnCredit = false;
                ActiveProducts.ToList().Remove(p);
                return;
            }

            throw new ProductAlreadyDeactivatedException(p.Name + " is already deactive.");
        }
    }
}
