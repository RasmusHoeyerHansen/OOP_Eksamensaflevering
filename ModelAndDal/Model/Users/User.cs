using ModelAndDal.Model.Users;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ModelAndDal
{
    public class User : IEquatable<User>, IComparable<User>
    {
        public event UserBalanceNotification UserBalanceWarning;
        //Static int increasing with each user constructed. Incremented in base constructor;
        private static int _IdCounter = _IdCounter++;
        private string _username = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _email = "";
        private const decimal _balanceWarningAmount = 50;

        private string FullName { get => FirstName + " " + LastName; }
        public int Id { get; }

        public string FirstName
        {
            get => _firstName;
            private set
            {
                if (InvalidName(value))
                {   
                    throw new ArgumentException("Invalid name.", nameof(LastName));
                }
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            private set
            {
                if (InvalidName(value))
                {
                    throw new ArgumentException("Invalid name", nameof(LastName));
                }
                _lastName = value;
            }
        }

        public string Username
        {
            get => _username;
            private set 
            {
                if (InvalidUserName(value))
                {
                    throw new ArgumentException("Invalid username", nameof(value));
                }
                _username = value;
            }
        }

        public string Email
        {
            get => _email;

            private set
            {
                if (InvalidEmailSyntax(value))
                {
                    throw new ArgumentException("Invalid email", nameof(value));
                }
                _email = value;
            }
        }

        public decimal Balance { get; private set; }

        public User(string firstName, string lastName, string userName, string email)
        { 
            FirstName = firstName;
            LastName = lastName;
            Username = userName;
            Email = email;
            Id = ++_IdCounter;
        }

        public User(string firstName, string lastName, string userName, string email, decimal balance)
            : this(firstName, lastName, userName, email)
        {
            Balance = balance;
        }

        public User(int id, string firstName, string lastName, string userName, string email, decimal balance)
                    : this(firstName, lastName, userName, email, balance)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $" [{FullName}], {Email},  balance: {Balance.ToString()}";
        }

        /// <summary>
        /// 'Det er brugernavnet alene, der identificerer brugeren i stregsystemet!' 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(User other)
        {
            //"Det er brugernavnet alene, der identificerer brugeren i stregsystemet!"
            return (this.Username == other.Username);
        }

        /// <summary>
        /// Compares on username. 'Det er brugernavnet alene, der identificerer brugeren i stregsystemet!' 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(User other)
        {    
            return Username.CompareTo(other.Username);
        }

        /// <summary>
        /// Get hash code based on some prime numbers and the hashcode of FullName
        /// </summary>
        /// <returns>@hash</returns>
        public override int GetHashCode()
        {
            //Stackoverflow said a good hash code used prime numbers to avoid collission as well as strings.
            int hash = 17;
            if (FullName != null)
                hash = hash * 23 + FullName.GetHashCode();

            if (Email != null)
                hash = hash * 23 + Email.GetHashCode();

            hash = hash * 23 + Id.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Adds amount to the user's balance. Amount must be >= 0.
        /// </summary>
        /// <param name="amount"></param>
        public void AddToBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Balance += amount;
        }

        /// <summary>
        /// Removes amount from balance - amount must be larger or equal to 0.
        /// </summary>
        /// <param name="amount"></param>
        public void RemoveFromBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (Balance - amount < _balanceWarningAmount)
            {
                //Otherwise the delegate is invoked before balance is withdrawn!
                UserBalanceWarning?.Invoke(this, Balance-amount);
            }
            Balance -= amount;
        }

        /// <summary>
        /// Checks for null or empty.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Bool representing if stringh is valid or not</returns>
        private bool InvalidName(string name)
        {
            return string.IsNullOrEmpty(name);
        }

        /// <summary>
        /// Checks for null or empty as well as æøå@#
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Bool representing whether input is valid or not</returns>
        // TODO: Add more special character checks
        private bool InvalidUserName(string userName)
        {

            if (string.IsNullOrEmpty(userName))
                return true;

            userName = userName.ToLower();

            return userName.Contains(" ") || userName.Contains("æ") || userName.Contains("#")
                || userName.Contains("ø") || userName.Contains("å") || userName.Contains("@");
        }

        // TODO: Add more special character checks
        /// <summary>
        /// Checks for æøå# and some other chars.
        /// </summary>
        /// <param name="email to check"></param>
        /// <returns></returns>
        private bool InvalidEmailSyntax(string email)
        {
            if (email == null)
                throw new ArgumentNullException("Email must not be null", nameof(email));


            email = email.ToLower();

            //Could contain more vlidation - use regex and System.Net.Mail;
            if (email.Contains(" ") || email.Contains("æ") || email.Contains("#")
                || email.Contains("ø") || email.Contains("å"))
            {
                return true;
            }

            return false;
        }
    }
}
