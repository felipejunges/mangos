using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Models;
using System;
using System.Collections.Generic;

namespace Mangos.Mvc.Models
{
    [Serializable]
    public class SessionData
    {
        public bool Autenticado { get; private set; }
        public int? UsuarioId { get; private set; }
        public string? NomeUsuario { get; private set; }
        public int? GrupoId { get; private set; }
        public TipoAvisoVencimentosUsuario TipoAlertaVencimentos { get; private set; }
        public int DiasAlertaVencimentos { get; private set; }
        public TipoAvisoVencimentosUsuario TipoEmailVencimentos { get; private set; }
        public int DiasEmailVencimentos { get; private set; }
        public List<ItemNotificacaoModel> Notificacoes { get; private set; }
        public DateTime? DataHoraAtualizacaoSessaoAcesso { get; private set; }
        public DateTime? DataHoraExpiracaoSessaoAcesso { get; private set; }
        public DateTime DataHoraCriacao { get; private set; }

        public SessionData()
        {
            Autenticado = false;
            Notificacoes = new List<ItemNotificacaoModel>();
        }

        public SessionData(Usuario usuario, List<ItemNotificacaoModel> notificacoes)
        {
            Autenticado = true;
            UsuarioId = usuario.Id;
            NomeUsuario = usuario.Nome;
            GrupoId = usuario.GrupoId;
            TipoAlertaVencimentos = usuario.TipoAlertaVencimentos;
            DiasAlertaVencimentos = usuario.DiasAlertaVencimentos;
            TipoEmailVencimentos = usuario.TipoEmailVencimentos;
            DiasEmailVencimentos = usuario.DiasEmailVencimentos;
            Notificacoes = notificacoes;
            DataHoraAtualizacaoSessaoAcesso = null;
            DataHoraExpiracaoSessaoAcesso = null;
            DataHoraCriacao = DateTime.Now;
        }

        public bool VerificarSessaoEhNova()
        {
            return DataHoraAtualizacaoSessaoAcesso == null || DataHoraExpiracaoSessaoAcesso == null;
        }

        public bool VerificarDeveAtualizarSessao(TimeSpan expiracao, DateTime dataHoraExpiracaoSessao)
        {
            if (VerificarSessaoEhNova())
                return true;

            return DataHoraAtualizacaoSessaoAcesso!.Value.Add(expiracao) < DateTime.Now || DataHoraExpiracaoSessaoAcesso!.Value != dataHoraExpiracaoSessao;
        }

        public void MarcarAtualizacaoSessaoAcesso(DateTime dataHoraExpiracaoSessao)
        {
            DataHoraAtualizacaoSessaoAcesso = DateTime.Now;
            DataHoraExpiracaoSessaoAcesso = dataHoraExpiracaoSessao;
        }

        public void AtualizarNotificacoes(List<ItemNotificacaoModel> notificacoes)
        {
            Notificacoes = notificacoes;
        }
    }
}