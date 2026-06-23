using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────

builder.Services.AddControllersWithViews();

// DbContext registered with Scoped lifetime (default for AddDbContext).
// Connection string is read from appsettings.json – never hardcoded here.
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LibraryDb")));

// Application services – business logic lives here, not in controllers.
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

// ── App pipeline ──────────────────────────────────────────────────────────────

var app = builder.Build();

// Apply pending migrations and seed data automatically on startup.
// HasData seeding is handled inside the migration itself, so this
// block just ensures the database schema is up-to-date.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
