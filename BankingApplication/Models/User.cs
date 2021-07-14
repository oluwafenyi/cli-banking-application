#nullable enable
using System.Collections.Generic;

namespace BankingApplication.Models
{
    /// <summary>
    /// User class containing fields particular for user identification and authentication
    /// </summary>
    public class User
    {
        /// <value>user's first name</value>
        public string FirstName;
        /// <value>user's last name</value>
        public string LastName;
        /// <value>user's email</value>
        public readonly string EmailAddress;
        private string _passCode;

        /// <summary>
        /// A reference to all accounts associated with the user object, list of account numbers
        /// </summary>
        /// <value>list of user account numbers</value>
        public HashSet<string> Accounts = new HashSet<string>();
        
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="emailAddress">valid email address</param>
        /// <param name="passCode">user's 4-digit passcode</param>
        public User(string firstName, string lastName, string emailAddress, string passCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            _passCode = passCode;
        }
        
        /// <returns>user.FirstName and user.Lastname delimited by a whitespace</returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
        
        /// <returns>unique hashcode for user object</returns>
        public override int GetHashCode()
        {
            return EmailAddress.GetHashCode();
        }
        
        /// <param name="obj">object for comparison</param>
        /// <returns>true if EmailAddress matches <paramref name="obj"/>.EmailAddress else false</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(User)) return false;
            return EmailAddress.Equals(((User) obj).EmailAddress);
        }
        
        /// <param name="passCode">passcode to check</param>
        /// <returns>true if <paramref name="passCode"/> matches user passcode else false</returns>
        public bool CheckPasscode(string passCode)
        {
            return _passCode.Equals(passCode);
        }
    }
}