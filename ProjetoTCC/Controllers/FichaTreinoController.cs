using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTCC.Data;
using ProjetoTCC.Models;
using ProjetoTCC.ViewModels;
using System.Security.Claims;

[Authorize]
public class FichaTreinoController : Controller
{
    private readonly BancoContext _context;

    public FichaTreinoController(BancoContext context)
    {
        _context = context;
    }

    // GET: FichaTreino/Create
    public IActionResult Create()
    {
        var model = new FichaCadastroViewModel();
        model.Exercicios.Add(new ExercicioCadastroViewModel());
        return View(model);
    }

    // POST: FichaTreino/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(FichaCadastroViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out int userId))
            return Unauthorized();

        var ficha = new FichaTreino
        {
            NomeFicha = model.NomeFicha,
            UsuarioId = userId,
            DataCriacao = DateTime.Now,
            Exercicios = model.Exercicios.Select(e => new ExercicioFicha
            {
                NomeExercicio = e.NomeExercicio,
                Series = e.Series,
                Pesos = new List<PesoExercicio>
                {
                    new PesoExercicio { Peso = e.Peso, DataRegistro = DateTime.Now }
                }
            }).ToList()
        };

        _context.FichaTreino.Add(ficha);
        _context.SaveChanges();

        return View("~/Views/Home/Index.cshtml", model);
    }
}