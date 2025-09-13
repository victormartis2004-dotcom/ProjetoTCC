using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTCC.Data;       // namespace do seu BancoContext
using ProjetoTCC.Models;     // namespace das Models
using ProjetoTCC.ViewModels; // namespace da ViewModel (ajuste se diferente)
using System;
using System.Security.Claims;


public class AccountController : Controller
{
    private readonly BancoContext _context;

    public AccountController(BancoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View("Index"); // chama Views/Account/Index.cshtml
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Busca usuário pelo email
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == model.Email);
        if (usuario == null)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(model);
        }

        // Verifica senha (usando BCrypt)
        bool senhaValida = BCrypt.Net.BCrypt.Verify(model.Password, usuario.SenhaHash);
        if (!senhaValida)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(model);
        }

        // Criar claims (dados do usuário no cookie)
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
    };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe // Se quiser "lembrar" o login
        };

        // Faz login criando cookie de autenticação
        await HttpContext.SignInAsync("CookieAuth",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View("~/Views/Cadastro/Index.cshtml");
    }

    [HttpPost]
    public IActionResult Register(CadastroViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        string senhaHash = BCrypt.Net.BCrypt.HashPassword(model.Senha);

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Sobrenome = model.Sobrenome,
            Email = model.Email,
            SenhaHash = senhaHash,
            DataNascimento = model.DataNascimento
        };

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        var medida = new Medida
        {
            UsuarioId = usuario.Id,
            Peso = model.Peso,
            Altura = model.Altura,
            DataRegistro = DateTime.Now
        };

        _context.Medidas.Add(medida);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [Authorize]
    public IActionResult Perfil()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdStr, out int userId))
        {
            return Unauthorized();
        }

        var usuario = _context.Usuarios
            .Include(u => u.Medidas)
            .FirstOrDefault(u => u.Id == userId);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        var ultimaMedida = usuario.Medidas
            .OrderByDescending(m => m.DataRegistro)
            .FirstOrDefault();

        var model = new CadastroViewModel
        {
            Nome = usuario.Nome,
            Sobrenome = usuario.Sobrenome,
            DataNascimento = usuario.DataNascimento,
            Email = usuario.Email,
            Altura = ultimaMedida?.Altura ?? 0,
            Peso = ultimaMedida?.Peso ?? 0
        };

        return View("~/Views/Perfil/Index.cshtml", model);
    }
}