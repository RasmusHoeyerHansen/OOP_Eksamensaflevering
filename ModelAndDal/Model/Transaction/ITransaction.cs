using System;

namespace ModelAndDal
{
    public interface ITransaction
    {
        public int Id { get; }
        public User User { get; }
        public DateTime TransactionDate { get; }
        public decimal Amount { get; }
        public string ToString();
        public void Execute();
        public void WriteContentToCsvFile();

    }
}