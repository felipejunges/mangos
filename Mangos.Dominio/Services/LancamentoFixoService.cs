using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Extensions;
using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mangos.Dominio.Interfaces;

namespace Mangos.Dominio.Services
{
    public class LancamentoFixoService
    {
        private readonly ILancamentoFixoRepository _lancamentoFixoRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LancamentoFixoService(ILancamentoFixoRepository lancamentoFixoRepository, ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, IGrupoRepository grupoRepository, IUnitOfWork unitOfWork)
        {
            _lancamentoFixoRepository = lancamentoFixoRepository;
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _grupoRepository = grupoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LancamentoGeradoLancamentoFixoModel>> ObterLancamentosGeradosPeloLancamentoFixo(int grupoId, int lancamentoFixoId)
        {
            var lancamentoFixo = await _lancamentoFixoRepository.ObterLancamentoFixoAsync(lancamentoFixoId);

            if (lancamentoFixo == null || lancamentoFixo.GrupoId != grupoId)
                return new List<LancamentoGeradoLancamentoFixoModel>();

            var lancamentos = await _lancamentoRepository.ListarLancamentosDoLancamentoFixoAsync(lancamentoFixoId);
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoDoLancamentoFixoAsync(lancamentoFixoId);

            var lancamentosGerados = lancamentos
                .Select(o => new LancamentoGeradoLancamentoFixoModel(
                    id: o.Id,
                    tipo: o.Tipo.GetDescription(),
                    descricao: o.Descricao,
                    pessoa: o.PessoaId != null ? o.Pessoa!.Nome : string.Empty,
                    valor: o.Valor,
                    dataVencimento: o.DataVencimento
                ))
                .Union(lancamentosCartao.Select(o =>
                    new LancamentoGeradoLancamentoFixoModel(
                        id: o.Id,
                        tipo: o.TipoLancamento.GetDescription() + " cartão",
                        descricao: o.Descricao,
                        pessoa: o.PessoaId != null ? o.Pessoa!.Nome : string.Empty,
                        valor: o.Valor,
                        dataVencimento: o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento).AddDays(o.CartaoCredito.DiaVencimento - 1)
                    )
                ))
                .OrderBy(o => o.DataVencimento)
                .ToList();

            return lancamentosGerados;
        }

