
using ModelAndDal;
using System;
using System.Linq;
using System.Collections.Generic;
using Controller;

namespace UI
{
    public class CommandParser
    {
        private readonly char _AdminCommandChar = ':';
        private const int _minimumNumberOfCommands = 1;
        private const int _maximumNumberOfCommands = 3;

        private readonly IStregsystem _stregSystem;
        private readonly UI _userInterface;


     
        public CommandParser(StregSystemKerne system, UI ui)
        {
            _stregSystem = system;
            _userInterface = ui;
            _userInterface.CommandEntered += ParseString;

            // private Dictionary<string, Delegate> _commandToDelegate = new Dictionary<string, Delegate>();
            // Action<int, User> refToMethod = UserInterface.DisplayAddCredits;
            // _commandToDelegate.Add("CommandWord", refToMethod);
        }

        /// <summary>
        /// React on the inputted string - there is a admin char symboling the wish to perform a admin command
        /// </summary>
        /// <param name="commandString"></param>
        private void ParseString(string commandString)
        {
            User user = null;
            Product product = null;

            //Split the string on ' ' and remove all empty elements
            string[] args = commandString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //Too few arguments or too many arguments.
            if (args.Length < _minimumNumberOfCommands)
            {
                _userInterface.DisplayTooFewArgumentsError(commandString);
                return;
            }
            else if (args.Length > _maximumNumberOfCommands)
            {
                _userInterface.DisplayTooManyArgumentsError(commandString);
                return;
            }

            string userName = args[0];

            try
            {
                //If first char of the commands == _AdminCommandChar
                if (args[0][0] == _AdminCommandChar)
                {
                    //Admin commands
                    ParseAdminCommand(args);
                    return;
                }
                else if (args.Length == 1)
                {
                    //Commands starting with usernames
                    ReactOnUsernameInput(userName, user);
                    return;
                }

                //If two numbers are inputted, try multibuy else if only one number try finding product!
                if (args.Length == 3 && uint.TryParse(args[1], out uint numberOfProducts) && int.TryParse(args[2], out int productId))
                {
                    MultiBuy(out user, out product, userName, numberOfProducts, productId);
                }
                else if (int.TryParse(args[1], out productId))
                {
                    BuyProduct(out user, out product, userName, productId);
                }
            }
            catch (UserNotFoundException e)
            {
                _userInterface.DisplayUserNotFound(e.Message);
            }
            catch (InsufficientCreditsException e)
            {
                _userInterface.DisplayInsufficientCash(user, product);
                _userInterface.DisplayInsufficientCash(e.Message);
            }
            catch (ProductNotFoundException)
            {
                _userInterface.DisplayProductNotFound();
            }
            catch (ProductNotActiveException e)
            {
                _userInterface.DisplayProductNotActive(e.Message);
            }
            catch (ProductAlreadyActivatedException e)
            {
                _userInterface.DisplayProductAlreadyActivatedException(e.Message);
            }
            catch (ProductAlreadyDeactivatedException e)
            {
               _userInterface.DisplayProductAlreadyActivatedException(e.Message);
            }

        }

        /// <summary>
        /// Buy a product using the stregsystem.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="product"></param>
        /// <param name="userName"></param>
        /// <param name="productId"></param>
        /// 
        //Pointers! :)
        private void BuyProduct(out User user, out Product product, string userName, int productId)
        {
            user = _stregSystem.GetUserByUsername(userName);
            product = _stregSystem.GetProductByID(productId);
            BuyTransaction transaction = _stregSystem.BuyProduct(user, product);
            _userInterface.DisplayUserBuysProduct(transaction);
        }

        /// <summary>
        /// Perform a multibuy -- Functionality not in stregsystem. Stregsystem only returns a IEnumerable<BuyTransactions>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="product"></param>
        /// <param name="userName"></param>
        /// <param name="numberOfProducts"></param>
        /// <param name="productId"></param>
        private void MultiBuy(out User user, out Product product, string userName, uint numberOfProducts, int productId)
        {
            user = _stregSystem.GetUserByUsername(userName);
            product = _stregSystem.GetProductByID(productId);

            int numberOfBoughtItems = _stregSystem.MultiBuy(numberOfProducts, product, user);
            IEnumerable<ITransaction> transaction = _stregSystem.GetTransactions(user, numberOfBoughtItems);

            foreach (ITransaction item in transaction)
            {
                _userInterface.DisplayTransaction(item);
            }
        
        }

        /// <summary>
        /// Display the user and the last 10 products they have bought.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool ReactOnUsernameInput(string userName, User user)
        {
            user = _stregSystem.GetUserByUsername(userName);
            _userInterface.DisplayUserInfo(user);

            IOrderedEnumerable<ITransaction> transactions = _stregSystem.GetTransactions(user, 10)
                .OrderByDescending(transaction => transaction.TransactionDate);

            foreach (ITransaction transaction in transactions)
            {
                if (transaction != null)
                    _userInterface.DisplayTransaction(transaction);
            }

            return true;
        }

        /// <summary>
        /// Parse for admin commands.
        /// </summary>
        /// <param name="args"></param>
        private void ParseAdminCommand(string[] args)
        {
            string commandWord = args[0].Substring(1);
            string secondCommand = string.Empty;
            string thirdCommand = string.Empty;

            if (args.Length == 2)
            {
                secondCommand = args[1];

            } 
            if (args.Length > 2)
            {
                secondCommand = args[1];
                thirdCommand = args[2];
            }

            //React on commandword
            switch (commandWord)
            {
                case "q":
                case "quit":
                    _userInterface.Close();
                    break;
                case "activate":
                    if (int.TryParse(secondCommand, out int result))
                    {
                        Product product = _stregSystem.GetProductByID(result);
                        _stregSystem.ActivateProduct(product);
                        _userInterface.DisplaySuccessActivation();
                    }
                    break;
                case "deactivate":
                    if (int.TryParse(secondCommand, out result))
                    {
                        Product product = _stregSystem.GetProductByID(result);
                        _stregSystem.DeactivateProduct(product);
                        _userInterface.DisplaySuccessActivation();
                    }
                    break;
                case "crediton":
                    if (int.TryParse(secondCommand, out result))
                    {
                        Product product = _stregSystem.GetProductByID(result);
                        _stregSystem.ActivateOnCredit(product);
                        _userInterface.DisplaySuccessfulOnCreditChange();
                    }
                    break;
                case "creditoff":
                    if (int.TryParse(secondCommand, out result))
                    {
                        Product product = _stregSystem.GetProductByID(result);
                        _stregSystem.ActivateOnCredit(product);
                        _userInterface.DisplaySuccessfulOnCreditChange();
                    }
                    break;
                case "addcredits":
                    if (int.TryParse(thirdCommand, out int amount)
                        && thirdCommand != null && thirdCommand != string.Empty)
                    {
                        User user = _stregSystem.GetUserByUsername(secondCommand);
                        _stregSystem.AddCreditsToAccount(user, amount);
                        _userInterface.DisplayAddCredits(amount, user);
                    }
                    break;
                default:
                    _userInterface.DisplayNoSuchCommand();
                    break;
            }
        }
    }
}