namespace ExpenseTracker.Domain.Entities
{
    public class Expense
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public User? User { get; set; }
    }
}
