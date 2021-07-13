using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using NUnit.Framework;


namespace BankingApplication.Test.Accounts
{
    public class AccountsTests
    {
        private static User _user;
        private static Account _account;
        
        [SetUp]
        public void SetUp()
        {
            _user = new User("John", "Smith", "test@user.com", "1234");
            _account = new SavingsAccount(_user);
        }
        
        [Test]
        public void TestAccountCredit()
        {
            _account.Credit(100m);
            Assert.AreEqual(_account.Balance, 100m);
        }

        [Test]
        public void TestAccountDebit()
        {
            _account.Credit(100m);
            _account.Debit(50m);
            Assert.AreEqual(_account.Balance, 50m);
        }

        [Test]
        public void TestAccountDebitInsufficientBalance()
        {
            decimal amount = 100m;
            _account.Credit(amount);
            Assert.Throws<InsufficientBalanceException>(() => _account.Debit(amount + 1)); }

        [Test]
        public void TestAccountDebitLimitExceeded()
        {
            _account.Credit(2_000_000m);
            Assert.Throws<DebitLimitExceededException>(() => _account.Debit(_account.WithdrawalLimit + 1));
        }
    }
}