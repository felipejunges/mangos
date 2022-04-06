using System.Threading.Tasks;
using Mangos.Dominio.Interfaces;

namespace Mangos.Infra.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MangosDb Db;

        public UnitOfWork(MangosDb db)
        {
            Db = db;
        }

        public async Task BeginTransactionAsync()
        {
            await Db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await Task.Run(() => Db.Database.CommitTransaction());
        }

        public async Task RollbackTransactionAsync()
        {
            await Task.Run(() => Db.Database.RollbackTransaction());
        }

        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}