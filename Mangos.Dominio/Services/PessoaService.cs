using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class PessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PessoaService(IPessoaRepository pessoaRepository, ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PessoaConsultaLancamentoModel>> ListarPessoaConsultaLancamentosAsync(int grupoId, int pessoaId)
        {
            var lancamentos = await _lancamentoRepository.ListarLancamentosDaPessoaAsync(grupoId, pessoaId);
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoDaPessoaAsync(grupoId, pessoaId);

            var lancamentosModel = lancamentos
                    .Select(l => new PessoaConsultaLancamentoModel(
                        descricao: l.DescricaoComParcelas,
                        tipo: l.Tipo == TipoLancamento.Receita ? "Receita" : "Despesa",
                        dataVencimento: l.DataVencimento,
                        dataPagamento: l.DataPagamento,
                        valor: l.Valor)
                    );

            var lancamentosCartaoModel = lancamentosCartao
                    .Select(l => new PessoaConsultaLancamentoModel(
                        descricao: l.DescricaoComParcelas,
                        tipo: l.TipoLancamento == TipoLancamentoCartao.Receita ? "Receita cartão" : "Despesa cartão",
                        dataVencimento: l.MesReferencia.AddMonths(l.CartaoCredito!.OffsetReferenciaVencimento).AddDays(l.CartaoCredito!.DiaVencimento - 1),
                        dataPagamento: l.Lancamento != null ? l.Lancamento.DataPagamento : null,
                        valor: l.Valor)
                    );

            var pessoaLancamentos =
                    lancamentosModel
                        .Concat(lancamentosCartaoModel)
                        .OrderBy(o => o.DataVencimento)
                        .ThenBy(o => o.DataPagamento)
                        .ToList();

            return pessoaLancamentos;
        }

        public async Task PersistirAsync(Pessoa pessoa)
        {
            if (pessoa.Id == 0)
                await _pessoaRepository.IncluirAsync(pessoa);
            else
                await _pessoaRepository.AlterarAsync(pessoa);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(Pessoa pessoa)
        {
            await _pessoaRepository.RemoverAsync(pessoa);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}