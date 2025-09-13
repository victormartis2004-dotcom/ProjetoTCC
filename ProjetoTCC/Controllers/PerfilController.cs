using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // Para usar Include
using ProjetoTCC.Data;
using ProjetoTCC.ViewModels;
using System.Linq;
using System.Security.Claims; // Necessário para FindFirstValue

namespace ProjetoTCC.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly BancoContext _context;

        public PerfilController(BancoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Pega o Id do usuário logado no claim NameIdentifier
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdStr, out int userId))
            {
                // Caso não consiga ler o Id, retorna Unauthorized (ou redireciona)
                return Unauthorized();
            }

            // Busca o usuário pelo Id, incluindo as medidas relacionadas
            var usuario = _context.Usuarios
                .Include(u => u.Medidas)
                .FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Pega a medida mais recente do usuário
            var ultimaMedida = usuario.Medidas
                .OrderByDescending(m => m.DataRegistro)
                .FirstOrDefault();

            // Mapeia para o ViewModel que a View espera
            var model = new CadastroViewModel
            {
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email,
                Altura = ultimaMedida?.Altura ?? 0,
                Peso = ultimaMedida?.Peso ?? 0
            };

            // Retorna a View com o model preenchido
            return View(model);
        }
    }
}