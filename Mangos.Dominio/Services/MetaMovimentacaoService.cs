using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models.Relatorios;
using Mangos.Dominio.Services.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class MetaMovimentacaoService
    {
        private readonly RelatorioProjecaoRealizacaoService _relatorioProjecaoRealizacaoService;
        private readonly IMetaMovimentacaoRepository _metaMovimentacaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MetaMovimentacaoService(RelatorioProjecaoRealizacaoService relatorioProjecaoRealizacaoService, IMetaMovimentacaoRepository metaMovimentacaoRepository, IUnitOfWork unitOfWork)
        {
            _relatorioProjecaoRealizacaoService = relatorioProjecaoRealizacaoService;
            _metaMovimentacaoRepository = metaMovimentacaoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ItemRelatorioBurnupModel>> GetRelatorioBurnup(int metaMovimentacaoId, int grupoId)
        {
            // busca os dados para o cálculo
            var metaMovimentacao = await _metaMovimentacaoRepository.ObterMetaMovimentacaoAsync(metaMovimentacaoId);

            if (metaMovimentacao is null)
                return new List<ItemRelatorioBurnupModel>();

            if (metaMovimentacao.GrupoId != grupoId)
                return new List<ItemRelatorioBurnupModel>();

            var itensRelatorio = new List<ItemRelatorioBurnupModel>();

            var relatorioProjecaoRealizacao = await _relatorioProjecaoRealizacaoService.ListarRelatorioProjecaoRealizacaoAsync(grupoId, metaMovimentacao.MesInicial, metaMovimentacao.MesFinal);

            // realiza o cálculo
            var diferencaMeses = ((metaMovimentacao.MesFinal.Year - metaMovimentacao.MesInicial.Year) * 12) + metaMovimentacao.MesFinal.Month - metaMovimentacao.MesInicial.Month + 1;

            double valorProjetado = 0;
            double valorAcumulado = 0;

            int ixMesAtual = 0;
            DateTime mesAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            for (int i = 0; i < diferencaMeses; i++)
            {
                var mes = metaMovimentacao.MesInicial.AddMonths(i);
                if (mes == mesAtual) ixMesAtual = i;

                var itemRelatorio = relatorioProjecaoRealizacao.Where(o => o.Mes == mes).FirstOrDefault();
                valorAcumulado += itemRelatorio != null ? (double)itemRelatorio.ValorTotal : 0;
                valorProjetado += (double)metaMovimentacao.ValorMensal;

                itensRelatorio.Add(new ItemRelatorioBurnupModel(
                    legenda: mes.ToString("MMM/yyyy"),
                    valorProjecao: valorProjetado,
                    valorRealizacao: valorAcumulado)
                );
            }

            //
            return itensRelatorio;
        }

        public async Task PersistirAsync(MetaMovimentacao metaMovimentacao)
        {
            if (metaMovimentacao.Id == 0)
                await _metaMovimentacaoRepository.IncluirAsync(metaMovimentacao);
            else
                await _metaMovimentacaoRepository.AlterarAsync(metaMovimentacao);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(MetaMovimentacao metaMovimentacao)
        {
            await _metaMovimentacaoRepository.RemoverAsync(metaMovimentacao);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}