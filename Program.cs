using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Managers;
using MunicipalServicesApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=municipal_services.db"));

// Register services
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<IssueManager>();
builder.Services.AddScoped<EventManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Initialize database and sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var issueManager = scope.ServiceProvider.GetRequiredService<IssueManager>();
    var eventManager = scope.ServiceProvider.GetRequiredService<EventManager>();
    
    try
    {
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Initialize sample data
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
