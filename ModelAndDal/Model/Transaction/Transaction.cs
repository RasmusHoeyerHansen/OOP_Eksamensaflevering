using System;

namespace ModelAndDal
{
    /// <summary>
    /// Abstract class - represents all transactions - implements ITransaction.
    /// Contains filenames of csv transaction files
    /// </summary>
    public abstract class Transaction : ITransaction
    {   
        protected const string _insertTransFileName = "insertTransactions.csv";
        protected const string _buyTransFileName = "buyTransactions.csv";
        protected User _user = null;
        protected decimal _amount = 0;

        protected static int _id;

        public int Id { get; set; }
        public DateTime TransactionDate { get; } = DateTime.Now;
        public User User
        {
            get => _user;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Value must not be null", nameof(value));
                }
                _user = value;
            }
        }

        public decimal Amount 
        { 
            get => _amount;
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Amount must be more than 0");
                }
                _amount = value;
            } 
        }

        public Transaction(User user, decimal amount)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User must not be null", nameof(user));
            }
            Amount = amount;
            User = user;
            Id = _id++;
        }

        public override string ToString()
        {
            return "Transaction:  " + Id.ToString() + "  Amount:  " + Amount.ToString() 
                    + "  [" + User.ToString() + "]" + "\n Date: " + TransactionDate.ToString(); 
        }

        public abstract void Execute();

        public abstract void WriteContentToCsvFile();
    }
}
