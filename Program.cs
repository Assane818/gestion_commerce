using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebGesCommande.Core;
using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Data.Fixtures;
using WebGesCommande.Models;
using WebGesCommande.Services;
using WebGesCommande.Services.Impl;
using WebService.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection")!;

builder.Services.AddDbContext<WebGesCommandeContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Connexion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    }
);

builder.Services.AddTransient<DataFixtures>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ISession<Panier>, Session<Panier>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<ILivreurService, LivreurService>();
builder.Services.AddScoped<ILivraisonService, LivraisonService>();
builder.Services.AddScoped<IPayementService, PayementService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Temps d'expiration de la session
    options.Cookie.HttpOnly = true;                  // Le cookie sera HttpOnly
    options.Cookie.IsEssential = true;               // Le cookie est essentiel pour le fonctionnement de l'application
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WebGesCommandeContext>();
    var dataFixtures = scope.ServiceProvider.GetRequiredService<DataFixtures>();
    
    // Charger les données si nécessaire
    dataFixtures.LoadData();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Connexion}/{id?}");

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Security/Error404");
    }
});

app.Run();
