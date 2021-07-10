using System;

namespace BankingApplication
{
    class ToTopMenu : Exception
    {
        public ToTopMenu(){}
    }

    class ToPreviousMenu : Exception
    {
        public ToPreviousMenu(){}
    }

    class EmptyInputException : Exception
    {
        public EmptyInputException(){}
    }

    class PassCodeException : Exception
    {
        public PassCodeException(){}
    }

    class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(){}
    }
}