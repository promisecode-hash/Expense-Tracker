namespace ExpenseTracker.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class ExpenseNotFoundException : DomainException
    {
        public ExpenseNotFoundException(Guid expenseId) 
            : base($"Expense with ID {expenseId} was not found.") { }
    }

    public class CategoryNotFoundException : DomainException
    {
        public CategoryNotFoundException(Guid categoryId) 
            : base($"Category with ID {categoryId} was not found.") { }
    }

    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId) 
            : base($"User with ID {userId} was not found.") { }
    }

    public class InvalidExpenseException : DomainException
    {
        public InvalidExpenseException(string reason) 
            : base($"Invalid expense: {reason}") { }
    }
}
