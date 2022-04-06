using Mangos.Mvc.Models.ViewModels.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class PessoaCoordenadaDadosModel : HashedModel
    {
        public int PessoaId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Display(Name = "Observação")]
        public string? Observacao { get; set; }

        public PessoaCoordenadaDadosModel()
        {
        }

        public PessoaCoordenadaDadosModel(int id, int grupoId, int pessoaId, double? latitude, double? longitude, string? observacao)
        {
            Id = id;
            GrupoId = grupoId;
            PessoaId = pessoaId;
            Latitude = latitude;
            Longitude = longitude;
            Observacao = observacao;

            SetValidationHash();
        }

        public static PessoaCoordenadaDadosModel Novo(int grupoId, int pessoaId)
        {
            return new PessoaCoordenadaDadosModel(
                id: 0,
                grupoId: grupoId,
                pessoaId: pessoaId,
                latitude: null,
                longitude: null,
                observacao: null
            );
        }
    }
}