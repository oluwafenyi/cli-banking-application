using System;
using System.Collections.Generic;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using BankingApplication.Models.Transactions;

namespace BankingApplication
{
    /// <summary>
    /// Static class to hold application data
    /// </summary>
    public static class Database
    {
        /// <value>A list of account types available to the user for account creation</value>
        public static List<Type> AccountTypes = new List<Type>()
        {
            typeof(SavingsAccount),
            typeof(CurrentAccount)
        };
        
        /// <value>List of third party bank names that have transfer support</value>
        public static List<string> Banks = new List<string>()
        {
            "Union Bank",
            "GTBank",
            "First Bank",
            "FCMB",
            "Sterling Bank",
            "WEMA Bank",
            "Access Bank",
            "Zenith Bank"
        };
        private static Dictionary<string, User> _users = new Dictionary<string, User>();
        private static Dictionary<string, Account> _accounts = new Dictionary<string, Account>();
        private static Dictionary<string, List<Transaction>> _transactionHistories =
            new Dictionary<string, List<Transaction>>();

        /// <summary>
        /// Method to insert an account into collection of accounts
        /// </summary>
        /// <param name="newAccount">instance of Account</param>
        public static void InsertAccount(Account newAccount)
        {
            _accounts[newAccount.AccountNumber] = newAccount;
        }

        /// <summary>
        /// Method to insert user into collection of users
        /// </summary>
        /// <param name="newUser">instance of User</param>
        public static void InsertUser(User newUser)
        {
            _users[newUser.EmailAddress] = newUser;
        }
        
        /// <summary>
        /// Method to return user data from collection by email address. If the user does not exist in the collection,
        /// null is returned.
        /// </summary>
        /// <param name="emailAddress">user email address</param>
        /// <returns>associated User object</returns>
        public static User GetUserByEmail(string emailAddress)
        {
            if (_users.ContainsKey(emailAddress))
            {
                return _users[emailAddress];
            }

            return null;
        }
        
        /// <summary>
        /// Method to return account data from collection by account number. If the account does not exist in the collection,
        /// null is returned
        /// </summary>
        /// <param name="accountNumber">account number</param>
        /// <returns>associated Account object</returns>
        public static Account GetAccountByAccountNumber(string accountNumber)
        {
            if (_accounts.ContainsKey(accountNumber))
            {
                return _accounts[accountNumber];
            }

            return null;
        }
        
        /// <summary>
        /// Method to delete account from collection and remove reference to the account from the user object
        /// </summary>
        /// <param name="user">associated user</param>
        /// <param name="account">associated account</param>
        /// <returns>true if deletion was successful else false</returns>
        public static bool DeleteAccount(User user, Account account)
        {
            user.Accounts.Remove(account.AccountNumber);
            return _accounts.Remove(account.AccountNumber);
        }

        /// <summary>
        /// Method to insert transaction record to collection
        /// </summary>
        /// <param name="accountNumber">associated account number</param>
        /// <param name="transaction">associated transaction object</param>
        public static void InsertTransactionHistoryRecord(string accountNumber, Transaction transaction)
        {
            if (!_transactionHistories.ContainsKey(accountNumber)) _transactionHistories.Add(accountNumber, new List<Transaction>());
            _transactionHistories[accountNumber].Add(transaction);
        }

        /// <summary>
        /// Method to return all transactions associated with an account
        /// </summary>
        /// <param name="accountNumber">account number</param>
        /// <returns>list of transactions associated with account</returns>
        public static List<Transaction> GetAccountTransactionHistory(string accountNumber)
        {
            if (_transactionHistories.ContainsKey(accountNumber)) return _transactionHistories[accountNumber];
            return new List<Transaction>();
        }
    }
}