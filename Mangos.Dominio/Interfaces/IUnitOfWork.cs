using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}