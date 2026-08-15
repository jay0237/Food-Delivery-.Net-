using System.Security.Claims;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

     [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var cart = await _cartService.GetOrCreateCartAsync(userId.Value);

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        int foodId,
        int quantity = 1)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var success = await _cartService.AddToCartAsync(
            userId.Value,
            foodId,
            quantity);

        if (!success)
        {
            TempData["Error"] = "Unable to add this food to your cart.";
        }

        return RedirectToAction("Index", "Food");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        int foodId,
        int quantity)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        await _cartService.UpdateQuantityAsync(
            userId.Value,
            foodId,
            quantity);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int foodId)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        await _cartService.RemoveFromCartAsync(
            userId.Value,
            foodId);

        return RedirectToAction(nameof(Index));
    }

    private int? GetUserId()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (int.TryParse(userId, out var id))
        {
            return id;
        }

        return null;
    }
}
