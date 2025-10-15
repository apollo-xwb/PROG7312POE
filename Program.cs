/*
 * Program.cs - Application Configuration and Startup
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Dependency injection configuration and database initialization patterns provided by AI assistant.
 * Student implementation: Service registration, middleware configuration, and application architecture.
 */

using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Managers;
using MunicipalServicesApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=municipal_services.db"));

builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<IssueManager>();
builder.Services.AddScoped<EventManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var issueManager = scope.ServiceProvider.GetRequiredService<IssueManager>();
    var eventManager = scope.ServiceProvider.GetRequiredService<EventManager>();
    
    try
    {
        context.Database.EnsureCreated();
        await issueManager.InitializeSampleDataAsync();
        await eventManager.InitializeSampleDataAsync();
        Console.WriteLine("Database initialized successfully with sample data.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
    }
}

app.Run();
