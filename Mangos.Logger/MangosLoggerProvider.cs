using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using System;

namespace Mangos.Logger
{
    public class MangosLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ILogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aplicacao;

        public MangosLoggerProvider(Func<string, LogLevel, bool> filter, Func<ILogRepository> repositoryFactory, Func<IUnitOfWork> unitOfWorkFactory, string aplicacao)
        {
            _filter = filter;
            _repository = repositoryFactory.Invoke();
            _unitOfWork = unitOfWorkFactory.Invoke();
            _aplicacao = aplicacao;
        }

        public ILogger CreateLogger(string categoryName) => new MangosLogger(_filter, _repository, _unitOfWork, categoryName, _aplicacao);

        public void Dispose() { }
    }
}