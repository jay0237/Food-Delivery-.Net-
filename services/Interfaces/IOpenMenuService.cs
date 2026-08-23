using FoodOrderingSystem.Models.DTOs.OpenMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IOpenMenuService
    {
        Task<List<OpenMenuItemDto>> SearchMenuItemsAsync(string search, string postalCode, string country);
    }
}
