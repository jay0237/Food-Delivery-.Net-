using System.Security.Claims;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        string fullName,
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(
                "",
                "All fields are required.");

            return View();
        }

        var registered = await _authService.RegisterAsync(
            fullName,
            email,
            password);

        if (!registered)
        {
            ModelState.AddModelError(
                "",
                "An account with this email already exists.");

            return View();
        }

        return RedirectToAction(nameof(Login));
    }

    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(
                "",
                "Email and password are required.");

            return View();
        }

        var user = await _authService.LoginAsync(
            email,
            password);

        if (user == null)
        {
            ModelState.AddModelError(
                "",
                "Invalid email or password.");

            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(ClaimTypes.Name,
                user.FullName),

            new Claim(ClaimTypes.Email,
                user.Email),

            new Claim(ClaimTypes.Role,
                user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Home");
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}