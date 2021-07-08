using Controller;
using ModelAndDal;
using ModelAndDal.Model.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI
{
    public class UI : IStregSystemUI
    {
        private StregSystemKerne _system;
        CommandParser _parser = null;
        private bool _activeUI;

        //Subsribed to by parser
        public event StregSystemEvent CommandEntered;

        public void Start()
        {
            _parser = new CommandParser(_system, this);
            _activeUI = true;

            if (_system == null)
            {
                throw new NullReferenceException();
            }

            // Subscribe to DisplayUserWarning (user, decimal, returns void)
            // When UserBalanceWarning gets invoked (in User) also invoke displayBalanceWarning
            _system.UserBalanceWarning += DisplayUserBalanceWarning;

            Run();
        }

        private void Run()
        {
            do
            {
                DisplayAvailableProduct();
                Console.WriteLine();
                Console.WriteLine();
                Console.Write(">>> ");
                string command = Console.ReadLine();
                // If CommandEntered is not null, invoke it with the inputted string as actual parameter.
                Console.WriteLine();
                Console.WriteLine();
                CommandEntered?.Invoke(command);

            } while (_activeUI);
        }

        private void DisplayUserBalanceWarning(User user, decimal balance)
        {
            Console.WriteLine($"{user.Username} has a low balance: {balance}.");
        }

        public UI(StregSystemKerne system)
        {
            this._system = system;
        }

        public void Close()
        {
            _activeUI = false;
            Console.WriteLine("Shutting down UI");
            //Timeout
            Console.ReadKey();
            Console.Clear();
            Environment.Exit(0);
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine("No such Admin Command");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine("General Error  - woops");
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"Inssufficient money,{user.Username} poor boy!!");
        }
        public void DisplayInsufficientCash(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayAdminCommandNotFoundMessageMultiBuy(User user, uint numberOfProducts, Product product)
        {
            Console.WriteLine($"{user.Username} have bought {numberOfProducts} {product}.");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Product {product} not found!");
        }

        public void DisplayProductNotFound()
        {
            Console.WriteLine($"Product not found!");
        }
        public void DisplayProductNotActive(string product)
        {
            Console.WriteLine($"Product {product}");
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine("Too many arguments");
        }
        public void DisplayTooFewArgumentsError(string command)
        {
            Console.WriteLine("Too few arguments");
        }
        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine("You have bought " + transaction.Product.Name);
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine("You have bought " + count + transaction.Product.Name);
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user.ToString());
        }

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"Could not find user {username}.\n" +
                $"If you are experiencing mild distraught because your friend isn't in F-klub yet, seek immediately help");
        }

        private void DisplayAvailableProduct()
        {
            Console.WriteLine("\nid  name  price  active  deactivate_date \n");
            foreach (Product item in _system.ActiveProducts)
            {
                DisplayProduct(item);
            }
        }
        private void DisplayProduct(Product p) => Console.WriteLine(p.ToString());

        public void DisplayTransaction(ITransaction trans) => Console.WriteLine(trans.ToString());

        public void DisplayProductAlreadyActivatedException(string message)
        {
            Console.WriteLine($"{message}");
        }

        public void DisplayProductAlreadyDeactivatedException(Product p)
        {
            Console.WriteLine($" {p} is already deactivated");
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void DisplayNoSuchCommand()
        {
            Console.WriteLine("No such command.");
        }

        public void DisplaySuccess(string additionalMessage)
        {
            Console.WriteLine("Successful command!");
        }

        public void DisplayAddCredits(int credits, User user)
        {
            Console.WriteLine($"Added {credits} credits to {user.Username}. User now has {user.Balance.ToString()}");
        }

        internal void DisplaySuccessfulOnCreditChange(string message)
        {
            Console.WriteLine("Succesfully changed OnCredit availability for the product!" + message);
        }
        internal void DisplaySuccessfulOnCreditChange()
        {
            Console.WriteLine("Succesfully changed OnCredit availability for the product!");
        }

        internal void DisplaySuccessActivation()
        {
            Console.WriteLine("Successfully activated/deactivated the product! Now buy some more stuff!");
        }
    }
}
