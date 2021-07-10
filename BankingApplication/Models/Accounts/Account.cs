using System;
using System.Text.RegularExpressions;

#nullable enable

namespace BankingApplication.Models.Accounts
{
    public abstract class Account
    {
        protected static readonly byte Savings = 0;
        protected static readonly byte Current = 1; 
        public readonly string AccountNumber;
        public readonly User Owner;
        public readonly byte AccountType;
        public decimal Balance = 0m;
        
        protected Account(User owner, byte accountType, long accountTypeCount)
        {
            Owner = owner;
            AccountType = accountType;
            AccountNumber = $"{AccountType}".PadLeft(2, '0') + $"{accountTypeCount}".PadLeft(10, '0');
        }

        public override string ToString()
        {
            string prefix = AccountType == Savings ? "Savings - " : "Current - ";
            return prefix + $"{AccountNumber}: Dr{Balance:0.00}";
        }

        public override int GetHashCode()
        {
            return AccountNumber.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Account)) return false;
            return AccountNumber.Equals(((Account) obj).AccountNumber);
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            Balance += amount;
            Balance = Math.Round(Balance, 2);
        }

        public void Debit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
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