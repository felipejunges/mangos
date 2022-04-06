namespace Mangos.Dominio.Extensions
{
    public static class StringExtensions
    {
        public static string FirstUpper(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            else if (str.Length == 1)
                return str.ToUpper();
            else
                return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public static string DiaSemFeira(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            string[] split = str.Split('-');

            if (split.Length == 0)
                return string.Empty;

            return split[0];
        }
    }
}