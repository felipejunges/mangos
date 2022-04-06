using System;
using System.Text.RegularExpressions;

namespace Mangos.Dominio.Types.TypeValidations
{
    public class TypeValidations
    {
        public static bool IsInteger32(string valor)
        {
            try
            {
                Int32.Parse(valor);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInteger64(string valor)
        {
            try
            {
                Int64.Parse(valor);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(string valor)
        {
            try
            {
                decimal.Parse(valor);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(object valor)
        {
            if (valor is null)
                return false;

            return IsDateTime(valor.ToString());
        }

        public static bool IsDateTime(string? valor)
        {
            if (valor is null)
                return false;

            try
            {
                DateTime dt = DateTime.Parse(valor);

                return (dt >= Convert.ToDateTime("01/01/1753") && dt <= Convert.ToDateTime("31/12/9999"));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsHexadecimal(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return Regex.IsMatch(str, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static bool IsValidYearMonth(string data)
        {
            if (string.IsNullOrEmpty(data))
                return false;

            return Regex.IsMatch(data, @"^(2\d{3}|19\d{2})(1[0-2]|0[1-9]|\d)$");
        }

        public static bool IsValidIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            return Regex.IsMatch(ip, "^(([1]?[0-9]{1,2}|2([0-4][0-9]|5[0-5]))\\.){3}([1]?[0-9]{1,2}|2([0-4][0-9]|5[0-5]))$");
        }

        public static bool IsValidEmail(string enderecoEmail)
        {
            if (string.IsNullOrEmpty(enderecoEmail))
                return false;

            return Regex.IsMatch(enderecoEmail, "\\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsValidPhone(string numeroTelefone)
        {
            if (string.IsNullOrEmpty(numeroTelefone))
                return false;

            return Regex.IsMatch(numeroTelefone, "^[(]{1}\\d{2}[)]{1}[ ]{1}\\d{4}[-]{1}\\d{4}$")
                   || Regex.IsMatch(numeroTelefone, "^[(]{1}\\d{2}[)]{1}[ ]{1}\\d{5}[-]{1}\\d{4}$");
        }

        public static bool IsValidCelularPhone(string numeroTelefone)
        {
            if (string.IsNullOrEmpty(numeroTelefone))
                return false;

            // (99) 9999-9999 ou (99) 9 9999-9999
            return Regex.IsMatch(numeroTelefone, "^[(]{1}\\d{2}[)]{1}[ ]{1}[56789]{1}\\d{3}[-]{1}\\d{4}$")
                   || Regex.IsMatch(numeroTelefone, "^[(]{1}\\d{2}[)]{1}[ ]{1}[9]{1}[ ]{1}[56789]{1}\\d{3}[-]{1}\\d{4}$");
        }

        public static bool IsValidaCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return false;

            return Regex.IsMatch(cep, "^\\d{5}[-]{1}\\d{3}$");
        }
    }
}