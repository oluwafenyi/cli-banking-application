namespace BankingApplication.Models.Accounts
{
    public class SavingsAccount: Account
    {
        private static long _count = 0;

        public SavingsAccount(User owner) : base(owner, Account.Savings, _count)
        {
            _count += 1;
        }
    }
}