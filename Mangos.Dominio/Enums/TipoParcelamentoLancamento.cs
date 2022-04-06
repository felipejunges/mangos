using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoParcelamentoLancamento
    {
        [Description("Parcelar")]
        Parcelar = 'P',
        [Description("Replicar")]
        Replicar = 'R'
    }
}