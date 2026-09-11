using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace FoodOrderingSystem.Controllers;

[Route("category")]
[Route("categories")]

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: /Category
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();

        return View(categories);
    }

    // GET: /Category/Create
    [HttpGet("create")]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Category/Create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryService.AddAsync(category);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Category/Edit/1
    [HttpGet("edit/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: /Category/Edit/1
    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryService.UpdateAsync(category);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Category/Delete/1
    [HttpGet("delete/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: /Category/Delete/1
    [HttpPost("delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}