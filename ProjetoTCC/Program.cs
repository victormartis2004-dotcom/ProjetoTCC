using Microsoft.EntityFrameworkCore;
using ProjetoTCC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura o DbContext para usar SQL Server
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura autenticação por cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";    // redireciona para login se não autenticado
        options.LogoutPath = "/Account/Logout";  // rota de logout
        options.AccessDeniedPath = "/Account/Login"; // acesso negado redireciona para login
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure o pipeline HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE: ordem correta
app.UseAuthentication();  // habilita autenticação
app.UseAuthorization();   // habilita autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
