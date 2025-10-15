public class ExercicioFicha
{
    public int Id { get; set; }

    public int FichaTreinoId { get; set; }
    public FichaTreino FichaTreino { get; set; }

    public string NomeExercicio { get; set; }
    public int Series { get; set; }

    public ICollection<PesoExercicio> Pesos { get; set; } = new List<PesoExercicio>();
}
