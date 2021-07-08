
using System;

namespace ModelAndDal
{
    public class Product
    {
        public const string _productFileName = "products.csv";
        public const string _userFileName = "users.csv";
        private string _name = "";
        private decimal _price = 0;

        public int Id { get; set; }

        public string Name 
        { 
            get => _name; 
            private set
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value == string.Empty || value.Trim() == string.Empty)
                    throw new ArgumentOutOfRangeException();
                _name = value;
            }

        }

        public decimal Price 
        { get => _price; 
            private set 
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                _price = value;
            } 
        }

        public bool IsActive { get; set; } = true;
        public bool CanBeBoughtOnCredit { get; set; } = false;

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
            string stringId = new Guid().ToString("N");
            Id = int.Parse(stringId);
        }

        public Product(string name, decimal price, bool availableOnCredit) : this(name, price)
        {
            CanBeBoughtOnCredit = availableOnCredit;
        }

        public Product(string name, decimal price, bool availableOnCredit, bool isActive) : this(name, price, availableOnCredit)
        { 
            IsActive = isActive;
        }

        public Product(int id, string name, decimal price, bool availableOnCredit, bool isActive)
            : this(name, price, availableOnCredit, isActive)
        {
            Id = id;
        }


        public override string ToString()
        {
            return $" {Id.ToString()}   {Name}   {Price.ToString()}";
        }
    }
}
