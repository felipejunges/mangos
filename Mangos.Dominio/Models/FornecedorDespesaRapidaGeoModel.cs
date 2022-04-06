namespace Mangos.Dominio.Models
{
    public class FornecedorDespesaRapidaGeoModel
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }

        public string? PessoaNome { get; set; }

        public string? UltimaDescricaoDespesa { get; set; }

        public int? ContaBancariaId { get; set; }

        public int? CartaoCreditoId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Distancia { get; set; }
    }
}