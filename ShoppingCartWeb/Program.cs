using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ShoppingCartWeb.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Set up localization options
var supportedCultures = new[] { new CultureInfo("en-AU") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-AU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Register ApiSettings configuration
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));


builder.Services.AddHttpClient("ProductApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ProductApiBaseUrl"]);
});

builder.Services.AddHttpClient("CartApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:CartApiBaseUrl"]);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Use localization middleware
app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
