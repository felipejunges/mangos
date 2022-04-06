using MediatR;

namespace Mangos.Dominio.Commands.DespesasRapidas
{
    public class IncluirDespesaRapidaCommand : IRequest
    {
        public int? PessoaId { get; private set; }

        public string Descricao { get; private set; }

        public decimal? Valor { get; private set; }

        public int? ContaBancariaId { get; private set; }

        public int? CartaoCreditoId { get; private set; }

        public bool AtualizarCoordenadas { get; private set; }

        public int? PessoaCoordenadaIdAtualizar { get; private set; }

        public double? Latitude { get; private set; }

        public double? Longitude { get; private set; }

        public IncluirDespesaRapidaCommand(int? pessoaId, string descricao, decimal? valor, int? contaBancariaId, int? cartaoCreditoId, bool atualizarCoordenadas, int? pessoaCoordenadaIdAtualizar, double? latitude, double? longitude)
        {
            PessoaId = pessoaId;
            Descricao = descricao;
            Valor = valor;
            ContaBancariaId = contaBancariaId;
            CartaoCreditoId = cartaoCreditoId;
            AtualizarCoordenadas = atualizarCoordenadas;
            PessoaCoordenadaIdAtualizar = pessoaCoordenadaIdAtualizar;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}