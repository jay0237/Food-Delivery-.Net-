using System.Security.Claims;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: /Order/Checkout
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var orders = await _orderService.GetMyOrdersAsync(userId.Value);

        return View(orders);
    }

    // POST: /Order/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create()
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var order = await _orderService.CreateOrderAsync(userId.Value);

        if (order == null)
        {
            TempData["Error"] =
                "Your cart is empty or no available food items were found.";

            return RedirectToAction("Index", "Cart");
        }

        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    // GET: /Order/Details/1
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        // Customer can only see their own order.
        if (order.UserId != userId.Value &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return View(order);
    }

    // GET: /Order/MyOrders
    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        var orders = await _orderService.GetMyOrdersAsync(userId.Value);

        return View(orders);
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