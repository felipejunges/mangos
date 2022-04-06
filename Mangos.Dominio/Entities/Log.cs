using System;

namespace Mangos.Dominio.Entities
{
    public class Log
    {
        public int Id { get; private set; }

        public DateTime DataHora { get; private set; }

        public string LogLevel { get; private set; }

        public string? Aplicacao { get; private set; }

        public string? CategoryName { get; private set; }

        public string Mensagem { get; private set; }

        public string? Exception { get; private set; }

        public Log(int id, DateTime dataHora, string logLevel, string? aplicacao, string? categoryName, string mensagem, string? exception)
        {
            Id = id;
            DataHora = dataHora;
            LogLevel = logLevel;
            Aplicacao = aplicacao;
            CategoryName = categoryName;
            Mensagem = mensagem;
            Exception = exception;
        }
    }
}