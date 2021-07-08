using System;

namespace ModelAndDal
{
    public class SeasonalProduct : Product
    {
        private DateTime _startdate = DateTime.Today;
        private DateTime _endDate = DateTime.Today.AddMonths(6);

        public DateTime SeasonStartDate
        { 
            get => _startdate;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Invalid Datetime input");
                }
                _startdate = value;

            }
        } 

        public DateTime SeasonEndDate 
        { get => _endDate;
            set 
            {
                if (value == null || value < SeasonStartDate)
                {
                    throw new ArgumentException("Invalid Datetime input");
                }
            }
        }


        public SeasonalProduct(string name, decimal price, DateTime startDate, DateTime endDate) 
            : base(name, price)
        { 
            //Don't change the order of these
            SeasonStartDate = startDate;
            SeasonEndDate = endDate;
        }

        public SeasonalProduct(string name, decimal price, bool availableOnCredit, DateTime startDate, DateTime endDate)
           : base(name, price, availableOnCredit)
        {
            SeasonStartDate = startDate;
            SeasonEndDate = endDate;
        }

        public SeasonalProduct(string name, decimal price, bool availableOnCredit, bool isActive, DateTime startDate, DateTime endDate)
            : base(name, price, availableOnCredit, isActive)
        {
            SeasonStartDate = startDate;
            SeasonEndDate = endDate;
        }


    }
}
