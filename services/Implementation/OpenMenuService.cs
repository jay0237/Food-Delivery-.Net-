using FoodOrderingSystem.Models.DTOs.OpenMenu;
using FoodOrderingSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FoodOrderingSystem.Services.Implementations
{
    public class OpenMenuService : IOpenMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<OpenMenuService> _logger;

        public OpenMenuService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<OpenMenuService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenMenu:ApiKey"]
                ?? throw new InvalidOperationException("OpenMenu:ApiKey is not configured.");
            _logger = logger;
        }

        public async Task<List<OpenMenuItemDto>> SearchMenuItemsAsync(string search, string postalCode, string country)
        {
            var query = $"key={Uri.EscapeDataString(_apiKey)}&s={Uri.EscapeDataString(search)}&postal_code={Uri.EscapeDataString(postalCode)}&country={Uri.EscapeDataString(country)}";
            var url = $"https://www.openmenu.com/api/v2/search.php?{query}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };
            var result = JsonSerializer.Deserialize<OpenMenuSearchResponse>(jsonString, options);

            var items = result?.Response?.Result?.Items ?? new List<OpenMenuItemDto>();

            foreach (var menu in result?.Response?.Result?.Menus ?? new List<OpenMenuDto>())
            {
                foreach (var item in menu.Items ?? new List<OpenMenuItemDto>())
                {
                    if (string.IsNullOrWhiteSpace(item.RestaurantName))
                    {
                        item.RestaurantName = menu.RestaurantName;
                    }

                    items.Add(item);
                }
            }

            return items;
        }
    }
}
