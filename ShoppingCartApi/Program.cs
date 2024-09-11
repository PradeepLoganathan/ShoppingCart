using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

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
// Seed the database with default products during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    ShoppingCartContext.SeedData(dbContext); // Call the seed method
}

// Use the CORS policy
app.UseCors("AllowAll");

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Redirect root "/" to "/swagger"
app.MapGet("/", () => Results.Redirect("/swagger"));

// === Product Endpoints (Grouped under 'Products') === //

// GET all products
app.MapGet("/products", async (ShoppingCartContext db) => await db.Products.ToListAsync())
    .WithTags("Products");

// GET specific product by ID
app.MapGet("/products/{id}", async (int id, ShoppingCartContext db) =>
{
    var product = await db.Products.FindAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
}).WithTags("Products");

// POST add new product
app.MapPost("/products", async (Product product, ShoppingCartContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
}).WithTags("Products");

// DELETE specific product by ID
app.MapDelete("/products/{id}", async (int id, ShoppingCartContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Products");

// === Cart Endpoints (Grouped under 'Cart') === //

// GET all cart items
app.MapGet("/cart", async (ShoppingCartContext db) => await db.CartItems.Include(c => c.Product).ToListAsync())
    .WithTags("Cart");

// GET specific cart item by ID
app.MapGet("/cart/{id}", async (int id, ShoppingCartContext db) =>
{
    var cartItem = await db.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == id);
    return cartItem is not null ? Results.Ok(cartItem) : Results.NotFound();
}).WithTags("Cart");

// POST add item to cart
app.MapPost("/cart", async (CartItem cartItem, ShoppingCartContext db) =>
{
    var product = await db.Products.FindAsync(cartItem.ProductId);
    if (product == null)
    {
        return Results.NotFound("Product not found");
    }

    db.CartItems.Add(cartItem);
    await db.SaveChangesAsync();

    return Results.Created($"/cart/{cartItem.Id}", cartItem);
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