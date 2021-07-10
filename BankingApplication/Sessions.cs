using System;
using System.Linq;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;

namespace BankingApplication
{
    public class Sessions
    {
        public static void NewSession(User user)
        {
            while (true)
            {
                Console.WriteLine($"\nGood Day, {user}. Enter '#' at any point to sign out.");
                Utils.DisplayBalanceBanner(user);
                Console.WriteLine(@"
What would you like to do today?
    1. Withdrawal
    2. Deposit
    3. Transfer
    4. See Transaction History
    5. Manage Accounts");

                int mode = Prompts.GetSelection();
                Account account = null;
                
                if (new int[] {1, 2, 3, 4}.Contains(mode))
                {
                    try
                    {
                        account = Prompts.AccountSelection(user);
                    }
                    catch (ToPreviousMenu)
                    {
                        continue;
                    }
                }
                
                try
                {
                    switch (mode)
                    {
                        case 1:
                            Prompts.Withdrawal(account);
                            break;
                        case 2:
                            Prompts.Deposit(account);
                            break;
                        case 3:
                            TransferSession(account);
                            break;
                        case 4:
                            Utils.DisplayTransactionHistory(account);
                            Prompts.GetSelection();
                            break;
                        case 5:
                            AccountManagementSession(user);
                            break;
                    }
                }
                catch (ToPreviousMenu) {}
            }
        }
        
        static void AccountManagementSession(User user)
        {
            bool complete = false;
            while (!complete)
            {
                Console.WriteLine("\nAt any time, enter '*' to navigate to the previous menu");
                Console.WriteLine(@"Account Management Options:
    1. Create New Account
    2. Close Account");

                int mode = Prompts.GetSelection();

                try
                {
                    if (mode == 1)
                    {
                        Console.WriteLine("\nAt any time, enter '*' to navigate to the previous menu");
                        Account newAccount = Prompts.CreateBankAccount(user);
                        Console.WriteLine($"\nThank you for banking with us {newAccount.Owner.FirstName}, your assigned account number is {newAccount.AccountNumber}");
                        complete = true;
                    }
                    else if (mode == 2)
                    {
                        Prompts.CloseBankAccount(user);
                        complete = true;
                    }
                    else
                    {
                        Console.WriteLine("invalid input");
                    }
                }
                catch (ToPreviousMenu) {}
            }
        }

        static void TransferSession(Account account)
        {
            while (true)
            {
                Console.WriteLine("\nAt any time, enter '*' to navigate to the previous menu");
                Console.WriteLine(@"Transfer Menu:
    1. Transfer to Iron Bank of Braavos
    2. Transfer to Other Banks");
                
                int selection = Prompts.GetSelection();
                if (selection == 1)
                {
                    Prompts.Transfer(account);
                    break;
                }
                if (selection == 2)
                {
                    Prompts.TransferToOtherBank(account);
                    break;
                }
                Console.WriteLine("\ninvalid input");
            }
        }
    }
}