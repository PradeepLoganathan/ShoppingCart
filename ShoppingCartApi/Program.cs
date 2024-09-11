using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShoppingCartContext>(opt => opt.UseInMemoryDatabase("ShoppingCartDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// GET all products
app.MapGet("/api/cart/products", async (ShoppingCartContext db) => await db.Products.ToListAsync());

// POST add product to cart
app.MapPost("/api/cart/add", async (CartItem cartItem, ShoppingCartContext db) =>
{
    var product = await db.Products.FindAsync(cartItem.ProductId);
    if (product == null)
    {
        return Results.NotFound("Product not found");
    }

    db.CartItems.Add(cartItem);
    await db.SaveChangesAsync();

    return Results.Ok(cartItem);
});

// GET all cart items
app.MapGet("/api/cart/all", async (ShoppingCartContext db) => await db.CartItems.Include(c => c.Product).ToListAsync());

// DELETE remove cart item
app.MapDelete("/api/cart/remove/{id}", async (int id, ShoppingCartContext db) =>
{
    var cartItem = await db.CartItems.FindAsync(id);
    if (cartItem == null)
    {
        return Results.NotFound();
    }

    db.CartItems.Remove(cartItem);
    await db.SaveChangesAsync();

    return Results.Ok();
});

// Seed initial products
app.MapPost("/api/cart/seed", async (ShoppingCartContext db) =>
{
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Product A", Price = 10 },
            new Product { Name = "Product B", Price = 20 }
        );
        await db.SaveChangesAsync();
    }
    return Results.Ok("Seeded");
});

app.Run();
