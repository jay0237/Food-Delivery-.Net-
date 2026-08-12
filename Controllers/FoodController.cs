using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

public class FoodController : Controller
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    // GET: /Food
    public async Task<IActionResult> Index()
    {
        var foods = await _foodService.GetAllAsync();

        return View(foods);
    }

    // GET: /Food/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Food/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Food food)
    {
        if (!ModelState.IsValid)
        {
            return View(food);
        }

        await _foodService.AddAsync(food);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Food/Edit/1
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var food = await _foodService.GetByIdAsync(id);

        if (food == null)
        {
            return NotFound();
        }

        return View(food);
    }

    // POST: /Food/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Food food)
    {
        if (!ModelState.IsValid)
        {
            return View(food);
        }

        await _foodService.UpdateAsync(food);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Food/Delete/1
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var food = await _foodService.GetByIdAsync(id);

        if (food == null)
        {
            return NotFound();
        }

        return View(food);
    }

    // POST: /Food/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _foodService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}