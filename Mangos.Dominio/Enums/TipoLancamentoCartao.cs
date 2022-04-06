using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoLancamentoCartao
    {
        [Description("Receita")]
        Receita = 'R',
        [Description("Despesa")]
        Despesa = 'D'
    }
}