namespace ProjetoTCC.Models
{
    public class Medida
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;
    }
}
