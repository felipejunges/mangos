using System;

namespace Mangos.Mvc.Utils
{
    public static class DateUtil
    {
        public static DateTime RemoveMilliseconds(this DateTime dataHora)
        {
            return dataHora.AddMilliseconds(dataHora.Millisecond * -1);
        }

        public static DateTime RemoveSeconds(this DateTime dataHora)
        {
            return dataHora.RemoveMilliseconds().AddSeconds(dataHora.Second * -1);
        }
    }
}