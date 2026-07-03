using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpenseDto?> GetExpenseByIdAsync(Guid id)
        {
            var expense = await _repository.GetByIdAsync(id);
            return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<IEnumerable<ExpenseDto>> GetUserExpensesAsync(Guid userId)
        {
            var expenses = await _repository.GetAllByUserAsync(userId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryAsync(Guid categoryId)
        {
            var expenses = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var expenses = await _repository.GetByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto> CreateExpenseAsync(Guid userId, CreateExpenseDto dto)
        {
            if (dto.Amount <= 0)
                throw new InvalidExpenseException("Amount must be greater than 0");

            var expense = _mapper.Map<Expense>(dto);
            expense.UserId = userId;

            var createdExpense = await _repository.CreateAsync(expense);
            return _mapper.Map<ExpenseDto>(createdExpense);
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto)
        {
            var expense = await _repository.GetByIdAsync(id);
            if (expense == null)
                throw new ExpenseNotFoundException(id);

            if (dto.Amount <= 0)
                throw new InvalidExpenseException("Amount must be greater than 0");

            _mapper.Map(dto, expense);
            var updatedExpense = await _repository.UpdateAsync(expense);
            return _mapper.Map<ExpenseDto>(updatedExpense);
        }

        public async Task<bool> DeleteExpenseAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId)
        {
            var categories = await _repository.GetAllByUserAsync(userId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.UserId = userId;

            var createdCategory = await _repository.CreateAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new CategoryNotFoundException(id);

            _mapper.Map(dto, category);
            var updatedCategory = await _repository.UpdateAsync(category);
            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
