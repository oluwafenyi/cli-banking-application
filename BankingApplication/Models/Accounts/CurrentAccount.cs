namespace BankingApplication.Models.Accounts
{
    public class CurrentAccount: Account
    {
        private static long _count = 0;

        public CurrentAccount(User owner) : base(owner, Account.Current, _count)
        {
            _count += 1;
        }
    }
}