using System;
using System.Collections.Generic;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using BankingApplication.Models.Transactions;

namespace BankingApplication
{
    public static class Prompts
    {
        public static int GetSelection()
        {
            int mode = 0;
            try
            {
                Console.Write(": ");
                mode = Convert.ToInt32(Utils.ReadLine());
            }
            catch (Exception e)
            {
                if (e is not (OverflowException or FormatException)) throw;
            }
        
            return mode;
        }
        
        public static User RegisterNewUser()
        {
            string firstName;
            string lastName;
            string emailAddress;
            string passCode;

            while (true)
            {
                firstName = Utils.ReadField("\nEnter your first name: ");
                lastName = Utils.ReadField("Enter your last name: ");
                emailAddress = Utils.ReadField("Enter your email address: ");

                User existingUser = Database.GetUserByEmail(emailAddress);
                if (existingUser != null)
                {
                    Console.WriteLine($"\nUser with address: {emailAddress} already exists. Please log in.");
                    throw new ToTopMenu();
                }
                
                while (true)
                {
                    try
                    {
                        passCode = Utils.SetPasscode();
                        break;
                    }
                    catch (PassCodeException)
                    {
                    }
                }

                Console.WriteLine(@$"
Here is what we have:
    Firstname: {firstName}
    LastName: {lastName}
    Email Address: {emailAddress}
    Passcode: ****");
                Console.Write("Confirm input (Y/n)? ");
                string input = Utils.ReadLine();
                if (input == null || input.Equals(String.Empty) || input.ToLower().Equals("y"))
                {
                    break;
                }
            }

            User user = new User(firstName, lastName, emailAddress, passCode);
            Database.InsertUser(user);
            return user;
        }
        
        public static Account CreateBankAccount(User user)
        {
            bool confirmed;
            int accountType = 0;
            Account account = null;
            do
            {
                Console.WriteLine("\nWhat kind of account will you like to create?");
                for (int i = 0; i < Database.AccountTypes.Count; i++)
                {
                    Console.WriteLine($"\t{i+1}. {Utils.SplitPascalCase(Database.AccountTypes[i].Name)}");
                }
                try
                {
                    Console.Write(": ");
                    accountType = Convert.ToInt32(Utils.ReadLine());
                }
                catch (Exception e)
                {
                    if (e is not (FormatException or OverflowException)) throw;
                }
                try
                {
                    account = (Account) Activator.CreateInstance(Database.AccountTypes[accountType - 1], user);
                    confirmed = true;
                }
                catch (ArgumentOutOfRangeException)
                {   Console.WriteLine("invalid input");
                    confirmed = false;
                }
            } while (!confirmed);

            if (account != null)
            {
                user.Accounts.Add(account.AccountNumber);
            }
            Database.InsertAccount(account);
            return account;
        }

        public static void CloseBankAccount(User user)
        {
            bool complete = false;
            while (!complete)
            {
                List<string> accountList = new List<string>(user.Accounts);
                Console.WriteLine("\nAt any time, enter '*' to navigate to the previous menu");
                Console.WriteLine("It is advised to empty your account before closure. Closure forfeits existing account balance.");

                Console.WriteLine("\nSelect account to close");
                for (int i = 0; i < accountList.Count; i++)
                {
                    Console.WriteLine($"\t{i+1}. {Database.GetAccountByAccountNumber(accountList[i])}");
                }

                try
                {
                    string accountNumber = accountList[GetSelection()-1];
                    Account account = Database.GetAccountByAccountNumber(accountNumber);
                    Console.Write($"\nYou have selected {account}, confirm (Y/n)? ");
                    string input = Utils.ReadLine();
                    if (input == null || input.Equals(String.Empty) || input.ToLower().Equals("y"))
                    {
                        Console.Write("\nEnter your passcode to confirm deletion: ");
                        string passcode = Utils.ReadLineMasked();
                        if (user.CheckPasscode(passcode))
                        {
                            Database.DeleteAccount(user, account);
                            Console.WriteLine($"\n{account} closed.");
                            complete = true;
                        }
                        else
                        {
                            Console.WriteLine("invalid passcode");
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("\ninvalid input");
                }
            }
        }

        public static Account AccountSelection(User user)
        {
            List<string> accountList = new List<string>(user.Accounts);
            while (true)
            {
                Console.WriteLine("\nAt any time, enter '*' to navigate to the previous menu");
                Console.WriteLine("Select account:");
                for (int i = 0; i < accountList.Count; i++)
                {
                    Console.WriteLine($"\t{i+1}. {Database.GetAccountByAccountNumber(accountList[i])}");
                }

                try
                {
                    string accountNumber = accountList[GetSelection()-1];
                    Account account = Database.GetAccountByAccountNumber(accountNumber);
                    return account;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("\ninvalid input");
                }
            }
        }

        public static void Withdrawal(Account account)
        {
            decimal amount;
            while (true)
            {
                try
                {
                    amount = Utils.ReadCurrencyValue("Enter amount to withdraw: ");
                    break;
                }
                catch (Exception e)
                {
                    if (e is FormatException or OverflowException)
                    {
                        Console.WriteLine("invalid input");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            Transaction transaction = new Debit(amount, "Withdrawal", account);
            if (!transaction.Status)
            {
                Console.WriteLine("\nWithdrawal failed, amount greater than account balance.");
            }
            else
            {
                Console.WriteLine($"Withdrawal successful, new account balance is Dr{account.Balance:0.00}");
            }
            Database.InsertTransactionHistoryRecord(account.AccountNumber, transaction);
        }
        
        public static void Deposit(Account account)
        {
            decimal amount;
            while (true)
            {
                try
                {
                    amount = Utils.ReadCurrencyValue("Enter amount to deposit: ");
                    break;
                }
                catch (Exception e)
                {
                    if (e is FormatException or OverflowException)
                    {
                        Console.WriteLine("invalid input");
                    }
                    else
                    {
                        throw;
                    }

                }
            }
            Transaction transaction = new Credit(amount, "Deposit", account);
            Database.InsertTransactionHistoryRecord(account.AccountNumber, transaction);
            Console.WriteLine($"Deposit successful, new account balance is {account.Balance:0.00}");
        }

        public static void Transfer(Account account)
        {
            decimal amount = Utils.ReadCurrencyValue("Enter amount to transfer: ");
            while (true)
            {
                Console.Write("Enter account number to transfer to: ");
                string accountNumber = Utils.ReadLine();
                Account recipient = Database.GetAccountByAccountNumber(accountNumber);
                if (recipient == null)
                {
                    Console.WriteLine("\nInvalid account number");
                    continue;
                }
                
                Console.WriteLine(@$"Impending transfer details:
    Amount: {amount}
    Recipient: {recipient.Owner}
    Recipient Acc No.: {accountNumber}");

                string passcode;
                while (true)
                {
                    Console.Write("Enter passcode to confirm transfer: ");
                    passcode = Utils.ReadLineMasked();

                    if (account.Owner.CheckPasscode(passcode))
                    {
                        break;
                    }
                    Console.WriteLine("invalid passcode");
                }
                
                Transaction debit = new Debit(amount, $"Transfer to {recipient.Owner}/{recipient.AccountNumber}", account);
                if (debit.Status)
                {
                    Transaction credit = new Credit(amount, $"Transfer from {account.Owner}/{account.AccountNumber}", recipient);
                    Database.InsertTransactionHistoryRecord(accountNumber, debit);
                    Database.InsertTransactionHistoryRecord(recipient.AccountNumber, credit);
                    Console.WriteLine("Transfer successful");
                }
                else
                {
                    Console.WriteLine("You have insufficient funds to complete this transaction");
                }
                break;
            }
        }

        public static void TransferToOtherBank(Account account)
        {
            decimal amount = Utils.ReadCurrencyValue("Enter amount to transfer: ");
            while (true)
            {
                Console.WriteLine("Select Recipient Bank:");
                for(int i = 0; i < Database.Banks.Count; i++)
                {
                    Console.WriteLine($"\t{i+1}. {Database.Banks[i]}");
                }
                int selected = GetSelection();

                if (selected > Database.Banks.Count || selected <= 0)
                {
                    Console.WriteLine("invalid selection");
                    continue;
                }

                string selectedBank = Database.Banks[selected - 1];
                Console.Write("Enter account number to transfer to: ");
                string accountNumber = Utils.ReadLine();
                
                Console.WriteLine(@$"Impending transfer details:
    Amount: {amount}
    Recipient Bank: {selectedBank}
    Recipient Acc No.: {accountNumber}");

                string passcode;
                while (true)
                {
                    Console.Write("Enter passcode to confirm transfer: ");
                    passcode = Utils.ReadLineMasked();

                    if (account.Owner.CheckPasscode(passcode))
                    {
                        break;
                    }
                    Console.WriteLine("invalid passcode");
                }
                
                Transaction debit = new Debit(amount, $"Transfer to {selectedBank}/{accountNumber}", account);
                if (debit.Status)
                {
                    Database.InsertTransactionHistoryRecord(accountNumber, debit);
                    Console.WriteLine("Transfer successful");
                }
                else
                {
                    Console.WriteLine("You have insufficient funds to complete this transaction");
                }
                break;
            }
        }
    }
}