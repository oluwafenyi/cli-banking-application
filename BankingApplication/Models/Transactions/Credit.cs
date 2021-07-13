using System;
using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    public class Credit: Transaction
    {
        public Credit(decimal amount, string desc, Account account) : base("Credit", amount, desc, account)
        {
            Status = _credit(amount);
            Balance = account.Balance;
        }

        private bool _credit(decimal amount)
        {
            try
            {
                Account.Credit(amount);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}