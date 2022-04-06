using System;

namespace Mangos.Dominio.Extensions
{
    public struct DateTimeWithZone
    {
        //private static TimeZoneInfo BR_TIMEZONE => TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        private static TimeZoneInfo BR_TIMEZONE => TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        private readonly DateTime _utcDateTime;
        private readonly TimeZoneInfo _timeZone;

        public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            _utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
            _timeZone = timeZone;
        }

        public static DateTimeWithZone Br(DateTime dateTime)
            => new DateTimeWithZone(dateTime, BR_TIMEZONE);

        public DateTime UniversalTime => _utcDateTime;

        public TimeZoneInfo TimeZone => _timeZone;

        public DateTime LocalTime => TimeZoneInfo.ConvertTime(_utcDateTime, _timeZone);
    }
}