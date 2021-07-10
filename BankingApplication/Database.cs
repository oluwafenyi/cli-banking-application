using System;
using System.Collections.Generic;
using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using BankingApplication.Models.Transactions;

namespace BankingApplication
{
    public static class Database
    {
        public static List<Type> AccountTypes = new List<Type>()
        {
            typeof(SavingsAccount),
            typeof(CurrentAccount)
        };

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

        public static void InsertAccount(Account newAccount)
        {
            _accounts[newAccount.AccountNumber] = newAccount;
        }

        public static void InsertUser(User newUser)
        {
            _users[newUser.EmailAddress] = newUser;
        }
        
        public static User GetUserByEmail(string emailAddress)
        {
            if (_users.ContainsKey(emailAddress))
            {
                return _users[emailAddress];
            }

            return null;
        }
        
        public static Account GetAccountByAccountNumber(string accountNumber)
        {
            if (_accounts.ContainsKey(accountNumber))
            {
                return _accounts[accountNumber];
            }

            return null;
        }
        public static bool DeleteAccount(User user, Account account)
        {
            user.Accounts.Remove(account.AccountNumber);
            return _accounts.Remove(account.AccountNumber);
        }

        public static void InsertTransactionHistoryRecord(string accountNumber, Transaction transaction)
        {
            if (!_transactionHistories.ContainsKey(accountNumber)) _transactionHistories.Add(accountNumber, new List<Transaction>());
            _transactionHistories[accountNumber].Add(transaction);
        }

        public static List<Transaction> GetAccountTransactionHistory(string accountNumber)
        {
            if (_transactionHistories.ContainsKey(accountNumber)) return _transactionHistories[accountNumber];
            return new List<Transaction>();
        }
    }
}