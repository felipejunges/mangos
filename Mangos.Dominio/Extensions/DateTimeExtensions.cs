using System;

namespace Mangos.Dominio.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeDiff(this DateTime? dataHora, int? dias = null)
        {
            if (dataHora == null)
                return string.Empty;

            return dataHora.Value.ToTimeDiff(dias);
        }

        public static string ToTimeDiff(this DateTime dataHora, int? dias = null)
        {
            var diff = DateTime.Now < dataHora ? dataHora - DateTime.Now : DateTime.Now - dataHora;

            if (dias.HasValue && diff.TotalDays >= dias.Value)
                return dataHora.ToString("dd/MM/yyyy HH:mm");
            else if (diff.TotalDays >= 1)
                return $"{diff.TotalDays.ToStringPluralized("dia", "dias")}, {diff.Hours.ToStringPluralized("hora", "horas")}";
            else if (diff.TotalHours >= 1)
                return $"{diff.Hours.ToStringPluralized("hora", "horas")}";
            else if (diff.TotalMinutes >= 1)
                return $"{diff.Minutes.ToStringPluralized("minuto", "minutos")}";
            else
                return $"{diff.Seconds.ToStringPluralized("segundo", "segundos")}";
        }

        public static string ToTimeDiffDuplo(this DateTime dataHora, int? dias = null)
        {
            var diff = DateTime.Now < dataHora ? dataHora - DateTime.Now : DateTime.Now - dataHora;

            if (dias.HasValue && diff.TotalDays >= dias.Value)
                return dataHora.ToString("dd/MM/yyyy HH:mm");
            else if (diff.TotalDays >= 1)
                return $"{diff.TotalDays.ToStringPluralized("dia", "dias")}, {diff.Hours.ToStringPluralized("hora", "horas")}";
            else if (diff.TotalHours >= 1)
                return $"{diff.Hours.ToStringPluralized("hora", "horas")}, {diff.Minutes.ToStringPluralized("minuto", "minutos")}";
            else
                return $"{diff.Minutes.ToStringPluralized("minuto", "minutos")}, {diff.Seconds.ToStringPluralized("segundo", "segundos")}";
        }

        public static string ToDiaDiff(this DateTime? dataHora)
        {
            if (dataHora == null)
                return string.Empty;

            if (dataHora.Value.Year < DateTime.Now.Year)
                return dataHora.Value.ToString("dd/MM/yyyy HH:mm");
            else if (dataHora.Value.Date <= DateTime.Now.Date.AddDays(-7))
                return dataHora.Value.ToString("dd/MMM HH:mm");
            else if (dataHora.Value.Date < DateTime.Now.Date.AddDays(-1))
                return $"{dataHora.Value.ToString("dddd").FirstUpper().DiaSemFeira()}, {dataHora.Value:HH:mm}";
            else if (dataHora.Value.Date == DateTime.Now.Date.AddDays(-1))
                return $"Ontem, {dataHora.Value:HH:mm}";
            else
                return $"Hoje, {dataHora.Value:HH:mm}";
        }

        public static DateTime UtcToBrasilia(this DateTime dataHoraUtc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dataHoraUtc, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        }

        public static DateTime BrasiliaToUtc(this DateTime dataHoraBrasilia)
        {
            dataHoraBrasilia = DateTime.SpecifyKind(dataHoraBrasilia, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dataHoraBrasilia, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        }
    }
}