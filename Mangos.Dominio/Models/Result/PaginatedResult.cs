using System;
using System.Collections.Generic;

namespace Mangos.Dominio.Models.Result
{
    public class PaginatedResult<T> where T : class
    {
        public int Total { get; private set; }

        public IList<T> Itens { get; private set; }

        public int Pagina { get; private set; }

        public int ItensPorPagina { get; private set; }

        public int TotalPaginas { get; }

        public PaginatedResult(int total, IList<T> itens, int pagina, int itensPorPagina)
        {
            Total = total;
            Itens = itens;
            Pagina = pagina;
            ItensPorPagina = itensPorPagina;
            TotalPaginas = (int)Math.Ceiling(total / (double)itensPorPagina);
        }

        public string MensagemRodape
        {
            get
            {
                var primeiro = (ItensPorPagina * (Pagina - 1)) + 1 > Total ? Total : (ItensPorPagina * (Pagina - 1)) + 1;
                var ultimo = ItensPorPagina * Pagina > Total ? Total : ItensPorPagina * Pagina;

                return $"Exibindo {primeiro} a {ultimo} do total de {Total} registros";
            }
        }
    }
}