using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoAvisoVencimentosUsuario
    {
        [Description("Nenhum")]
        Nenhum = 'N',
        [Description("Ambos")]
        Ambos = 'A',
        [Description("Receitas")]
        Receitas = 'R',
        [Description("Despesas")]
        Despesas = 'D'
    }
}