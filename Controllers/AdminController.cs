using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IOrderService _orderService;

    public AdminController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: /Admin
    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        return View(orders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(
        int orderId,
        string status)
    {
        var success = await _orderService.UpdateStatusAsync(
            orderId,
            status);

        if (!success)
        {
            TempData["Error"] =
                "Unable to update the order status.";
        }

        return RedirectToAction(nameof(Index));
    }
}