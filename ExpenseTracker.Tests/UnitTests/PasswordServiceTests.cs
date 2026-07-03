using Xunit;
using Moq;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Services;

namespace ExpenseTracker.Tests.UnitTests
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _service;

        public PasswordServiceTests()
        {
            _service = new PasswordService();
        }

        [Fact]
        public void HashPassword_WithValidPassword_ReturnsHash()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hash = _service.HashPassword(password);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "SecurePassword123!";
            var hash = _service.HashPassword(password);

            // Act
            var result = _service.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var password = "SecurePassword123!";
            var wrongPassword = "WrongPassword123!";
            var hash = _service.HashPassword(password);

            // Act
            var result = _service.VerifyPassword(wrongPassword, hash);

            // Assert
            Assert.False(result);
        }
    }
}
