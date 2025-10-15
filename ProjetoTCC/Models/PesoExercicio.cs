public class PesoExercicio
{
    public int Id { get; set; }

    public int ExercicioFichaId { get; set; }
    public ExercicioFicha ExercicioFicha { get; set; }

    public decimal Peso { get; set; }
    public DateTime DataRegistro { get; set; } = DateTime.Now;
}