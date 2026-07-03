namespace ExpenseTracker.Application.DTOs
{
    public class SpendingSummaryDto
    {
        public decimal TotalSpending { get; set; }
        public int TransactionCount { get; set; }
        public decimal AverageTransaction { get; set; }
        public Dictionary<string, decimal> SpendingByCategory { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CategorySpendingDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
}
