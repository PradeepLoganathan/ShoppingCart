using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Data;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()       
              .AllowAnyMethod()       
              .AllowAnyHeader();      
    });
});


// Add services for EF Core and Swagger
builder.Services.AddDbContext<ShoppingCartContext>(opt => opt.UseInMemoryDatabase("ShoppingCartDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingCartApi", Version = "v1" });
});

var app = builder.Build();


// Use the CORS policy
app.UseCors("AllowAll");

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Redirect root "/" to "/swagger"
app.MapGet("/", () => Results.Redirect("/swagger"));


// === Cart Endpoints (Grouped under 'Cart') === //

// GET all cart items
app.MapGet("/cart", async (ShoppingCartContext db) => await db.CartItems.Include(c => c.Product).ToListAsync())
    .WithTags("Cart");

// GET specific cart item by ID
app.MapGet("/cart/{id}", async (int id, ShoppingCartContext db) =>
{
    var cartItem = await db.CartItems
        .Include(c => c.Product)
        .FirstOrDefaultAsync(c => c.CartItemId == id);

    return cartItem is not null ? Results.Ok(cartItem) : Results.NotFound();
}).WithTags("Cart");

// POST add item to cart
app.MapPost("/cart", async (CartItem cartItem, ShoppingCartContext db) =>
{
    // Directly add the cart item with the full product details
    db.CartItems.Add(cartItem);
    await db.SaveChangesAsync();

    return Results.Created($"/cart/{cartItem.CartItemId}", cartItem);
}).WithTags("Cart");

// DELETE specific cart item by ID
app.MapDelete("/cart/{id}", async (int id, ShoppingCartContext db) =>
{
    var cartItem = await db.CartItems.FindAsync(id);
    if (cartItem is null)
    {
        return Results.NotFound();
    }

    db.CartItems.Remove(cartItem);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Cart");

app.Run();