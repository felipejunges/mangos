using Mangos.Dominio.Entities;
using Mangos.Dominio.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class PessoaGeoModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string? Observacao { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Display(Name = "Distância")]
        [DisplayFormat(DataFormatString = "{0:f2}")]
        public double Distancia { get; set; }

        public string DistanciaExtenso
        {
            get
            {
                return Distancia < 1000
                        ? Distancia.ToString("f0") + "m"
                        : (Distancia / 1000).ToString(Distancia < 10000 ? "f1" : "f0") + "km";
            }
        }

        public PessoaGeoModel()
        {
            Nome = string.Empty;
        }

        public PessoaGeoModel(int id, string nome, string? observacao, double latitude, double longitude, double distancia)
        {
            Id = id;
            Nome = nome;
            Observacao = observacao;
            Latitude = latitude;
            Longitude = longitude;
            Distancia = distancia;
        }

        public PessoaGeoModel(PessoaCoordenada pessoaCoordenada, double? latitude = null, double? longitude = null)
        {
            Id = pessoaCoordenada.Id;
            Nome = pessoaCoordenada.Pessoa?.Nome ?? string.Empty;
            Observacao = pessoaCoordenada.Observacao;
            Latitude = pessoaCoordenada.Latitude;
            Longitude = pessoaCoordenada.Longitude;
            Distancia = latitude == null || longitude == null ? 0 : GeoLocation.GetDistance(latitude.Value, longitude.Value, pessoaCoordenada.Latitude, pessoaCoordenada.Longitude);
        }
    }
}