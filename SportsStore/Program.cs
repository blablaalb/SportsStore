using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
//builder.Services.AddTransient<IProductRepository, FakeProductRepository>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddTransient<IProductRepository, EFProductRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePages();
app.UseStaticFiles();

// It's important to add this route before the default one. The routing system processes routes in the order they are listed.
app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{page}",
    defaults: new { Controller = "Product", action = "List" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=List}/{id?}");


SeedData.EnsurePopulated(app);
app.Run();
