using FoodOrderingSystem.Models.DTOs.OpenMenu;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FoodOrderingSystem.Services.Implementations
{
    public class OpenMenuService : IOpenMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenMenuService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenMenu:ApiKey"] ?? string.Empty;
        }

        public async Task<List<OpenMenuItemDto>> SearchMenuItemsAsync(string search, string postalCode, string country)
        {
            var url = $"https://www.openmenu.com/api/v2/search.php?key={_apiKey}&s={Uri.EscapeDataString(search ?? "")}&postal_code={Uri.EscapeDataString(postalCode ?? "")}&country={Uri.EscapeDataString(country ?? "")}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("===== OPENMENU RESPONSE =====");
            Console.WriteLine(jsonString);
            Console.WriteLine("============================");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<OpenMenuSearchResponse>(jsonString, options);

            var items = result?.Response?.Result?.Items ?? new List<OpenMenuItemDto>();

            var dtos = new List<OpenMenuItemDto>();
            foreach (var item in items)
            {
                decimal? price = null;
                if (!string.IsNullOrWhiteSpace(item.MenuItemPrice) && decimal.TryParse(item.MenuItemPrice, out decimal parsedPrice))
                {
                    price = parsedPrice;
                }

                dtos.Add(new OpenMenuItemDto
                {
                    MenuItemName = item.MenuItemName ?? string.Empty,
                    MenuItemDescription = item.MenuItemDescription ?? string.Empty,
                    MenuItemPrice = price,
                    ImageUrl = item.ImageUrl,
                    RestaurantName = item.RestaurantName ?? string.Empty,
                    CuisineTypePrimary = item.CuisineTypePrimary,
                    CityTown = item.CityTown,
                    StateProvince = item.StateProvince,
                    Country = item.Country,
                    Address1 = item.Address1
                });
            }

            return dtos;
        }
    }
}
