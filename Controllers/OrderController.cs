using System.Security.Claims;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(
        IOrderService orderService,
        ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
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

        var cart = await _cartService.GetOrCreateCartAsync(userId.Value);

        return View(cart);
    }

    // GET: /Order/Address - New step to collect address
    [HttpGet]
    public IActionResult Address()
    {
        var model = new OrderAddressViewModel();
        return View(model);
    }

    // POST: /Order/Create - Updated to accept address
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderAddressViewModel addressModel)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View("Address", addressModel);
        }

        var order = await _orderService.CreateOrderAsync(
            userId.Value,
            addressModel.DeliveryAddress,
            addressModel.DeliveryCity,
            addressModel.DeliveryState,
            addressModel.DeliveryZipCode,
            addressModel.DeliveryPhone,
            addressModel.DeliveryInstructions,
            addressModel.Latitude,
            addressModel.Longitude);

        if (order == null)
        {
            TempData["Error"] =
                "Your cart is empty or no available food items were found.";

            return RedirectToAction("Index", "Cart");
        }

        TempData["Success"] = "Order placed successfully!";
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