using System;
using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    /// <summary>
    /// Class for encapsulating credit transaction data
    /// </summary>
    public class Credit: Transaction
    {
        /// <summary>
        /// Upon instantiation of this account the <see cref="Account.Credit"/> method is called with the <paramref name="amount"/>
        /// passed. <see cref="Transaction.Status"/> is then set.
        /// </summary>
        /// <param name="amount">transaction amount, currency value</param>
        /// <param name="desc">transaction description</param>
        /// <param name="account">account the transaction is to be carried out on</param>
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