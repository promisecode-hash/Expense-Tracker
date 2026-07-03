using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(IExpenseService service, ILogger<ExpensesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            var userId = GetCurrentUserId();
            var expenses = await _service.GetUserExpensesAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
            var expense = await _service.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound(new { message = "Expense not found" });

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var expense = await _service.CreateExpenseAsync(userId, dto);
                return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto)
        {
            try
            {
                var expense = await _service.UpdateExpenseAsync(id, dto);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var result = await _service.DeleteExpenseAsync(id);
            if (!result)
                return NotFound(new { message = "Expense not found" });

            return NoContent();
        }

        [HttpGet("filter/category/{categoryId}")]
        public async Task<IActionResult> GetExpensesByCategory(Guid categoryId)
        {
            var expenses = await _service.GetExpensesByCategoryAsync(categoryId);
            return Ok(expenses);
        }

        [HttpGet("filter/date-range")]
        public async Task<IActionResult> GetExpensesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = GetCurrentUserId();
            var expenses = await _service.GetExpensesByDateRangeAsync(userId, startDate, endDate);
            return Ok(expenses);
        }
    }
}
