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
            Account.Credit(amount);
            return true;
        }
    }
}