using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class MetaSaldoService
    {
        private readonly IMetaSaldoRepository _metaSaldoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MetaSaldoService(IMetaSaldoRepository metaSaldoRepository, IUnitOfWork unitOfWork)
        {
            _metaSaldoRepository = metaSaldoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(MetaSaldo metaSaldo)
        {
            if (metaSaldo.Id == 0)
                await _metaSaldoRepository.IncluirAsync(metaSaldo);
            else
                await _metaSaldoRepository.AlterarAsync(metaSaldo);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(MetaSaldo metaSaldo)
        {
            await _metaSaldoRepository.RemoverAsync(metaSaldo);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}