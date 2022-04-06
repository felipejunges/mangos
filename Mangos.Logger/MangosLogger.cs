using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mangos.Logger
{
    public class MangosLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ILogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aplicacao;

        private IExternalScopeProvider? ScopeProvider { get; set; }

        private bool _busy = false;

        public MangosLogger(Func<string, LogLevel, bool> filter, ILogRepository repository, IUnitOfWork unitOfWork, string categoryName, string aplicacao)
        {
            _filter = filter;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _categoryName = categoryName;
            _aplicacao = aplicacao;
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => (_filter == null || _filter(_categoryName, logLevel));

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string exceptionMessage = string.Empty;
            var exceptionLaco = exception!;
            while (exceptionLaco != null)
            {
                if (exceptionMessage != string.Empty)
                    exceptionMessage += Environment.NewLine;

                exceptionMessage += exceptionLaco.Message;

                if (!string.IsNullOrEmpty(exceptionLaco.StackTrace))
                    exceptionMessage += Environment.NewLine + "Stack Trace: " + exceptionLaco.StackTrace;

                if (!string.IsNullOrEmpty(exceptionLaco.Source))
                    exceptionMessage += Environment.NewLine + "Source: " + exceptionLaco.Source;

                if (exceptionLaco.TargetSite != null && !string.IsNullOrEmpty(exceptionLaco.TargetSite.Name))
                    exceptionMessage += Environment.NewLine + "TargetSite: " + exceptionLaco.TargetSite.Name;

                exceptionLaco = exceptionLaco.InnerException;
            }

            Task.Run(async () => await GravarLogAsync(logLevel.ToString(), _categoryName, formatter(state, exception), exceptionMessage));
        }

        private async Task GravarLogAsync(string logLevel, string categoryName, string mensagem, string exception)
        {
            if (_busy)
                return;

            _busy = true;

            var log = new Log(
                id: 0,
                dataHora: DateTime.Now,
                logLevel: logLevel,
                aplicacao: _aplicacao,
                categoryName: categoryName,
                mensagem: mensagem,
                exception: exception);

            try
            {
                await _repository.IncluirLogAsync(log);
                await _unitOfWork.SaveChangesAsync();

                _busy = false;
            }
            catch { }
        }
    }
}