        public async Task PersistirAsync(LancamentoFixo lancamentoFixo)
        {
            if (lancamentoFixo.Id == 0)
                await _lancamentoFixoRepository.IncluirAsync(lancamentoFixo);
            else
                await _lancamentoFixoRepository.AlterarAsync(lancamentoFixo);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> Gerar(int? grupoId, int? id)
        {
            var lancamentosFixos = await _lancamentoFixoRepository.ListarLancamentosFixosGerarAsync(grupoId, id);

            int registrosGerados = 0;

            await _unitOfWork.BeginTransactionAsync();

            foreach (LancamentoFixo lancamentoFixo in lancamentosFixos)
            {
                if (lancamentoFixo.Tipo == TipoLancamentoFixo.Receita || lancamentoFixo.Tipo == TipoLancamentoFixo.Despesa)
                    registrosGerados += await this.GerarLancamentos(lancamentoFixo);
                else if (lancamentoFixo.Tipo == TipoLancamentoFixo.ReceitaCartao || lancamentoFixo.Tipo == TipoLancamentoFixo.DebitoCartao)
                    registrosGerados += await this.GerarLancamentosCartao(lancamentoFixo);
            }

            await _unitOfWork.CommitTransactionAsync();

            return registrosGerados;
        }

        private async Task<int> GerarLancamentos(LancamentoFixo lancamentoFixo)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(lancamentoFixo.GrupoId);

            if (grupo is null)
                return default;

            int registrosGerados = 0;
            List<DateTime> listaDatasGerar = GetDatasGerar(lancamentoFixo, grupo.MesesAntecedenciaGerarLancamento, false);

            if (listaDatasGerar != null && listaDatasGerar.Count > 0)
            {
                foreach (DateTime dataVencimento in listaDatasGerar)
                {
                    var lancamento = new Lancamento()
                    {
                        Id = 0,
                        GrupoId = lancamentoFixo.GrupoId,
                        DataHoraCadastro = DateTime.Now,
                        Tipo = lancamentoFixo.Tipo == TipoLancamentoFixo.Receita ? TipoLancamento.Receita : lancamentoFixo.Tipo == TipoLancamentoFixo.Despesa ? TipoLancamento.Despesa : 0,
                        Valor = lancamentoFixo.Valor,
                        DataVencimento = dataVencimento,
                        Descricao = lancamentoFixo.Descricao,
                        NumeroParcela = 1,
                        TotalParcelas = 1,
                        PessoaId = lancamentoFixo.PessoaId,
                        CategoriaId = lancamentoFixo.CategoriaId,
                        Pago = false,
                        LancamentoFixoOrigemId = lancamentoFixo.Id
                    };
                    await _lancamentoRepository.IncluirAsync(lancamento);
                    await _unitOfWork.SaveChangesAsync();

                    registrosGerados++;
                }

                DateTime ultimoDiaGerado = listaDatasGerar.Max(o => o);
                DateTime ultimoMesGerado = new DateTime(ultimoDiaGerado.Year, ultimoDiaGerado.Month, 1);

                lancamentoFixo.DataUltimoMesGerado = ultimoMesGerado;
                lancamentoFixo.DataHoraUltimaGeracao = DateTime.Now;

                await _lancamentoFixoRepository.AlterarAsync(lancamentoFixo);
                await _unitOfWork.SaveChangesAsync();
            }

            return registrosGerados;
        }

        private async Task<int> GerarLancamentosCartao(LancamentoFixo lancamentoFixo)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(lancamentoFixo.GrupoId);

            if (grupo is null)
                return default;

            int registrosGerados = 0;
            List<DateTime> listaDatasGerar = GetDatasGerar(lancamentoFixo, grupo.MesesAntecedenciaGerarLancamentoCartao, true);

            if (listaDatasGerar != null && listaDatasGerar.Count > 0)
            {
                foreach (DateTime mesReferencia in listaDatasGerar)
                {
                    if (lancamentoFixo.CartaoCreditoId is null)
                        continue;

                    var lancamentoCartao = new LancamentoCartao()
                    {
                        Id = 0,
                        GrupoId = lancamentoFixo.GrupoId,
                        DataHoraCadastro = DateTime.Now,
                        CartaoCreditoId = lancamentoFixo.CartaoCreditoId.Value,
                        TipoLancamento = lancamentoFixo.Tipo == TipoLancamentoFixo.ReceitaCartao ? TipoLancamentoCartao.Receita : lancamentoFixo.Tipo == TipoLancamentoFixo.DebitoCartao ? TipoLancamentoCartao.Despesa : 0,
                        Valor = lancamentoFixo.Valor,
                        MesReferencia = new DateTime(mesReferencia.Year, mesReferencia.Month, 1),
                        Descricao = lancamentoFixo.Descricao,
                        NumeroParcela = 1,
                        TotalParcelas = 1,
                        PessoaId = lancamentoFixo.PessoaId,
                        CategoriaId = lancamentoFixo.CategoriaId,
                        GeradoLancamento = false,
                        LancamentoFixoOrigemId = lancamentoFixo.Id
                    };
                    await _lancamentoCartaoRepository.IncluirAsync(lancamentoCartao);
                    await _unitOfWork.SaveChangesAsync();

                    registrosGerados++;
                }

                DateTime ultimoDiaGerado = listaDatasGerar.Max(o => o);
                DateTime ultimoMesGerado = new DateTime(ultimoDiaGerado.Year, ultimoDiaGerado.Month, 1);

                lancamentoFixo.DataUltimoMesGerado = ultimoMesGerado;
                lancamentoFixo.DataHoraUltimaGeracao = DateTime.Now;

                await _lancamentoFixoRepository.AlterarAsync(lancamentoFixo);
                await _unitOfWork.SaveChangesAsync();
            }

