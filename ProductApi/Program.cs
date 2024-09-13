using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductApi.Data;
using ProductApi.Models;

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
builder.Services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductApi", Version = "v1" });
});

var app = builder.Build();
// Seed the database with default products during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
    ProductContext.SeedData(dbContext); // Call the seed method
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
app.MapGet("/products", async (ProductContext db) => await db.Products.ToListAsync())
    .WithTags("Products");

// GET specific product by ID
app.MapGet("/products/{id}", async (int id, ProductContext db) =>
{
    var product = await db.Products.FindAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
}).WithTags("Products");

// POST add new product
app.MapPost("/products", async (Product product, ProductContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
}).WithTags("Products");

// DELETE specific product by ID
app.MapDelete("/products/{id}", async (int id, ProductContext db) =>
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

#region insecure
// Insecure SQL Injection Endpoint
// app.MapGet("/products/insecure/{productName}", async (string productName, ShoppingCartContext db) =>
// {
//     // Insecure: Directly concatenate user input into an SQL query (SQL Injection)
//     var query = $"SELECT * FROM Products WHERE Name = '{productName}'";  // Vulnerable to SQL Injection
//     var products = await db.Products.FromSqlRaw(query).ToListAsync();

//     return products.Any() ? Results.Ok(products) : Results.NotFound();
// }).WithTags("Products");
#endregion

app.Run();