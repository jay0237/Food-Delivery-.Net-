using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Interfaces;

namespace FoodOrderingSystem.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterAsync(
        string fullName,
        string email,
        string password)
    {
        var existingUser =
            await _userRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            return false;
        }

        var passwordHash =
            BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            Role = "Customer"
        };

        await _userRepository.AddAsync(user);

        return true;
    }

    public async Task<User?> LoginAsync(
        string email,
        string password)
    {
        var user =
            await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return null;
        }

        return user;
    }
}