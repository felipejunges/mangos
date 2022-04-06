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
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly MangosDb Db;

        public LancamentoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Lancamento?> ObterLancamentoAsync(int id)
        {
            return Db.Lancamentos
                .Include(l => l.Pessoa)
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<Lancamento?> ObterLancamentoNoTrackingAsync(int id)
        {
            return Db.Lancamentos
                .Where(l => l.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosAsync(int grupoId, TipoLancamento tipoLancamento, string descricao, string tipoPesquisa, string pesquisa, DateTime dataInicial, DateTime dataFinal, LancamentoTipoDataPesquisa tipoData)
        {
            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Include(l => l.ContaBancaria)
                        .Include(l => l.Categoria)
                        #nullable disable
                        .ThenInclude(l => l.CategoriaSuperior)
                        #nullable enable
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.Tipo == tipoLancamento
                            && (string.IsNullOrEmpty(descricao) || l.Descricao.Contains(descricao))
                            && (
                                string.IsNullOrEmpty(pesquisa)
                                || (tipoPesquisa == "P" && l.Pessoa != null && l.Pessoa.Nome.Contains(pesquisa))
                                || (tipoPesquisa == "C" && l.Categoria != null && l.Categoria.Descricao.Contains(pesquisa))
                                || (tipoPesquisa == "O" && l.ContaBancaria != null && l.ContaBancaria.Descricao.Contains(pesquisa))
                            )
                            && (
                                (tipoData == LancamentoTipoDataPesquisa.Vencimento && l.DataVencimento >= dataInicial && l.DataVencimento <= dataFinal)
                                || (tipoData == LancamentoTipoDataPesquisa.Pagamento && l.DataPagamento >= dataInicial && l.DataPagamento <= dataFinal)
                            )
                        )
                        .OrderBy(l => tipoData == LancamentoTipoDataPesquisa.Vencimento ? l.DataVencimento : l.DataPagamento)
                        .ThenBy(l => tipoData == LancamentoTipoDataPesquisa.Vencimento ? l.DataPagamento : l.DataVencimento)
                        .ThenBy(l => l.DataHoraCadastro)
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosPeloAgrupadorAsync(int grupoId, TipoLancamento tipoLancamento, Guid agrupador)
        {
            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Include(l => l.ContaBancaria)
                        .Include(l => l.Categoria)
                        #nullable disable
                        .ThenInclude(l => l.CategoriaSuperior)
                        #nullable enable
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.Tipo == tipoLancamento
                            && l.Agrupador == agrupador
                        )
                        .OrderBy(l => l.DataVencimento)
                        .ThenBy(l => l.DataHoraCadastro)
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosAlertaAsync(int grupoId, int diasAntecedencia, TipoLancamento tipo)
        {
            DateTime dataVencimentoMaximo = DateTime.Now.Date.AddDays(diasAntecedencia);

            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.Tipo == tipo
                            && !l.Pago
                            && l.DataVencimento <= dataVencimentoMaximo
                        )
                        .OrderBy(o => o.DataVencimento)
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarDespesasPagasDaPessoaNoTrackingAsync(int grupoId, int pessoaId, DateTime dataMinima)
        {
            return Db.Lancamentos
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.PessoaId == pessoaId
                            && l.Tipo == TipoLancamento.Despesa
                            && l.DataPagamento != null
                            && l.DataPagamento >= dataMinima
                        )
                        .AsNoTracking()
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosDaPessoaAsync(int grupoId, int pessoaId)
        {
            return Db.Lancamentos
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.PessoaId == pessoaId
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosVencendoAsync(int grupoId, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.Pago
                            && l.DataVencimento <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && l.Pago
                            && l.ContaBancariaId != null
                            && l.DataPagamento != null
                            && l.DataContaBancaria != null
                            && l.ValorPago != null
                            && l.ContaBancariaId == contaBancariaId
                            && l.DataContaBancaria >= dataInicial
                            && l.DataContaBancaria <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosPendentesRelatorioProjecaoAsync(int grupoId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Include(l => l.Categoria)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.Pago
                            && (l.CategoriaId == null || l.Categoria!.RelatorioTotal)
                            && l.DataVencimento >= dataInicial
                            && l.DataVencimento <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosPagosRelatorioProjecaoAsync(int grupoId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Include(l => l.Categoria)
                        .Where(o =>
                            o.GrupoId == grupoId
                            && o.Pago
                            && (o.CategoriaId == null || o.Categoria!.RelatorioTotal)
                            && o.ValorPago != null
                            && o.DataPagamento != null
                            && o.DataPagamento >= dataInicial
                            && o.DataPagamento <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Include(l => l.Categoria)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && (situacao == "T" || (situacao == "R" && l.Pago) || (situacao == "P" && !l.Pago))
                            && (tipo == "T" || (tipo == "R" && l.Tipo == TipoLancamento.Receita) || (tipo == "D" && l.Tipo == TipoLancamento.Despesa))
                            && l.CategoriaId != null
                            && l.Categoria!.RelatorioTotal
                            && (
                                (l.Pago && l.DataPagamento >= dataInicial && l.DataPagamento <= dataFinal)
                                || (!l.Pago && l.DataVencimento >= dataInicial && l.DataVencimento <= dataFinal)
                            )
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosDoLancamentoFixoAsync(int lancamentoFixoId)
        {
            return Db.Lancamentos
                        .Include(l => l.Pessoa)
                        .Where(l => l.LancamentoFixoOrigemId == lancamentoFixoId)
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosAtrasadosRelatorioProjecaoSaldo(int grupoId)
        {
            return Db.Lancamentos
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.Pago
                            && l.DataVencimento < DateTime.Now.Date
                        )
                        .ToListAsync();
        }

        public Task<List<Lancamento>> ListarLancamentosPendentesRelatorioProjecaoSaldo(int grupoId, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Where(l =>
                            l.GrupoId == grupoId
                            && !l.Pago
                            && l.DataVencimento >= DateTime.Now.Date
                            && l.DataVencimento <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<DateTime?> ObterMenorDataDaContaBancariaAsync(int contaBancariaId)
        {
            return Db.Lancamentos.Where(l => l.ContaBancariaId == contaBancariaId && l.DataContaBancaria != null).MinAsync(l => l.DataContaBancaria);
        }

        public Task<decimal> ObterValorLancamentosNoPeriodoAsync(int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Lancamentos
                        .Where(l =>
                            l.Pago
                            && l.ContaBancariaId == contaBancariaId
                            && l.DataContaBancaria != null
                            && l.DataContaBancaria >= dataInicial
                            && l.DataContaBancaria <= dataFinal
                            && l.ValorPago != null
                        )
                        .SumAsync(l =>
                                    l.Tipo == TipoLancamento.Receita
                                        ? l.ValorPago!.Value
                                        : l.Tipo == TipoLancamento.Despesa
                                            ? l.ValorPago!.Value * -1
                                            : 0);
        }

        public async Task IncluirAsync(Lancamento lancamento)
        {
            await Db.Lancamentos.AddAsync(lancamento);
        }

        public Task AlterarAsync(Lancamento lancamento)
        {
            return Task.Run(() => Db.Lancamentos.Update(lancamento));
        }

        public Task RemoverAsync(Lancamento lancamento)
        {
            return Task.Run(() => Db.Lancamentos.Remove(lancamento));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var lancamentos = await Db.Lancamentos.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.Lancamentos.RemoveRange(lancamentos);
        }
    }
}