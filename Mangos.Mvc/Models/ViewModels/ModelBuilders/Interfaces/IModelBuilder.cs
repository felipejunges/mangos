using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces
{
    public interface IModelBuilder<TViewModel, TEntity>
    {
        Task<TViewModel?> CriarModelPeloIdAsync(int? id);
        Task AtualizarAsync(TViewModel model);
        Task<TEntity?> CriarEntidadeAsync(TViewModel model);
    }
}