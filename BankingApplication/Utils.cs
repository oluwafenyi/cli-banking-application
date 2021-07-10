using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using Transaction = BankingApplication.Models.Transactions.Transaction;

namespace BankingApplication
{
    public static class Utils
    {
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

        public static string SplitPascalCase(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }

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