using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public SummaryService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<SpendingSummaryDto> GetSpendingSummaryAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseRepository.GetByDateRangeAsync(userId, startDate, endDate);
            var expenseList = expenses.ToList();

            if (!expenseList.Any())
            {
                return new SpendingSummaryDto
                {
                    TotalSpending = 0,
                    TransactionCount = 0,
                    AverageTransaction = 0,
                    SpendingByCategory = new Dictionary<string, decimal>(),
                    StartDate = startDate,
                    EndDate = endDate
                };
            }

            var totalSpending = expenseList.Sum(e => e.Amount);
            var transactionCount = expenseList.Count;
            var averageTransaction = totalSpending / transactionCount;

            var spendingByCategory = expenseList
                .GroupBy(e => e.Category?.Name ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            return new SpendingSummaryDto
            {
                TotalSpending = totalSpending,
                TransactionCount = transactionCount,
                AverageTransaction = averageTransaction,
                SpendingByCategory = spendingByCategory,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public async Task<IEnumerable<CategorySpendingDto>> GetCategoryBreakdownAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseRepository.GetByDateRangeAsync(userId, startDate, endDate);
            var expenseList = expenses.ToList();

            if (!expenseList.Any())
                return Enumerable.Empty<CategorySpendingDto>();

            var totalSpending = expenseList.Sum(e => e.Amount);

            var categoryBreakdown = expenseList
                .GroupBy(e => e.Category)
                .Select(g => new CategorySpendingDto
                {
                    CategoryId = g.Key?.Id ?? Guid.Empty,
                    CategoryName = g.Key?.Name ?? "Uncategorized",
                    Total = g.Sum(e => e.Amount),
                    Count = g.Count(),
                    Percentage = totalSpending > 0 ? (g.Sum(e => e.Amount) / totalSpending) * 100 : 0
                })
                .OrderByDescending(c => c.Total)
                .ToList();

            return categoryBreakdown;
        }
    }
}
