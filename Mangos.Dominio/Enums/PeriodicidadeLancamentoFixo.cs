using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum PeriodicidadeLancamentoFixo
    {
        [Description("Mensal")]
        Mensal = 'M',
        [Description("Anual")]
        Anual = 'A'
    }
}