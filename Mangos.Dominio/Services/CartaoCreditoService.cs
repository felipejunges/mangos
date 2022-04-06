using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class CartaoCreditoService
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CartaoCreditoService(ICartaoCreditoRepository cartaoCreditoRepository, IUnitOfWork unitOfWork)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(CartaoCredito cartaoCredito)
        {
            if (cartaoCredito.Id == 0)
                await _cartaoCreditoRepository.IncluirAsync(cartaoCredito);
            else
                await _cartaoCreditoRepository.AlterarAsync(cartaoCredito);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(CartaoCredito cartaoCredito)
        {
            await _cartaoCreditoRepository.RemoverAsync(cartaoCredito);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}