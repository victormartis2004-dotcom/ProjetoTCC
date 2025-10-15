using ProjetoTCC.Models;

public class FichaTreino
{
    public int Id { get; set; }
    public string NomeFicha { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public ICollection<ExercicioFicha> Exercicios { get; set; } = new List<ExercicioFicha>();
}