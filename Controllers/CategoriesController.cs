using FoodOrderingSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return View(categories);
    }
}