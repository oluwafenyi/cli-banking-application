using System;
using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    /// <summary>
    /// Abstract class providing a base transaction type
    /// </summary>
    public abstract class Transaction
    {
        private readonly string _type;
        /// <value>account the transaction is to be carried out on</value>
        protected readonly Account Account;
        private readonly decimal _amount;
        private readonly string _desc;
        private readonly DateTime _date;
        /// <value>status of transaction, true if successful else false</value>
        public bool Status;
        /// <value>account balance post-transaction</value>
        protected Decimal Balance;
        
        /// <param name="type">transaction type</param>
        /// <param name="amount">transaction amount, currency value</param>
        /// <param name="desc">transaction description</param>
        /// <param name="account">account the transaction is to be carried out on</param>
        protected Transaction(string type, decimal amount, string desc, Account account)
        {
            _type = type;
            Account = account;
            _amount = amount;
            _desc = desc;
            _date = DateTime.Now;
        }
        
        /// <returns>summary detailing the amount, account number, description, date and balance of the transaction</returns>
        public override string ToString()
        {
            return $@"{_type}
Amt: Dr{_amount:0.00}
Acc: {Account.AccountNumber}
Desc: {_desc}
Date: {_date.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"))}
Balance: Dr{Balance:0.00}";
        }
    }
}