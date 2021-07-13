using BankingApplication.Models;
using NUnit.Framework;

namespace BankingApplication.Test.Users
{
    public class UsersTests
    {
        [Test]
        public void TestUserPasscodeCheck()
        {
            User user = new User("John", "Smith", "test@user.com", "1234");
            Assert.IsTrue(user.CheckPasscode("1234"));
            Assert.IsFalse(user.CheckPasscode("1123"));
        }
    }
}