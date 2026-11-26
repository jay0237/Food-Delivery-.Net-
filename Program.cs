using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Implementations;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Implementations;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpClient<IOpenMenuService, OpenMenuService>();

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Always apply pending migrations first
    context.Database.Migrate();

    // Seed Admin User
    var adminEmail = "admin@foodordering.com";
    if (!context.Users.Any(u => u.Email == adminEmail))
    {
        var admin = new User
        {
            FullName = "System Admin",
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(admin);
    }

    // Seed Initial Categories
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category
            {
                Name = "Burgers",
                Description = "Classic and premium burgers prepared fresh to order.",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=800&q=80"
            },
            new Category
            {
                Name = "Pizza",
                Description = "Hand-tossed pizza with fresh toppings and rich sauces.",
                ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?auto=format&fit=crop&w=800&q=80"
            },
            new Category
            {
                Name = "Drinks",
                Description = "Cold beverages, shakes, and refreshers for every meal.",
                ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?auto=format&fit=crop&w=800&q=80"
            }
        );
    }

    var additionalCategories = new[]
    {
        new { Name = "Healthy", Description = "Fresh bowls and feel-good plates.", ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=800&q=80" },
        new { Name = "Asian", Description = "Bold noodles, bao, and wok-fired favourites.", ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?auto=format&fit=crop&w=800&q=80" },
        new { Name = "Fries", Description = "Crispy fries with classic and loaded toppings.", ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?auto=format&fit=crop&w=800&q=80" }
    };

    context.Categories.AddRange(additionalCategories
        .Where(category => !context.Categories.Any(existing => existing.Name == category.Name))
        .Select(category => new Category
        {
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl
        }));

    context.SaveChanges();

    if (!context.Foods.Any())
    {
        var categories = context.Categories.ToDictionary(category => category.Name);
        var seededFoods = new[]
        {
            new { Name = "Smoky Smash Burger", Category = "Burgers", Description = "Double smashed patties, American cheese, pickles, and house sauce.", Price = 289m, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=900&q=85" },
            new { Name = "Truffle Mushroom Pizza", Category = "Pizza", Description = "Wild mushrooms, mozzarella, truffle oil, and fresh herbs.", Price = 449m, ImageUrl = "https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=900&q=85" },
            new { Name = "Paneer Tikka Bowl", Category = "Healthy", Description = "Charred paneer, greens, grains, and a bright mint dressing.", Price = 329m, ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=85" },
            new { Name = "Crispy Chicken Bao", Category = "Asian", Description = "Three fluffy bao, crispy chicken, slaw, and chilli mayo.", Price = 379m, ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?auto=format&fit=crop&w=900&q=85" },
            new { Name = "Mango Chili Cooler", Category = "Drinks", Description = "Fresh mango, lime, chilli salt, and sparkling water.", Price = 149m, ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?auto=format&fit=crop&w=900&q=85" },
            new { Name = "Classic Margherita", Category = "Pizza", Description = "San Marzano tomato, fior di latte, basil, and olive oil.", Price = 349m, ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=900&q=85" }
        };

        context.Foods.AddRange(seededFoods
            .Where(food => categories.ContainsKey(food.Category))
            .Select(food => new Food
            {
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                ImageUrl = food.ImageUrl,
                CategoryId = categories[food.Category].Id,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            }));

        context.SaveChanges();
    }

    var foodCategories = context.Categories.ToDictionary(category => category.Name);
    var menuFoods = new[]
    {
        new { Name = "Smoky Smash Burger", Category = "Burgers", Description = "Double smashed patties, American cheese, pickles, and house sauce.", Price = 289m, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Classic Cheeseburger", Category = "Burgers", Description = "Juicy grilled beef, cheddar, lettuce, tomato, and tangy burger sauce.", Price = 259m, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=900&q=85" },
        new { Name = "BBQ Bacon Burger", Category = "Burgers", Description = "Grilled beef patty with smoky barbecue sauce, crispy bacon, and melted cheese.", Price = 329m, ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Crispy Chicken Burger", Category = "Burgers", Description = "Crunchy chicken fillet, fresh slaw, and creamy pepper mayonnaise.", Price = 279m, ImageUrl = "https://images.unsplash.com/photo-1606755962773-d324e0a13086?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Garden Veggie Burger", Category = "Burgers", Description = "A hearty vegetable patty with lettuce, tomato, onion, and herb dressing.", Price = 239m, ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Truffle Mushroom Pizza", Category = "Pizza", Description = "Wild mushrooms, mozzarella, truffle oil, and fresh herbs.", Price = 449m, ImageUrl = "https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Classic Margherita", Category = "Pizza", Description = "San Marzano tomato, mozzarella, fresh basil, and olive oil.", Price = 349m, ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Pepperoni Feast Pizza", Category = "Pizza", Description = "Stone-baked pizza topped with pepperoni, mozzarella, and tomato sauce.", Price = 429m, ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Farmhouse Veg Pizza", Category = "Pizza", Description = "Bell peppers, red onion, olives, mushrooms, and mozzarella on a crisp base.", Price = 389m, ImageUrl = "https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Spicy Paneer Pizza", Category = "Pizza", Description = "Tandoori paneer, peppers, onion, and a lively chilli-tomato sauce.", Price = 419m, ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Classic Salted Fries", Category = "Fries", Description = "Golden-cut potatoes fried until crisp and finished with sea salt.", Price = 119m, ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Loaded Cheese Fries", Category = "Fries", Description = "Crispy fries covered in warm cheddar sauce and a sprinkle of herbs.", Price = 179m, ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Peri Peri Fries", Category = "Fries", Description = "Hot, crunchy fries tossed with our tangy peri peri seasoning.", Price = 149m, ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Paneer Tikka Bowl", Category = "Healthy", Description = "Charred paneer, greens, grains, and a bright mint dressing.", Price = 329m, ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141eefd?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Green Garden Salad", Category = "Healthy", Description = "Crisp greens, cucumber, tomato, and a light lemon-herb dressing.", Price = 229m, ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141eefd?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Crispy Chicken Bao", Category = "Asian", Description = "Three fluffy bao with crispy chicken, crunchy slaw, and chilli mayo.", Price = 379m, ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Mango Chili Cooler", Category = "Drinks", Description = "Fresh mango, lime, a hint of chilli, and sparkling water.", Price = 149m, ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?auto=format&fit=crop&w=900&q=85" },
        new { Name = "Chocolate Milkshake", Category = "Drinks", Description = "A thick, creamy chocolate shake topped with chocolate drizzle.", Price = 189m, ImageUrl = "https://images.unsplash.com/photo-1572490122747-3968b75cc699?auto=format&fit=crop&w=900&q=85" }
    };

    var existingFoodNames = context.Foods
        .Select(food => food.Name)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    context.Foods.AddRange(menuFoods
        .Where(food => foodCategories.ContainsKey(food.Category) && !existingFoodNames.Contains(food.Name))
        .Select(food => new Food
        {
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            ImageUrl = food.ImageUrl,
            CategoryId = foodCategories[food.Category].Id,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        }));

    context.SaveChanges();
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Note: Changed to UseStaticFiles for compatibility across standard .NET setups

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();