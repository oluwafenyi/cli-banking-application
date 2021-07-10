using System;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;

namespace BankingApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    int mode = StartupModeSelection();
                    if (mode == 1)
                    {
                        User user = UserSignIn();
                        Sessions.NewSession(user);
                    }
                    else if (mode == 2)
                    {
                        Account newAccount = CreateNewAccount();
                        Console.WriteLine($"\nThank you for banking with us {newAccount.Owner.FirstName}, your assigned account number is {newAccount.AccountNumber}");
                    }
                }
                catch (ToTopMenu){}
                catch (ToPreviousMenu){}
            }
        }
        
        static int StartupModeSelection()
        {
            while (true)
            {
                Console.WriteLine(@"
Welcome to The Iron Bank of Braavos, how may we help you today?
    1. Sign in
    2. Create an impregnable account");
                int mode = Prompts.GetSelection();
                
                if (mode == 1 || mode == 2)
                {
                    return mode;
                }
                Console.WriteLine("\nInvalid input. Please try again");
            }
        }

        static Account CreateNewAccount()
        {
            Console.WriteLine("\nThank you for banking with us, please follow the prompts to create your account. At any time, enter '#' to return to the top menu. Or '*' to navigate to the previous menu.");

            User newUser = Prompts.RegisterNewUser();
            return Prompts.CreateBankAccount(newUser);
        }
        
        static User UserSignIn()
        {
            Console.WriteLine("\nThank you for banking with us, please follow the prompts to sign into your account. Enter '#' at anytime to return to the top menu. Or '*' to navigate to the previous menu.");
            bool valid = false;
            User user = null;
            while (!valid)
            {
                string email = Utils.ReadField("\nEmail: ");
                Console.Write("Passcode: ");
                string passcode = Utils.ReadLineMasked();

                user = Database.GetUserByEmail(email);

                if (!(user != null && user.CheckPasscode(passcode)))
                {
                    Console.WriteLine("Email or passcode incorrect");
                }
                else
                {
                    valid = true;
                }
            }
            return user;
        }
    }
}