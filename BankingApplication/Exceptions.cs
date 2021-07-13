using System;

namespace BankingApplication
{
    /// <summary>
    /// Custom Exception thrown to send user back to the initial startup menu execution flow
    /// </summary>
    public class ToTopMenu : Exception
    {
        public ToTopMenu(){}
    }

    /// <summary>
    /// Custom Exception thrown to send user back to previous menu execution flow
    /// </summary>
    public class ToPreviousMenu : Exception
    {
        public ToPreviousMenu(){}
    }

    /// <summary>
    /// Custom Exception thrown when user enters a null value for a mandatory field
    /// </summary>
    public class EmptyInputException : Exception
    {
        public EmptyInputException(){}
    }

    /// <summary>
    /// Custom Exception thrown when user enters an invalid passcode
    /// </summary>
    public class PassCodeException : Exception
    {
        public PassCodeException(){}
    }

    /// <summary>
    /// Custom Exception thrown when an overdraft is attempted from an account
    /// </summary>
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(){}
    }

    /// <summary>
    /// Custom Exception thrown when user attempts to withdraw an amount greater than the withdrawal limit for their
    /// account type
    /// </summary>
    public class DebitLimitExceededException : Exception
    {
        public DebitLimitExceededException(){}
    }
}