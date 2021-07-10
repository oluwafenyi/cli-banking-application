#nullable enable
using System.Collections.Generic;

namespace BankingApplication.Models
{
    public class User
    {
        public string FirstName;
        public string LastName;
        public readonly string EmailAddress;
        private string _passCode;

        public HashSet<string> Accounts = new HashSet<string>();

        public User(string firstName, string lastName, string emailAddress, string passCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            _passCode = passCode;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public override int GetHashCode()
        {
            return EmailAddress.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(User)) return false;
            return EmailAddress.Equals(((User) obj).EmailAddress);
        }

        public bool CheckPasscode(string passCode)
        {
            return _passCode.Equals(passCode);
        }
    }
}