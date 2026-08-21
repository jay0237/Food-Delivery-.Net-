using FoodOrderingSystem.Data;
using FoodOrderingSystem.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodOrderingSystem.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldCreateUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        var repository = new TestUserRepository(context);

        var service = new AuthService(repository);

        var result = await service.RegisterAsync(
            "Test User",
            "test@example.com",
            "Test@123");

        Assert.True(result);

        var user = await context.Users
            .FirstOrDefaultAsync();

        Assert.NotNull(user);

        Assert.Equal(
            "test@example.com",
            user.Email);

        Assert.NotEqual(
            "Test@123",
            user.PasswordHash);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRejectDuplicateEmail()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        var repository = new TestUserRepository(context);

        var service = new AuthService(repository);

        await service.RegisterAsync(
            "First User",
            "same@example.com",
            "Test@123");

        var result = await service.RegisterAsync(
            "Second User",
            "same@example.com",
            "Test@456");

        Assert.False(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUserWithCorrectPassword()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        var repository = new TestUserRepository(context);

        var service = new AuthService(repository);

        await service.RegisterAsync(
            "Test User",
            "login@example.com",
            "Test@123");

        var user = await service.LoginAsync(
            "login@example.com",
            "Test@123");

        Assert.NotNull(user);

        Assert.Equal(
            "login@example.com",
            user.Email);
    }
}