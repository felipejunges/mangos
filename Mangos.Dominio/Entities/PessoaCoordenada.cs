namespace Mangos.Dominio.Entities
{
    public class PessoaCoordenada
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string? Observacao { get; set; }

        public virtual Pessoa? Pessoa { get; set; }
    }
}