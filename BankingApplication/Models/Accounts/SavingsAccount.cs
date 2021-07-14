namespace BankingApplication.Models.Accounts
{
    /// <summary>
    /// Class for representing a user's Savings Account
    /// </summary>
    public class SavingsAccount: Account
    {
        /// <value>withdrawal limit for account, default for savings account is 500,000</value>
        public decimal WithdrawalLimit = 500_000m;
        private static long _count = 0;

        /// <param name="owner">User instance that owns this account</param>
        public SavingsAccount(User owner) : base(owner, Account.Savings, _count)
        {
            _count += 1;
        }
    }
}