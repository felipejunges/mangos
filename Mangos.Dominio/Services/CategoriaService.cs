using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaService(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(Categoria categoria)
        {
            if (categoria.Id == 0)
                await _categoriaRepository.IncluirAsync(categoria);
            else
                await _categoriaRepository.AlterarAsync(categoria);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(Categoria categoria)
        {
            await _categoriaRepository.RemoverAsync(categoria);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}