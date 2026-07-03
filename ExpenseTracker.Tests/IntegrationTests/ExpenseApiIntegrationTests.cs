using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Infrastructure.Data;
using ExpenseTracker.Application.DTOs;
using Xunit;

namespace ExpenseTracker.Tests.IntegrationTests
{
    public class ExpenseApiIntegrationTests : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;
        private string _authToken = string.Empty;

        public ExpenseApiIntegrationTests()
        {
            _factory = new CustomWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var registerData = new
            {
                username = "testuser",
                email = "testuser@example.com",
                password = "Password123!",
                firstName = "Test",
                lastName = "User"
            };

            var registerResponse = await _httpClient.PostAsJsonAsync("api/auth/register", registerData);
            registerResponse.EnsureSuccessStatusCode();

            var loginData = new
            {
                email = "testuser@example.com",
                password = "Password123!"
            };

            var loginResponse = await _httpClient.PostAsJsonAsync("api/auth/login", loginData);
            loginResponse.EnsureSuccessStatusCode();

            var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
            _authToken = authResponse?.Token ?? string.Empty;
        }

        public Task DisposeAsync()
        {
            _httpClient.Dispose();
            _factory.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task CreateExpense_WithValidData_ReturnsCreatedExpense()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

            var expenseData = new
            {
                description = "Integration Test Expense",
                amount = 50.00m,
                transactionDate = DateTime.UtcNow,
                categoryId = Guid.NewGuid()
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/expenses", expenseData);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAllExpenses_WithAuthenticatedUser_ReturnsExpenses()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

            // Act
            var response = await _httpClient.GetAsync("api/expenses");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private class CustomWebApplicationFactory : WebApplicationFactory<Program>
        {
            protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ExpenseTrackerDbContext>));

                    if (dbContextDescriptor != null)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    services.AddDbContext<ExpenseTrackerDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("ExpenseTrackerTestDb");
                    });

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ExpenseTrackerDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                });
            }
        }
    }
}
