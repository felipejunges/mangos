using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class GrupoService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GrupoService(IServiceProvider serviceProvider, IGrupoRepository grupoRepository, IUnitOfWork unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _grupoRepository = grupoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(Grupo grupo)
        {
            if (grupo.Id == 0)
                await _grupoRepository.IncluirAsync(grupo);
            else
                await _grupoRepository.AlterarAsync(grupo);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(Grupo grupo)
        {
            await _unitOfWork.BeginTransactionAsync();

            await _serviceProvider.GetRequiredService<ILancamentoFixoRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<ILancamentoCartaoRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<ILancamentoRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<ITransferenciaContaRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<IMetaMovimentacaoRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<IMetaSaldoRepository>().RemoverDoGrupoAsync(grupo);

            await _serviceProvider.GetRequiredService<IRendimentoMensalContaRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<ISaldoContaBancariaRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<IContaBancariaRepository>().RemoverDoGrupoAsync(grupo);
            
            await _serviceProvider.GetRequiredService<ICategoriaRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<ICartaoCreditoRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<IPessoaRepository>().RemoverDoGrupoAsync(grupo);
            await _serviceProvider.GetRequiredService<IUsuarioRepository>().RemoverDoGrupoAsync(grupo);
            
            await _grupoRepository.RemoverAsync(grupo);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}