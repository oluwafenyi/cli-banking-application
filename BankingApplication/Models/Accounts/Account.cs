using System;

#nullable enable

namespace BankingApplication.Models.Accounts
{
    /// <summary>
    /// Abstract class providing a base account type
    /// </summary>
    public abstract class Account
    {
        /// <value>withdrawal limit for account, default is 1,000,000</value>
        public decimal WithdrawalLimit = 1_000_000m;
        /// <value>0 represents savings account type</value>
        protected static readonly byte Savings = 0;
        /// <value>1 represents current account type</value>
        protected static readonly byte Current = 1; 
        /// <value>unique account number</value>
        public readonly string AccountNumber;
        /// <value><see cref="User"/> instance that owns this account</value>
        public readonly User Owner;
        private readonly byte _accountType;
        /// <value>account balance, currency value</value>
        public decimal Balance = 0m;
        
        /// <param name="owner">User instance that owns this account</param>
        /// <param name="accountType">byte value for account type</param>
        /// <param name="accountTypeCount">current number of accounts of <paramref name="accountType"/></param>
        protected Account(User owner, byte accountType, long accountTypeCount)
        {
            Owner = owner;
            _accountType = accountType;
            AccountNumber = $"{_accountType}".PadLeft(2, '0') + $"{accountTypeCount}".PadLeft(10, '0');
        }
        
        /// <returns>account summary detailing account number and current balance</returns>
        public override string ToString()
        {
            string prefix = _accountType == Savings ? "Savings - " : "Current - ";
            return prefix + $"{AccountNumber}: Dr{Balance:0.00}";
        }
        
        /// <returns>unique hashcode for account</returns>
        public override int GetHashCode()
        {
            return AccountNumber.GetHashCode();
        }
        
        /// <param name="obj">object for comparison</param>
        /// <returns>true if AccountNumber matches <paramref name="obj"/>.AccountNumber else false</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Account)) return false;
            return AccountNumber.Equals(((Account) obj).AccountNumber);
        }

        /// <summary>
        /// method for crediting (adding to) the account balance. This method should be called by <see cref="Transactions.Credit"/>
        /// </summary>
        /// <param name="amount">transaction amount, currency value</param>
        /// <exception cref="ArgumentOutOfRangeException">exception raised when <paramref name="amount"/> is less than or equal to zero</exception>
        public void Credit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            Balance += amount;
            Balance = Math.Round(Balance, 2);
        }

        /// <summary>
        /// method for debiting (deducting from) the account balance. This method should be called by <see cref="Transactions.Debit"/>
        /// </summary>
        /// <param name="amount">transaction amount, currency value</param>
        /// <exception cref="ArgumentOutOfRangeException">exception raised when <paramref name="amount"/> is less than or equal to zero</exception>
        /// <exception cref="DebitLimitExceededException">exception raised when <paramref name="amount"/> is greater than the <see cref="Account.WithdrawalLimit"/>></exception>#
        /// <exception cref="InsufficientBalanceException">exception raised when <paramref name="amount"/> is greater than the available balance</exception>
        public void Debit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (amount > WithdrawalLimit)
            {
                throw new DebitLimitExceededException();
            }
            
            if (Balance > amount)
            {
                Balance -= amount;
                Balance = Math.Round(Balance, 2);
                return;
            }

            throw new InsufficientBalanceException();
        }
    }
}