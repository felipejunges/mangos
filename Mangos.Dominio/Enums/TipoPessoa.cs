using System.ComponentModel;

namespace Mangos.Dominio.Enums
{
    public enum TipoPessoa
    {
        [Description("Cliente")]
        Cliente = 'C',
        [Description("Fornecedor")]
        Fornecedor = 'F',
        [Description("Ambos")]
        Ambos = 'A'
    }
}