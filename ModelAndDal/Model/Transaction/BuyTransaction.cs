using System;
using System.IO;

namespace ModelAndDal
{
    public class BuyTransaction : Transaction
    {
        public Product Product { get; private set; }

        public BuyTransaction(decimal amountToWithdraw, User user, Product product) : base(user, amountToWithdraw)
        {
            Product = product ?? throw new ArgumentNullException("Product must not be null", nameof(product));
        }

        public override string ToString()
        {
            return "Buy " + base.ToString();
        }

        /// <summary>
        /// Execute the transaction - remove from users balance.
        /// </summary>
        public override void Execute()
        {
            User.RemoveFromBalance(Amount); 
        }

        /// <summary>
        /// Write to temp cvs file. Located in /ModelAndDAL/bin/debug/netcoreapp3.1/Data
        /// </summary>
        public override void WriteContentToCsvFile()
        {
            var dataPath = Environment.CurrentDirectory.Replace("CLI", "ModelAndDal");
            string path = Path.Combine(dataPath, "Data", _buyTransFileName);

            using StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(string.Join(';', Id, User.Username, Product.Id, TransactionDate, Amount));
            sw.Close();
        }
    }
}
    

    

