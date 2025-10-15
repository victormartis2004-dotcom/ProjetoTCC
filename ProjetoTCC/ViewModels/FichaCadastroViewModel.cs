namespace ProjetoTCC.ViewModels
{
    public class FichaCadastroViewModel
    {
        public string NomeFicha { get; set; }
        public List<ExercicioCadastroViewModel> Exercicios { get; set; } = new List<ExercicioCadastroViewModel>();
    }
}
