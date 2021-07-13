using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using Transaction = BankingApplication.Models.Transactions.Transaction;

namespace BankingApplication
{
    /// <summary>
    /// Static class containing a number of utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Static method to display all balances for accounts associated with <paramref name="user"/> as well as total balance
        /// </summary>
        /// <param name="user">User object</param>
        public static void DisplayBalanceBanner(User user)
        {
            Decimal totalBalance = 0;
            if (user.Accounts.Count == 0)
            {
                Console.WriteLine("Please select 'Manage Accounts' and create an account with us before proceeding with any other functions.");
            }
            
            foreach (string accountNumber in user.Accounts)
            {
                Account account = Database.GetAccountByAccountNumber(accountNumber);
                totalBalance += account.Balance;
                Console.WriteLine(account.ToString());
            }

            if (user.Accounts.Count > 0)
            {
                Console.WriteLine($"\nTotal Balance: Dr{totalBalance:0.00}");
            }
        }
        
        private static void _throwMenuExceptionsIfAny(string input)
        {
            if ((input != null ? input : "").Equals("#"))
            {
                throw new ToTopMenu();
            }
            if ((input != null ? input : "").Equals("*"))
            {
                throw new ToPreviousMenu();
            }
        }
        
        /// <summary>
        /// Static method to read input from console
        /// </summary>
        /// <returns>string input</returns>
        /// <exception cref="ToTopMenu">Exception thrown if user enters "#"</exception>
        /// <exception cref="ToPreviousMenu">Exception thrown if user enters "*"</exception>
        public static string ReadLine()
        {
            string input = Console.ReadLine();
            _throwMenuExceptionsIfAny(input);
            return input;
        }

        private static string _readLineNotEmpty()
        {
            string line = ReadLine();
            if (line.Equals(String.Empty))
            {
                throw new EmptyInputException();
            }
            return line;
        }

        /// <summary>
        /// Static method to read input from console, ensuring that the read input is not an empty string
        /// </summary>
        /// <param name="prompt">string message to prompt</param>
        /// <returns>string input</returns>
        /// <exception cref="ToTopMenu">Exception thrown if user enters "#"</exception>
        /// <exception cref="ToPreviousMenu">Exception thrown if user enters "*"</exception>
        public static string ReadField(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}");
                try
                {
                    return _readLineNotEmpty();
                }
                catch (EmptyInputException){}
            }
        }

        /// <summary>
        /// Static method to read a currency value from console, ensuring the value has at most two decimal places
        /// </summary>
        /// <param name="prompt">string message to prompt</param>
        /// <returns>decimal currency value</returns>
        /// <exception cref="ToTopMenu">Exception thrown if user enters "#"</exception>
        /// <exception cref="ToPreviousMenu">Exception thrown if user enters "*"</exception>
        public static decimal ReadCurrencyValue(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write($"\n{prompt}");
                    decimal value = Convert.ToDecimal(ReadLine());
                    if (!(value > 0))
                    {
                        Console.WriteLine("invalid input, value must be greater than zero");
                        continue;
                    }
                    if (decimal.Round(value, 2) == value)
                    {
                        return value;
                    }
                    Console.WriteLine("invalid input, currency value should have at most two places of decimal");
                }
                catch (Exception e)
                {
                    if (e is not (OverflowException or FormatException))
                    {
                        throw;
                    }
                    Console.WriteLine("invalid input");
                }
            }
        }
        
        /// <summary>
        /// Static method to read sensitive input from console, with each input character masked
        /// </summary>
        /// <param name="mask">character to mask input with, '*' by default</param>
        /// <returns>input string</returns>
        public static string ReadLineMasked(char mask='*')
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write(mask);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);

                    if (Console.CursorLeft == 0)
                    {
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                    }
                    else Console.Write("\b \b");
                }
            }
            Console.WriteLine();
            _throwMenuExceptionsIfAny(sb.ToString());
            return sb.ToString();
        }
        
        /// <summary>
        /// Static method for prompts to set passcode, ensures that user confirms the passcode they wish to set. Also
        /// ensures passcode is four digits.
        /// </summary>
        /// <returns>string passcode</returns>
        /// <exception cref="PassCodeException">Exception thrown when confirm passcode does not match or passcode is invalid</exception>
        public static string SetPasscode()
        {
            Console.Write("Set your four digit passcode: ");
            string code = ReadLineMasked();
            if (code.Length != 4)
            {
                Console.WriteLine("code should be 4 digits only");
                throw new PassCodeException();
            }

            if (!int.TryParse(code, out _))
            {
                Console.WriteLine("code should be numeric");
                throw new PassCodeException();
            }
            
            Console.Write("Confirm your passcode: ");
            string confirmCode = ReadLineMasked();
            if (!confirmCode.Equals(code))
            {
                throw new PassCodeException();
            }

            return code;
        }

        /// <summary>
        /// Static method to split pascal case string and join each word delimited with a space
        /// </summary>
        /// <param name="str">input pascal case string</param>
        /// <returns>space delimited string</returns>
        public static string SplitPascalCase(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }

        /// <summary>
        /// Static method to display the transaction history for an account
        /// </summary>
        /// <param name="account">Account instance</param>
        public static void DisplayTransactionHistory(Account account)
        {
            List<Transaction> transactions = Database.GetAccountTransactionHistory(account.AccountNumber);
            if (transactions.Count == 0)
            {
                Console.WriteLine("\nNo transactions have occurred on this account yet.");
            }
            else
            {
                Console.WriteLine("History\n---------");
            }
            foreach (var transaction in transactions)
            {
                if (transaction.Status) Console.WriteLine($"\n{transaction}");
                else Console.WriteLine("Failed");
            }
            Console.WriteLine("---------");
        }
    }
}