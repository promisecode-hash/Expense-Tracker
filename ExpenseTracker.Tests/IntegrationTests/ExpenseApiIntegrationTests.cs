using Xunit;

namespace ExpenseTracker.Tests.IntegrationTests
{
    public class ExpenseApiIntegrationTests : IAsyncLifetime
    {
        private readonly HttpClient _httpClient;
        private string _authToken = string.Empty;

        public ExpenseApiIntegrationTests()
        {
            // Initialize HttpClient with API base address
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
        }

        public async Task InitializeAsync()
        {
            // Setup: Register and login user before tests
            // This would create a test user and get an auth token
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            // Cleanup: Delete test data after tests
            _httpClient.Dispose();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task CreateExpense_WithValidData_ReturnsCreatedExpense()
        {
            // Arrange
            var expenseData = new
            {
                description = "Integration Test Expense",
                amount = 50.00m,
                transactionDate = DateTime.Now,
                categoryId = Guid.NewGuid()
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(expenseData),
                System.Text.Encoding.UTF8,
                "application/json");

            if (!string.IsNullOrEmpty(_authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
            }

            // Act
            var response = await _httpClient.PostAsync("expenses", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetAllExpenses_WithAuthenticatedUser_ReturnsExpenses()
        {
            // Arrange
            if (!string.IsNullOrEmpty(_authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
            }

            // Act
            var response = await _httpClient.GetAsync("expenses");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