            return registrosGerados;
        }

        public async Task RemoverAsync(LancamentoFixo lancamentoFixo)
        {
            await _unitOfWork.BeginTransactionAsync();

            // TODO: testar se nao existe um jeito melhor de obter os objetos relacionados (usar o LazyLoad novamente, por ex)
            var lancamentos = await _lancamentoRepository.ListarLancamentosDoLancamentoFixoAsync(lancamentoFixo.Id);
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoDoLancamentoFixoAsync(lancamentoFixo.Id);

            for (int i = 1; i < lancamentos.Count(); i++)
            {
                var lancamento = lancamentos.ElementAt(i);
                lancamento.LancamentoFixoOrigemId = null;

                await _lancamentoRepository.AlterarAsync(lancamento);
                await _unitOfWork.SaveChangesAsync();
            }

            for (int i = 1; i < lancamentosCartao.Count(); i++)
            {
                var lancamentoCartao = lancamentosCartao.ElementAt(i);
                lancamentoCartao.LancamentoFixoOrigemId = null;

                await _lancamentoCartaoRepository.AlterarAsync(lancamentoCartao);
                await _unitOfWork.SaveChangesAsync();
            }

            await _lancamentoFixoRepository.RemoverAsync(lancamentoFixo);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }

        public List<DateTime> GetDatasGerar(LancamentoFixo lancamentoFixo, int mesesAntecedenciaGerar, bool gerarSomenteNoVencimento)
        {
            if (lancamentoFixo.Periodicidade == PeriodicidadeLancamentoFixo.Anual && lancamentoFixo.MesVencimento == null)
                return new List<DateTime>();

            List<DateTime> listaDatasGerar = new List<DateTime>();

            int mesComparar;
            int stepMensal;
            bool diaJaPassou;

            if (lancamentoFixo.Periodicidade == PeriodicidadeLancamentoFixo.Mensal)
            {
                diaJaPassou = lancamentoFixo.DiaVencimento < DateTime.Now.Day;
                mesComparar = DateTime.Now.Month;
                stepMensal = 1;
            }
            else
            {
                diaJaPassou = lancamentoFixo.MesVencimento!.Value < DateTime.Now.Month || (lancamentoFixo.MesVencimento.Value == DateTime.Now.Month && lancamentoFixo.DiaVencimento < DateTime.Now.Day);
                mesComparar = lancamentoFixo.MesVencimento.Value;
                stepMensal = 12;
            }

            var primeiroMesGerar =
                lancamentoFixo.DataUltimoMesGerado != null
                    ? lancamentoFixo.DataUltimoMesGerado.Value.AddMonths(stepMensal)
                    : new DateTime(DateTime.Now.Year, mesComparar, 1).AddMonths(diaJaPassou ? stepMensal : 0);

            var mes = primeiroMesGerar;
            int controle = 0;

            var mesHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            while (mes <= mesHoje.AddMonths(mesesAntecedenciaGerar))
            {
                controle += stepMensal;

                int diasNoMes = DateTime.DaysInMonth(mes.Year, mes.Month);

                DateTime data = lancamentoFixo.DiaVencimento > diasNoMes ? mes.AddDays(diasNoMes - 1) : mes.AddDays(lancamentoFixo.DiaVencimento - 1);

                if (!gerarSomenteNoVencimento || data <= DateTime.Now.AddMonths(mesesAntecedenciaGerar)) // para gerar somente no dia do vencimento
                    listaDatasGerar.Add(data);

                mes = primeiroMesGerar.AddMonths(controle);
            }

            return listaDatasGerar;
        }
    }
}