namespace ProjetoTCC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public DateTime DataNascimento { get; set; }

        public ICollection<Medida> Medidas { get; set; }
    }
}
