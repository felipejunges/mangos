using System;

namespace Mangos.Dominio.Entities
{
    public class DispositivoConectado
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Identificador { get; set; }

        public string RefreshToken { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public DateTime DataHoraUltimoRefresh { get; set; }

        public DateTime DataHoraExpiracaoRefreshToken { get; set; }

        public string? IP { get; set; }

        public string? Sistema { get; set; }

        public bool Expirado { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public bool EstaExpirada()
        {
            return Expirado || DataHoraExpiracaoRefreshToken < DateTime.Now;
        }

        public DispositivoConectado()
        {
            Identificador = string.Empty;
            RefreshToken = string.Empty;
        }
    }
}