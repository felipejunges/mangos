using Mangos.Dominio.Entities;
using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Mvc.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Mvc.Services
{
    public class DataKeeperService
    {
        private static TimeSpan INTERVALO_CACHE_EXPIRA_SLIDING = TimeSpan.FromMinutes(2);
        private static TimeSpan INTERVALO_CACHE_EXPIRA_ABSOLUTE = TimeSpan.FromMinutes(5);

        private readonly IMemoryCache _memoryCache;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly NotificacaoService _notificacaoService;

        public DataKeeperService(IMemoryCache memoryCache, IUsuarioRepository usuarioRepository, NotificacaoService notificacaoService)
        {
            _memoryCache = memoryCache;
            _usuarioRepository = usuarioRepository;
            _notificacaoService = notificacaoService;
        }

        public async Task<SessionData> GetData(int usuarioId, string chaveSessao)
        {
            if (usuarioId == default || chaveSessao == default)
                return new SessionData();

            var data = await _memoryCache.GetOrCreateAsync(chaveSessao, async entry =>
            {
                entry.SlidingExpiration = INTERVALO_CACHE_EXPIRA_SLIDING;
                entry.AbsoluteExpirationRelativeToNow = INTERVALO_CACHE_EXPIRA_ABSOLUTE;

                var (usuario, notificacoes) = await BuscarDadosUsuario(usuarioId);
                return new SessionData(usuario, notificacoes);
            });

            return data;
        }

        public void Invalidar(string chaveSessao)
        {
            _memoryCache.Remove(chaveSessao);
        }

        private async Task<(Usuario usuario, List<ItemNotificacaoModel> notificacoes)> BuscarDadosUsuario(int usuarioLogadoId)
        {
            var usuario = await _usuarioRepository.ObterUsuarioAsync(usuarioLogadoId);

            if (usuario is null)
                return default;

            var notificacoes = await _notificacaoService.ObterNotificacoesAsync(usuario);

            return (usuario, notificacoes);
        }
    }
}