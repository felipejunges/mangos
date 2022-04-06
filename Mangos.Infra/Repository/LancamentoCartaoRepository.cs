using Mangos.Dominio.DTOs;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class LancamentoCartaoRepository : ILancamentoCartaoRepository
    {
        private readonly MangosDb Db;

        public LancamentoCartaoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<LancamentoCartao?> ObterLancamentoCartaoAsync(int id)
        {
            return Db.LancamentosCartao
                            .Include(l => l.Pessoa)
                            .Where(l => l.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<DateTime?> ObterDataPrimeiroLancamentoAbertoAsync(int cartaoCreditoId)
        {
            return Db.LancamentosCartao.Where(l => l.CartaoCreditoId == cartaoCreditoId && !l.GeradoLancamento).MinAsync(l => (DateTime?)l.MesReferencia);
        }

        public Task<DateTime?> ObterDataUltimoLancamentoGeradoAsync(int cartaoCreditoId)
        {
            return Db.LancamentosCartao.Where(l => l.CartaoCreditoId == cartaoCreditoId && l.GeradoLancamento).MaxAsync(l => (DateTime?)l.MesReferencia);
        }

        public async Task<DateTime> ObterMesReferenciaGerarLancamentoCartaoAsync(int cartaoCreditoId)
        {
            // TODO: isso deveria estar na camada service

            var ultimoMesReferenciaGerado = await Db.LancamentosCartao.Where(o => o.GeradoLancamento).MaxAsync(o => (DateTime?)o.MesReferencia);

            if (ultimoMesReferenciaGerado != null)
            {
                return ultimoMesReferenciaGerado.Value.AddMonths(1);
            }
            else
            {
                var cartaoCredito = await Db.CartoesCredito.FirstOrDefaultAsync(c => c.Id == cartaoCreditoId);

                var data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                if (cartaoCredito is null)
                    return data;

                return data.AddMonths(cartaoCredito.OffsetReferenciaVencimento);
            }
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoAsync(int grupoId, int? cartaoCreditoId, string? descricao, string? pessoa, DateTime? mesReferenciaInicial, DateTime? mesReferenciaFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.Pessoa)
                        .Include(l => l.CartaoCredito)
                        .Where(o =>
                            o.GrupoId == grupoId
                            && (cartaoCreditoId == null || o.CartaoCreditoId == cartaoCreditoId.Value)
                            && (string.IsNullOrEmpty(descricao) || o.Descricao.Contains(descricao))
                            && (string.IsNullOrEmpty(pessoa) || o.Pessoa!.Nome.Contains(pessoa))
                            && (mesReferenciaInicial == null || o.MesReferencia >= mesReferenciaInicial)
                            && (mesReferenciaFinal == null || o.MesReferencia <= mesReferenciaFinal)
                        )
                        .OrderBy(o => o.MesReferencia)
                        .ThenBy(o => o.CartaoCredito!.Descricao)
                        .ThenBy(o => o.DataHoraCadastro)
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoAgrupadosAsync(int grupoId, Guid agrupador)
        {
            return Db.LancamentosCartao
                        .Include(l => l.Pessoa)
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.Agrupador == agrupador
                        )
                        .OrderBy(o => o.MesReferencia)
                        .ThenBy(o => o.CartaoCredito!.Descricao)
                        .ThenBy(o => o.DataHoraCadastro)
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoFecharAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia)
        {
            return Db.LancamentosCartao
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.GeradoLancamento
                            && l.CartaoCreditoId == cartaoCreditoId
                            && l.MesReferencia == mesReferencia
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoReabrirAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia)
        {
            return Db.LancamentosCartao
                        .Include(l => l.Lancamento)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.GeradoLancamento
                            && l.CartaoCreditoId == cartaoCreditoId
                            && l.MesReferencia == mesReferencia
                        )
                        .ToListAsync();
        }

        public async Task<(DateTime? DataMinima, DateTime? DataMaxima)> ObterDatasMenorMaiorVencimentosNaoFechadosAsync(int grupoId)
        {
            return (await Db.LancamentosCartao
                            .Where(l =>
                                l.GrupoId == grupoId
                                && !l.GeradoLancamento
                            )
                            .GroupBy(l => l.GrupoId)
                            .Select(l => new
                            {
                                DataMinima = l.Min(m => (DateTime?)m.MesReferencia),
                                DataMaxima = l.Max(m => (DateTime?)m.MesReferencia)
                            })
                            .ToListAsync()
                        )
                        .Select(o => (o.DataMinima, o.DataMaxima))
                        .FirstOrDefault();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoAlertaAsync(int grupoId)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.CartaoCredito!.GerarLancamentoFecharMes
                            && !l.GeradoLancamento
                            && l.MesReferencia.AddMonths(l.CartaoCredito.OffsetReferenciaVencimento).AddDays(l.CartaoCredito.DiaVencimento - 1) <= DateTime.Now.Date
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarDespesasPagasDaPessoaNoTrackingAsync(int grupoId, int pessoaId, DateTime dataMinima)
        {
            return Db.LancamentosCartao
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.PessoaId == pessoaId
                            && l.TipoLancamento == TipoLancamentoCartao.Despesa
                            && l.MesReferencia >= dataMinima
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoDaPessoaAsync(int grupoId, int pessoaId)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Include(l => l.Lancamento)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.PessoaId == pessoaId
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoVencendoAsync(int grupoId, DateTime dataFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.CartaoCredito!.GerarLancamentoFecharMes
                            && !l.GeradoLancamento
                            && l.MesReferencia.AddMonths(l.CartaoCredito.OffsetReferenciaVencimento).AddDays(l.CartaoCredito.DiaVencimento - 1) <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoRelatorioAsync(int grupoId, int? cartaoCreditoId, DateTime? mesInicial, DateTime? mesFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.CartaoCredito!.ExibirDadosRelatorio
                            && (cartaoCreditoId == null || l.CartaoCreditoId == cartaoCreditoId.Value)
                            && (mesInicial == null || l.MesReferencia >= mesInicial.Value)
                            && (mesFinal == null || l.MesReferencia <= mesFinal.Value)
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoPendentesRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Include(l => l.Categoria)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && (l.CategoriaId == null || l.Categoria!.RelatorioTotal)
                            && (l.CartaoCredito!.GerarLancamentoFecharMes && !l.GeradoLancamento)
                            && l.MesReferencia.AddMonths(l.CartaoCredito.OffsetReferenciaVencimento) >= mesInicial
                            && l.MesReferencia.AddMonths(l.CartaoCredito.OffsetReferenciaVencimento) <= mesFinal
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoPagosRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Include(l => l.Categoria)
                        .Where(o =>
                            o.GrupoId == grupoId
                            && (o.CategoriaId == null || o.Categoria!.RelatorioTotal)
                            && (!o.CartaoCredito!.GerarLancamentoFecharMes || o.GeradoLancamento)
                            && o.MesReferencia.AddMonths(o.CartaoCredito.OffsetReferenciaVencimento) >= mesInicial
                            && o.MesReferencia.AddMonths(o.CartaoCredito.OffsetReferenciaVencimento) <= mesFinal
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.Categoria)
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && (
                                situacao == "T"
                                || (situacao == "P" && l.CartaoCredito!.GerarLancamentoFecharMes && !l.GeradoLancamento)
                                || (situacao == "R" && (!l.CartaoCredito!.GerarLancamentoFecharMes || l.GeradoLancamento))
                            )
                            && (tipo == "T" || (tipo == "R" && l.TipoLancamento == TipoLancamentoCartao.Receita) || (tipo == "D" && l.TipoLancamento == TipoLancamentoCartao.Despesa))
                            && l.CategoriaId != null
                            && l.Categoria!.RelatorioTotal
                            && l.MesReferencia.AddMonths(l.CartaoCredito!.OffsetReferenciaVencimento) >= dataInicial
                            && l.MesReferencia.AddMonths(l.CartaoCredito.OffsetReferenciaVencimento) <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoAtrasadosRelatorioProjecaoSaldo(int grupoId)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.GeradoLancamento
                            && l.MesReferencia.AddMonths(l.CartaoCredito!.OffsetReferenciaVencimento).AddDays(l.CartaoCredito.DiaVencimento - 1) < DateTime.Now.Date
                        )
                        .ToListAsync();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoPendentesRelatorioProjecaoSaldo(int grupoId, DateTime dataFinal)
        {
            return Db.LancamentosCartao
                        .Include(l => l.CartaoCredito)
                        .Where(o =>
                            o.GrupoId == grupoId
                            && !o.GeradoLancamento
                            && o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento).AddDays(o.CartaoCredito.DiaVencimento - 1) >= DateTime.Now.Date
                            && o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento).AddDays(o.CartaoCredito.DiaVencimento - 1) <= dataFinal
                        )
                        .ToListAsync();
        }

        public async Task<List<CartaoCreditoLimiteDto>> ListarCartoesLancamentosLimitesAsync(int grupoId, int? cartaoCreditoId)
        {
            var cartoesLancamentos = await (from c in Db.CartoesCredito
                                            join lc in Db.LancamentosCartao on c.Id equals lc.CartaoCreditoId into lcNull
                                            from lc in lcNull.DefaultIfEmpty()
                                            where
                                                c.GrupoId == grupoId
                                                && c.ExibirDadosRelatorio
                                                && c.Ativo
                                                && (cartaoCreditoId == null || c.Id == cartaoCreditoId.Value)
                                                && (lc == null || !lc.GeradoLancamento || (lc.Lancamento != null && !lc.Lancamento.Pago))
                                            group lc by new
                                            {
                                                CartaoCreditoId = c.Id,
                                                CartaoCredito = c.Descricao,
                                                c.ValorLimite
                                            } into g
                                            orderby g.Key.CartaoCredito
                                            select new
                                            {
                                                CartaoCreditoId = g.Key.CartaoCreditoId,
                                                CartaoCredito = g.Key.CartaoCredito,
                                                ValorLimite = g.Key.ValorLimite,
                                                ValorLimiteUtilizado = g.Sum(s => s.TipoLancamento == TipoLancamentoCartao.Despesa ? s.Valor : s.Valor * -1)
                                            })
                                            .ToListAsync();

            return cartoesLancamentos
                    .Select(g => new CartaoCreditoLimiteDto(
                        cartaoCreditoId: g.CartaoCreditoId,
                        cartaoCredito: g.CartaoCredito,
                        valorLimite: g.ValorLimite,
                        valorLimiteUtilizado: g.ValorLimiteUtilizado)
                    )
                    .ToList();
        }

        public Task<List<LancamentoCartao>> ListarLancamentosCartaoDoLancamentoFixoAsync(int lancamentoFixoId)
        {
            return Db.LancamentosCartao
                        .Include(l => l.Pessoa)
                        .Include(l => l.CartaoCredito)
                        .Where(l => l.LancamentoFixoOrigemId == lancamentoFixoId)
                        .ToListAsync();
        }

        public async Task IncluirAsync(LancamentoCartao lancamentoCartao)
        {
            await Db.LancamentosCartao.AddAsync(lancamentoCartao);
        }

        public Task AlterarAsync(LancamentoCartao lancamentoCartao)
        {
            return Task.Run(() => Db.LancamentosCartao.Update(lancamentoCartao));
        }

        public Task RemoverAsync(LancamentoCartao lancamentoCartao)
        {
            return Task.Run(() => Db.LancamentosCartao.Remove(lancamentoCartao));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var lancamentosCartao = await Db.LancamentosCartao.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.LancamentosCartao.RemoveRange(lancamentosCartao);
        }
    }
}