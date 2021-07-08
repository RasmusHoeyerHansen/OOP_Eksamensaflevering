using System;
using System.IO;

namespace ModelAndDal
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(decimal amount, User user) : base(user, amount) 
        { }

        /// <summary>
        /// Adds @Amount to users balance
        /// </summary>
        public override void Execute()
        {
            User.AddToBalance(Amount);
        }
        
        public override string ToString()
        {
            return "Insertion " + base.ToString();
        }

        /// <summary>
        /// Write to temp cvs file. Located in /ModelAndDAL/bin/debug/netcoreapp3.1/Data
        /// </summary>
        public override void WriteContentToCsvFile()
        {

            var dataPath = Environment.CurrentDirectory.Replace("CLI", "ModelAndDal");
            string path = Path.Combine(dataPath, "Data", _insertTransFileName);
            
            using StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(string.Join(';', Id, User.Username, TransactionDate, Amount));
            sw.Close();
        }
    }
}
