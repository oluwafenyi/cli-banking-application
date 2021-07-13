namespace BankingApplication.Models.Accounts
{
    public class SavingsAccount: Account
    {
        public decimal WithdrawalLimit = 500_000m;
        private static long _count = 0;

        public SavingsAccount(User owner) : base(owner, Account.Savings, _count)
        {
            _count += 1;
        }
    }
}