using System;

namespace Mangos.Dominio.Utils
{
    public class StringUtils
    {
        public static string GeraStringRandomica(int tamanho)
        {
            string retorno = string.Empty;
            Random randTipo = new Random();
            Random randFinal = new Random();

            for (int i = 1; i <= tamanho; i++)
            {
                int tipoCaracter = randTipo.Next(1, 4);

                switch (tipoCaracter)
                {
                    case 1:
                        retorno += randFinal.Next(0, 9).ToString();
                        break;
                    case 2:
                        retorno += Convert.ToChar(randFinal.Next(65, 90));
                        break;
                    case 3:
                        retorno += Convert.ToChar(randFinal.Next(97, 122));
                        break;
                    default:
                        retorno += i.ToString();
                        break;
                }
            }

            return retorno;
        }
    }
}