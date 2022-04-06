using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoLancamento
    {
        [Description("Receita")]
        Receita = 'R',
        [Description("Despesa")]
        Despesa = 'D'
    }
}