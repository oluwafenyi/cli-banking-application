using BankingApplication.Models;
using BankingApplication.Models.Accounts;
using BankingApplication.Models.Transactions;
using NUnit.Framework;

namespace BankingApplication.Test.Database
{
    public class Database
    {
        [Test]
        public void TestUserInsertionAndRetrieval()
        {
            User user = new User("John", "Smith", "test@user.com", "1234");
            Assert.IsNull(BankingApplication.Database.GetUserByEmail(user.EmailAddress));
            BankingApplication.Database.InsertUser(user);
            User retrieved = BankingApplication.Database.GetUserByEmail(user.EmailAddress);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(user.EmailAddress, retrieved.EmailAddress);
        }
        
        [Test]
        public void TestAccountInsertion()
        {
            string firstAccountNumber = "000000000000";
            Assert.IsNull(BankingApplication.Database.GetAccountByAccountNumber(firstAccountNumber));
            User user = new User("John", "Smith", "test@user.com", "1234");
            Account account = new SavingsAccount(user);
            BankingApplication.Database.InsertAccount(account);
            Assert.IsNotNull(BankingApplication.Database.GetAccountByAccountNumber(account.AccountNumber));
        }

        [Test]
        public void TestAccountDeletion()
        {
            User user = new User("John", "Smith", "test@user.com", "1234");
            Account account = new SavingsAccount(user);
            BankingApplication.Database.InsertAccount(account);
            Assert.IsNotNull(BankingApplication.Database.GetAccountByAccountNumber(account.AccountNumber));
            BankingApplication.Database.DeleteAccount(user, account);
            Assert.IsNull(BankingApplication.Database.GetAccountByAccountNumber(account.AccountNumber));
        }

        [Test]
        public void TestTransactionHistoryInsertionAndRetrieval()
        {
            User user = new User("John", "Smith", "test@user.com", "1234");
            Account account = new SavingsAccount(user);
            var prehistory = BankingApplication.Database.GetAccountTransactionHistory(account.AccountNumber);
            Assert.IsEmpty(prehistory);
            BankingApplication.Database.InsertTransactionHistoryRecord(account.AccountNumber, new Credit(5000m,"test", account));
            var history = BankingApplication.Database.GetAccountTransactionHistory(account.AccountNumber);
            Assert.IsNotEmpty(history);
        }
    }
}