using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Tests.UnitTests
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IExpenseRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ExpenseService _service;

        public ExpenseServiceTests()
        {
            _mockRepository = new Mock<IExpenseRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ExpenseService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateExpenseAsync_WithValidData_ReturnsExpenseDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createDto = new CreateExpenseDto
            {
                Description = "Lunch",
                Amount = 25.50m,
                TransactionDate = DateTime.Now,
                CategoryId = Guid.NewGuid()
            };

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = createDto.Description,
                Amount = createDto.Amount,
                TransactionDate = createDto.TransactionDate,
                CategoryId = createDto.CategoryId,
                UserId = userId
            };

            var expectedDto = new ExpenseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                TransactionDate = expense.TransactionDate,
                CategoryId = expense.CategoryId
            };

            _mockMapper.Setup(m => m.Map<Expense>(createDto)).Returns(expense);
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Expense>())).ReturnsAsync(expense);
            _mockMapper.Setup(m => m.Map<ExpenseDto>(expense)).Returns(expectedDto);

            // Act
            var result = await _service.CreateExpenseAsync(userId, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Description, result.Description);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Expense>()), Times.Once);
        }

        [Fact]
        public async Task CreateExpenseAsync_WithZeroAmount_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createDto = new CreateExpenseDto
            {
                Description = "Test",
                Amount = 0,
                TransactionDate = DateTime.Now,
                CategoryId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidExpenseException>(() => _service.CreateExpenseAsync(userId, createDto));
        }

        [Fact]
        public async Task GetExpenseByIdAsync_WithValidId_ReturnsExpenseDto()
        {
            // Arrange
            var expenseId = Guid.NewGuid();
            var expense = new Expense
            {
                Id = expenseId,
                Description = "Test Expense",
                Amount = 50m,
                TransactionDate = DateTime.Now
            };

            var expectedDto = new ExpenseDto { Id = expense.Id, Description = expense.Description };

            _mockRepository.Setup(r => r.GetByIdAsync(expenseId)).ReturnsAsync(expense);
            _mockMapper.Setup(m => m.Map<ExpenseDto>(expense)).Returns(expectedDto);

            // Act
            var result = await _service.GetExpenseByIdAsync(expenseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseId, result.Id);
        }
    }
}
