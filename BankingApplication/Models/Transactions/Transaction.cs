using System;
using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    public abstract class Transaction
    {
        private readonly string _type;
        protected readonly Account Account;
        private readonly decimal _amount;
        private readonly string _desc;
        private readonly DateTime _date;
        public bool Status;
        protected Decimal Balance;
        
        protected Transaction(string type, decimal amount, string desc, Account account)
        {
            _type = type;
            Account = account;
            _amount = amount;
            _desc = desc;
            _date = DateTime.Now;
        }

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