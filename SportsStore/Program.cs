using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
//builder.Services.AddTransient<IProductRepository, FakeProductRepository>();
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(defaultConnectionString));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite(identityConnectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddTransient<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePages();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// It's important to add this route before the default one. The routing system processes routes in the order they are listed.
app.MapControllerRoute(
    name: "Error",
    pattern: "Error",
    defaults: new { controller = "Error", action = "Error" });
app.MapControllerRoute(
    name: null,
    pattern: "{category}/Page{page:int}",
    defaults: new { controller = "Product", action = "List" });
app.MapControllerRoute(
    name: null,
    pattern: "Page{page:int}",
    defaults: new { controller = "Product", action = "List", page = 1 });
app.MapControllerRoute(
    name: null,
    pattern: "{category}",
    defaults: new {controller = "Product", action = "List", page = 1});
app.MapControllerRoute(
    name: null,
    pattern: "",
    defaults: new { controller = "Product", action = "List", page = 1 });
app.MapControllerRoute(
    name: null,
    pattern: "{controller}/{action}/{id?}");

SeedData.EnsurePopulated(app);
IdentitySeedData.EnsurePopulated(app);
app.Run();
