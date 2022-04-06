using System;

namespace Mangos.Dominio.Extensions
{
    public static class Int32Extensions
    {
        public static string ToStringPluralized(this int valor, string termoSingular, string termoPlural)
        {
            return $"{valor.ToString("f0")} {(valor == 1 ? termoSingular : termoPlural)}";
        }

        public static string ToStringPluralized(this double valor, string termoSingular, string termoPlural)
        {
            return ToStringPluralized((int)Math.Floor(valor), termoSingular, termoPlural);
        }
    }
}