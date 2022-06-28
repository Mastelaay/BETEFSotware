using System.Data.Entity;

namespace BET.Domain
{
    public class BETUnitOfWork : IBETUnitOfWork
    {
        public DbContext DbContext { get; private set; }

        public BETUnitOfWork(DbContext context)
        {
            DbContext = context;
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public IBETRepository<T> GetRepo<T>() where T : class
        {
            return new BETRepository<T>(DbContext);
        }
    }
}
