namespace BankingApplication.Models.Accounts
{
    /// <summary>
    /// Class for representing a user's Current Account
    /// </summary>
    public class CurrentAccount: Account
    {
        private static long _count = 0;
        
        /// <param name="owner">User instance that owns this account</param>
        public CurrentAccount(User owner) : base(owner, Account.Current, _count)
        {
            _count += 1;
        }
    }
}