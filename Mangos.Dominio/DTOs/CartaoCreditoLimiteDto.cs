namespace Mangos.Dominio.DTOs
{
    public class CartaoCreditoLimiteDto
    {
        public int CartaoCreditoId { get; private set; }
        public string CartaoCredito { get; private set; }
        public decimal ValorLimite { get; private set; }
        public decimal ValorLimiteUtilizado { get; private set; }

        public CartaoCreditoLimiteDto(int cartaoCreditoId, string cartaoCredito, decimal valorLimite, decimal valorLimiteUtilizado)
        {
            CartaoCreditoId = cartaoCreditoId;
            CartaoCredito = cartaoCredito;
            ValorLimite = valorLimite;
            ValorLimiteUtilizado = valorLimiteUtilizado;
        }
    }
}