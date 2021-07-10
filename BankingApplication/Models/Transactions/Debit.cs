using BankingApplication.Models.Accounts;

namespace BankingApplication.Models.Transactions
{
    public class Debit: Transaction
    {
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