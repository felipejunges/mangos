using Mangos.Dominio.Entities;
using Mangos.Dominio.Extensions;
using Mangos.Mvc.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class SessaoAcessoMappers
    {
        public static IEnumerable<SessaoAcessoListaModel> ToListaModel(IEnumerable<SessaoAcesso> sessoesAcesso)
            => sessoesAcesso.Select(ToListaModel);

        public static SessaoAcessoListaModel ToListaModel(SessaoAcesso sessaoAcesso)
        {
            return new SessaoAcessoListaModel(
                id: sessaoAcesso.Id,
                dataHoraAtualizacao: sessaoAcesso.DataHoraAtualizacao.ToTimeDiff(),
                status: sessaoAcesso.Logout ? "Deslogado"
                                                : sessaoAcesso.DataHoraExpiracao < DateTime.Now ? "Expirado"
                                                : $"Ativo [{sessaoAcesso.DataHoraExpiracao.ToTimeDiffDuplo()}]",
                usuario: sessaoAcesso.Usuario?.Nome ?? string.Empty,
                persistente: sessaoAcesso.Persistente,
                ip: sessaoAcesso.Ip,
                browser: sessaoAcesso.Browser,
                userAgent: sessaoAcesso.UserAgent,
                isMobile: sessaoAcesso.IsMobile,
                deslogado: sessaoAcesso.Deslogado
            );
        }
    }
}