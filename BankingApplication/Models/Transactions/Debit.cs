using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    /// <summary>
    /// Class for encapsulating debit transaction data
    /// </summary>
    public class Debit: Transaction
    {
        /// <summary>
        /// Upon instantiation of this account the <see cref="Account.Debit"/> method is called with the <paramref name="amount"/>
        /// passed. <see cref="Transaction.Status"/> is then set.
        /// </summary>
        /// <param name="amount">transaction amount, currency value</param>
        /// <param name="desc">transaction description</param>
        /// <param name="account">account the transaction is to be carried out on</param>
        public Debit(decimal amount, string desc, Account account) : base("Debit", amount, desc, account)
        {
            Status = _debit(amount);
            Balance = account.Balance;
        }

        private bool _debit(decimal amount)
        {
            try
            {
                Account.Debit(amount);
                return true;
            }
            catch (InsufficientBalanceException)
            {
                return false;
            }
        }
    }
}