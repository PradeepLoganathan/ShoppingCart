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
//builder.Services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductDb"));
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer("Server=sqlserver-service,1433;Database=ProductDb;User Id=SA;Password=Afhk#^98^&;TrustServerCertificate=True;MultipleActiveResultSets=true;"));

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
    dbContext.Database.EnsureCreated();
    ProductContext.SeedData(dbContext); // Call the seed method
}

// Use the CORS policy
app.UseCors("AllowAll");

// Enable Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Redirect root "/" to "/swagger"
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

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
// Causes SQL Injection
app.MapGet("/products/insecure/{productName}", async (string productName, ProductContext db) =>
{
    // Insecure: Directly concatenate user input into an SQL query (SQL Injection)
    var query = $"SELECT * FROM Products WHERE Name = '{productName}'";  // Vulnerable to SQL Injection
    var products = db.Products.FromSqlRaw(query).ToListAsync();
    return Results.Ok(products);
}).WithTags("Products");


//Causes Command Injection
app.MapGet("/run/{command}", (string command) =>
{
    // Vulnerable: Directly passing user input to system commands
    System.Diagnostics.Process.Start("cmd.exe", $"/c {command}");
    return Results.Ok($"Executed command: {command}");
}).WithTags("Insecure");

// unvalidated redirects and forwards
app.MapGet("/redirect", (string url) =>
{
    // Vulnerable: User input directly used in a redirect
    return Results.Redirect(url);
}).WithTags("Insecure");

// Causes cross site scripting - xss
app.MapGet("/xss", (string input) =>
{
    // Vulnerable: Outputting unsanitized user input directly to the webpage
    return Results.Content($"<html><body><h1>{input}</h1></body></html>", "text/html");
}).WithTags("Insecure");

// Insecure deserialization
app.MapPost("/deserialize", (HttpRequest request) =>
{
    using (var reader = new StreamReader(request.Body))
    {
        var serializedObject = reader.ReadToEnd();
        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(serializedObject);
        // Vulnerable: Deserializing untrusted input
        return Results.Ok(obj);
    }
}).WithTags("Insecure");

// sensitive data exposure
app.MapPost("/login", (string username, string password) =>
{
    // Vulnerable: Storing passwords in plain text or logging sensitive information
    Console.WriteLine($"Logging in user: {username} with password: {password}");
    return Results.Ok("Login attempt");
}).WithTags("Insecure");

// Hard coded credentials
app.MapGet("/api-key", () =>
{
    //Vulnerable: Hardcoded API key
    var apiKey = "12345-ABCDE";
    return Results.Ok($"API key is: {apiKey}");
}).WithTags("Insecure");

// Example of unused variable
string unusedVariable = "This is not used";

// Example of an unused function
void UnusedFunction()
{
    Console.WriteLine("This function is never called.");
}


#endregion


app.Run();