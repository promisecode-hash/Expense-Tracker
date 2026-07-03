using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseDto?> GetExpenseByIdAsync(Guid id);
        Task<IEnumerable<ExpenseDto>> GetUserExpensesAsync(Guid userId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<ExpenseDto> CreateExpenseAsync(Guid userId, CreateExpenseDto dto);
        Task<ExpenseDto> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto);
        Task<bool> DeleteExpenseAsync(Guid id);
    }

    public interface ICategoryService
    {
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId);
        Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(Guid id);
    }

    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }

    public interface ISummaryService
    {
        Task<SpendingSummaryDto> GetSpendingSummaryAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<CategorySpendingDto>> GetCategoryBreakdownAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}
