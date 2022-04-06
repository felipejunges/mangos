using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoLancamentoFixo
    {
        [Description("Receita")]
        Receita = 'R',
        [Description("Despesa")]
        Despesa = 'D',
        [Description("Receita cartão")]
        ReceitaCartao = 'A',
        [Description("Despesa cartão")]
        DebitoCartao = 'C'
    }
}